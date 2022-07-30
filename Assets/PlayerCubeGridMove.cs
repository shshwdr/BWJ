using PixelCrushers.DialogueSystem;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
public struct VisuallyPosition
{
    public Vector3 position;
    public Vector3 right;
    public VisuallyPosition(Vector3 p, Vector3 u)
    {
        position = p;
        right = u;
    }
}
public class PlayerCubeGridMove : MonoBehaviour
{
    public bool isAuto = false;
    int autoStep = 0;
    SerializedHint currentHint;
    public class PlayerMoveState
    {
        //public Transform transform = new GameObject().transform;

        public bool ignoreNextSign;
        public bool turnAroundNext;
        public bool swimNext;

        public bool isSwiming;
        public bool lastIsMoveBack;

        public Transform targetTransform;

        public void copy(PlayerMoveState state)
        {
            //transform.position = state.transform.position;
            //transform.rotation = state.transform.rotation;
            targetTransform.position = state.targetTransform.position;
            targetTransform.rotation = state.targetTransform.rotation;

            ignoreNextSign = state.ignoreNextSign;
            turnAroundNext = state.turnAroundNext;
            swimNext = state.swimNext;
            isSwiming = state.isSwiming;
            lastIsMoveBack = state.lastIsMoveBack;
        }

        public PlayerMoveState()
        {
            targetTransform = new GameObject().transform;
        }
    }

    public PlayerMoveState moveState;

    public float gridSize = 1;
    public float moveSpeed = 3;
    public float rotateSpeed = 10;
    public LayerMask walkableLayer;
    public LayerMask swimLayer;
    public LayerMask signLayer;
    public LayerMask teleportLayer;

    public LayerMask collectableLayer;
    public LayerMask ladderLayer;
    public LayerMask endLayer;
    public LayerMask leverLayer;
    public LayerMask tutorialLayer;
    public LayerMask validationPadLayer;
    public bool startedMoving = false;
    public Transform frontDetection;
    public float rotateCoolDown = 0.1f;
    float rotateCoolDownTimer = 100;
    List<Vector3> nextPositions = new List<Vector3>();

    public List<VisuallyPosition> visuallyNextPositions = new List<VisuallyPosition>();
    List<Vector3> visuallyNextUps = new List<Vector3>();
    List<Quaternion> nextRotations = new List<Quaternion>();
    public float stopDistance = 0.001f;


    PlayerMoveState tempMoveState;

    int simulateCount = 3;

    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        // Time.timeScale = 2;
        if (StageLevelManager.Instance.playHintNext)
        {
            isAuto = true;
            autoStep = 0;
        }
    }

    public void stopAuto(){
        isAuto = false;
        
        }


    public void startPosition(Vector3 position, Quaternion rotation)
    {
        Utils.gridSize = gridSize / 4f;
        transform.position = position;
        transform.rotation = rotation;
        moveState = new PlayerMoveState();

        moveState.targetTransform.position = transform.position;
        moveState.targetTransform.rotation = transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("StartGame", startMove);
        currentHint = HintSaveLoad.Load(StageLevelManager.Instance.currentLevelId);

    }

    void swimInMove(RaycastHit hit, ref PlayerMoveState state, bool isSimulating)
    {
        if (1 << hit.collider.gameObject.layer == swimLayer)
        {

            if (startedMoving)
            {
                state.isSwiming = true;
                if (!isSimulating)
                {
                    animator.SetBool("swim", true);

                    FMODUnity.RuntimeManager.PlayOneShot("event:/in water 2");
                }
            }
        }
        else
        {
            if (startedMoving)
            {
                if (!isSimulating)
                {
                    animator.SetBool("swim", false);
                    animator.SetBool("walk", true);
                }
                if (state.isSwiming)
                {
                    //swimNext = false;
                    state.isSwiming = false;
                    if (!isSimulating)
                    {
                        FMODUnity.RuntimeManager.PlayOneShot("event:/iout water");
                    }

                    //EventPool.Trigger<int>("turnedInstructionOff", 3);
                }
            }
        }
    }

    void addMovePosition(ref PlayerMoveState state, ref List<VisuallyPosition> visuallyNextPositions, Vector3 nextPosition, Quaternion targetRotation, bool isSimulating, Vector3 up, Vector3 right)
    {
        addMovePosition(ref state, ref visuallyNextPositions, nextPosition, targetRotation, isSimulating, up, right, Quaternion.identity);
    }

    void addMovePosition(ref PlayerMoveState state, ref List<VisuallyPosition> visuallyNextPositions, Vector3 nextPosition, Quaternion targetRotation, bool isSimulating, Vector3 up, Vector3 right, Quaternion realRight)
    {
        if (!isSimulating)
        {
            nextPositions.Add(nextPosition);
        }
        state.targetTransform.position = nextPosition;
        visuallyNextPositions.Add(new VisuallyPosition(nextPosition + up * 0.1f, right));

        targetRotation *= realRight;
        state.targetTransform.rotation = targetRotation;

        if (!isSimulating)
        {
            nextRotations.Add(targetRotation);
        }
    }

    bool canMove(ref PlayerMoveState state, Vector3 dir, ref List<VisuallyPosition> visuallyNextPositions, bool isSimulating, bool forceSwim = false)
    {
        var visual = new List<VisuallyPosition>();
        bool res = canMove_internal(ref state, dir, ref visual, isSimulating, forceSwim);
        if (res)
        {
            foreach (var v in visual)
            {
                visuallyNextPositions.Add(v);
            }
        }
        else
        {
            res = res;
        }
        return res;
    }

    bool canMove_internal(ref PlayerMoveState state, Vector3 dir, ref List<VisuallyPosition> visuallyNextPositions, bool isSimulating, bool forceSwim = false)
    {

        LayerMask canWalkLayer = walkableLayer;
        if (state.swimNext || forceSwim)
        {
            canWalkLayer |= swimLayer;
        }
        //var originRight = state.targetTransform.right;
        state.targetTransform.rotation *= Quaternion.Euler(dir);

        var right = state.targetTransform.right;
        //if(right != originRight)
        //{
        //    right += originRight;
        //}
        Quaternion targetRotation = state.targetTransform.rotation;

        //check if there is a wall in front

        var nextPosition1 = Utils.snapToGrid(state.targetTransform.position + targetRotation * Vector3.forward * gridSize * 0.33f);
        bool hitRoad1 = Physics.Raycast(nextPosition1 + state.targetTransform.up * 0.5f, -state.targetTransform.up, 1, canWalkLayer);


        //check if there is a ladder to the direction
        bool hitInfront = Physics.Raycast(state.targetTransform.position + state.targetTransform.up * 0.5f, state.targetTransform.forward, 0.5f);
        if (hitInfront)
        {

            bool hitLadderInfront = Physics.Raycast(state.targetTransform.position + state.targetTransform.up * 0.5f, state.targetTransform.forward, 0.5f, ladderLayer);
            var positionAfterLadder = Utils.snapToGrid(state.targetTransform.position + state.targetTransform.forward + state.targetTransform.up);
            var roadPositionJustBeforeLadder = Utils.snapToGrid(positionAfterLadder - state.targetTransform.forward * 0.33f);
            bool hasRoadAfterLadder = Physics.Raycast(positionAfterLadder + state.targetTransform.up * 0.5f, -state.targetTransform.up, 1, canWalkLayer);
            bool hasRoadAfterLadder2 = Physics.Raycast(roadPositionJustBeforeLadder + state.targetTransform.up * 0.5f, -state.targetTransform.up, 1, canWalkLayer);
            if (hitLadderInfront)
            {

                if (hasRoadAfterLadder && hasRoadAfterLadder2 && hitRoad1)
                {
                    if (state.ignoreNextSign)
                    {
                        state.ignoreNextSign = false;

                        EventPool.Trigger<int>("turnedInstructionOff", 2);
                    }
                    else
                    {
                        addMovePosition(ref state, ref visuallyNextPositions, state.targetTransform.position + state.targetTransform.forward * 0.5f, targetRotation, isSimulating, state.targetTransform.up, right);
                        addMovePosition(ref state, ref visuallyNextPositions, state.targetTransform.position + state.targetTransform.up, targetRotation, isSimulating, state.targetTransform.up, right);
                        addMovePosition(ref state, ref visuallyNextPositions, state.targetTransform.position + state.targetTransform.forward * 0.5f, targetRotation, isSimulating, state.targetTransform.up, right);

                        return true;
                    }
                }
            }
            else
            {
                //hit wall, give up
                state.targetTransform.rotation *= Quaternion.Euler(-dir);
                return false;
            }
        }




        //check if next grid is moveable
        var nextPosition = Utils.snapToGrid(state.targetTransform.position + targetRotation * Vector3.forward * gridSize);
        var nextPosition2 = Utils.snapToGrid(state.targetTransform.position + targetRotation * Vector3.forward * gridSize * 0.66f);
        RaycastHit hitTest;
        bool hitAny = Physics.Raycast(nextPosition + state.targetTransform.up * 0.5f, -state.targetTransform.up,out hitTest, 1, ~(ladderLayer| validationPadLayer));
        if (hitAny)
        {

            //if next is hitted, check if hit on road
            RaycastHit hit;
            bool hitRoad = Physics.Raycast(nextPosition + state.targetTransform.up * 0.5f, -state.targetTransform.up, out hit, 1, canWalkLayer);
            bool hitRoad2 = Physics.Raycast(nextPosition2 + state.targetTransform.up * 0.5f, -state.targetTransform.up, 1, canWalkLayer);
            if (hitRoad && hitRoad1 && hitRoad2)
            {

                addMovePosition(ref state, ref visuallyNextPositions, nextPosition, targetRotation, isSimulating, state.targetTransform.up, right);
                swimInMove(hit, ref state, isSimulating);
                return true;
            }
            else
            {
                //if (startedMoving)
                //{
                //    animator.SetBool("walk", false);
                //}

                if (startedMoving)
                {
                    if (!isSimulating)
                    {
                        animator.SetBool("walk", false);
                    }

                }
                state.targetTransform.rotation *= Quaternion.Euler(-dir);
                return false;
            }
        }
        else
        {
            //if not hit
            //check if there is a ladder to get down
            bool hitDownStair = Physics.Raycast(state.targetTransform.position + state.targetTransform.forward - state.targetTransform.up * 0.5f, -state.targetTransform.forward, 0.5f, ladderLayer);


            //bool hitLadderInfront = Physics.Raycast(state.targetTransform.position + state.targetTransform.up * 0.5f, state.targetTransform.forward, 1, ladderLayer);
            var positionAfterLadder = Utils.snapToGrid(state.targetTransform.position + state.targetTransform.forward - state.targetTransform.up);
            var roadPositionJustBeforeLadder = Utils.snapToGrid(positionAfterLadder - state.targetTransform.forward * 0.33f);
            bool hasRoadAfterLadder = Physics.Raycast(positionAfterLadder + state.targetTransform.up * 0.5f, -state.targetTransform.up, 1, canWalkLayer);
            bool hasRoadAfterLadder2 = Physics.Raycast(roadPositionJustBeforeLadder + state.targetTransform.up * 0.5f, -state.targetTransform.up, 1, canWalkLayer);

            if (hitDownStair && hasRoadAfterLadder && hasRoadAfterLadder2 && hitRoad1)
            {
                if (state.ignoreNextSign)
                {
                    state.ignoreNextSign = false;

                    EventPool.Trigger<int>("turnedInstructionOff", 2);
                }
                else
                {
                    addMovePosition(ref state, ref visuallyNextPositions, state.targetTransform.position + state.targetTransform.forward * 0.5f, targetRotation, isSimulating, state.targetTransform.up, right);
                    addMovePosition(ref state, ref visuallyNextPositions, state.targetTransform.position - state.targetTransform.up, targetRotation, isSimulating, state.targetTransform.up, right);
                    addMovePosition(ref state, ref visuallyNextPositions, state.targetTransform.position + state.targetTransform.forward * 0.5f, targetRotation, isSimulating, state.targetTransform.up, right);
                    return true;
                }
            }



            // check if there is a angle to rotate

            RaycastHit hit;
            nextPosition = Utils.snapToGrid(state.targetTransform.position + targetRotation * Vector3.forward * gridSize * 0.5f);
            nextPosition1 = Utils.snapToGrid(state.targetTransform.position + targetRotation * Vector3.forward * gridSize * 0.33f);
            var nnPosition = Utils.snapToGrid(nextPosition - state.targetTransform.up * gridSize * 0.5f);
            nextPosition2 = Utils.snapToGrid(nnPosition + state.targetTransform.up * gridSize * 0.33f);
            //if next is hitted, check if hit on ground
            bool hitRoad = Physics.Raycast(nnPosition + targetRotation * Vector3.forward * 0.5f, -(targetRotation * Vector3.forward), out hit, 1, canWalkLayer);
            //bool hitRoad1 = Physics.Raycast(nextPosition1 + targetRotation * Vector3.up * 0.5f, -(targetRotation * Vector3.up), 1, canWalkLayer);
            bool hitRoad2 = Physics.Raycast(nextPosition2 + targetRotation * Vector3.forward * 0.5f, -(targetRotation * Vector3.forward), 1, canWalkLayer);

            //or check if hit on ladder

            if (hitRoad && hitRoad1 && hitRoad2)
            {
                addMovePosition(ref state, ref visuallyNextPositions, nextPosition, targetRotation, isSimulating, state.targetTransform.up + state.targetTransform.forward, right);
                //if (!isSimulating)
                //{
                //    nextPositions.Add(nextPosition);
                //}

                //state.targetTransform.position = nextPosition;
                //visuallyNextPositions.Add(new VisuallyPosition( nextPosition + state.targetTransform.up * 0.1f + state.targetTransform.forward * 0.1f, state.targetTransform.right));

                //if (!isSimulating)
                //{
                //    nextRotations.Add(targetRotation);
                //}



                addMovePosition(ref state, ref visuallyNextPositions, nnPosition, targetRotation, isSimulating, state.targetTransform.forward, right, Quaternion.Euler(Vector3.right * 90));
                //if (!isSimulating)
                //{
                //    nextPositions.Add(nnPosition);
                //}

                //state.targetTransform.position = nnPosition;
                //visuallyNextPositions.Add(new VisuallyPosition(nnPosition + state.targetTransform.forward * 0.1f, state.targetTransform.right));
                //state.targetTransform.rotation *= Quaternion.Euler(Vector3.right * 90);

                //if (!isSimulating)
                //{
                //    nextRotations.Add(state.targetTransform.rotation);
                //}
                swimInMove(hit, ref state, isSimulating);
                return true;
            }
            else
            {
                if (startedMoving)
                {
                    if (!isSimulating)
                    {
                        animator.SetBool("walk", false);
                    }

                }
                state.targetTransform.rotation *= Quaternion.Euler(-dir);
                return false;
            }
        }
    }


    void moveBack(ref PlayerMoveState state, ref List<VisuallyPosition> visuallyNextPositions, bool isSimulating)
    {
        state.lastIsMoveBack = true;
        var targetTransform = state.targetTransform;


        var fakeList = new List<VisuallyPosition>();
        addMovePosition(ref state, ref fakeList, targetTransform.position, targetTransform.rotation, isSimulating, state.targetTransform.up, targetTransform.right, Quaternion.Euler(Vector3.up * 90));

        addMovePosition(ref state, ref visuallyNextPositions, targetTransform.position, targetTransform.rotation, isSimulating, state.targetTransform.up, targetTransform.right, Quaternion.Euler(Vector3.up * 90));

        //if (!isSimulating)
        //{
        //    nextPositions.Add(targetTransform.position);
        //}
        //targetTransform.rotation *= Quaternion.Euler(Vector3.up * 90);
        //visuallyNextPositions.Add(new VisuallyPosition(targetTransform.position + targetTransform.up * 0.1f, targetTransform.right));
        //if (!isSimulating)
        //{
        //    nextRotations.Add(targetTransform.rotation);
        //    nextPositions.Add(targetTransform.position);
        //}

        //targetTransform.rotation *= Quaternion.Euler(Vector3.up * 90);
        //if (!isSimulating)
        //{
        //    nextRotations.Add(targetTransform.rotation);
        //}
    }

    public void moveNextMove()
    {
        nextPositions.Clear();
        visuallyNextPositions.Clear();
        nextRotations.Clear();


        decideNextMove();
        Assert.AreEqual(nextPositions.Count, nextRotations.Count);
        for (int i = 0; i < nextPositions.Count; i++)
        {

            transform.position = nextPositions[i];
            transform.rotation = nextRotations[i];
        }
        moveState.targetTransform.position = transform.position;
        moveState.targetTransform.rotation = transform.rotation;
    }

    public void updateOtherData()
    {

        moveState.targetTransform.position = transform.position;
        moveState.targetTransform.rotation = transform.rotation;
    }



    static void simulate(ref List<VisuallyPosition> visuallyNextPositions, PlayerCubeGridMove playerMove, PlayerMoveState moveState, int simulateStepCount)
    {
        if (playerMove.tempMoveState == null)
        {
            playerMove.tempMoveState = new PlayerMoveState();
        }
        playerMove.tempMoveState.copy(moveState);
        while (simulateStepCount > 0)
        {
            simulateStepCount--;
            calculateNextMove(playerMove, ref playerMove.tempMoveState, ref visuallyNextPositions, true);
        }
    }

    static void calculateNextMove(PlayerCubeGridMove playerMove, ref PlayerMoveState moveState, ref List<VisuallyPosition> visuallyNextPositions, bool isInSimulating)
    {
        var transform = moveState.targetTransform;
        var signLayer = playerMove.signLayer;
        var teleportLayer = playerMove.teleportLayer;
        var leverLayer = playerMove.leverLayer;
        var ignoreNextSign = moveState.ignoreNextSign;
        var isSwiming = moveState.isSwiming;
        var lastIsMoveBack = moveState.lastIsMoveBack;


        //if force turn around
        if (moveState.turnAroundNext)
        {
            LogManager.log("used skill to move back");
            playerMove.moveBack(ref moveState, ref visuallyNextPositions, isInSimulating);
            moveState.turnAroundNext = false;

            //simulate(ref visuallyNextPositions, this, ref moveState, 3);
            if (!isInSimulating)
            {

                EventPool.Trigger<int>("turnedInstructionOff", 1);
            }
            return;
        }

        // if on a teleport, find teleport to mvoe
        RaycastHit teleportItem;
        bool hitTeleport = Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, out teleportItem, 1, teleportLayer);
        if (hitTeleport)
        {
            if (ignoreNextSign)
            {
                ignoreNextSign = false;

                EventPool.Trigger<int>("turnedInstructionOff", 2);
            }
            else
            {
                //find another teleport on this face
                playerMove.addMovePosition(ref moveState, ref visuallyNextPositions, teleportItem.collider.GetComponent<Teleport>().teleportTarget.position, moveState.targetTransform.rotation, isInSimulating, moveState.targetTransform.up, moveState.targetTransform.right);
                //if (teleportItem.transform.parent.parent.GetComponentsInChildren<Teleport>().Length == 2)
                //{
                return;
                //}
                //else
                //{
                //    Debug.LogError("teleport should appear in pair");
                //}


                //if (playerMove.canMove(ref moveState, Vector3.up * -90, ref playerMove.visuallyNextPositions, isInSimulating, isSwiming))
                //{
                //    if (!isInSimulating)
                //    {
                //        FMODUnity.RuntimeManager.PlayOneShot("event:/sign");
                //    }
                //    return;
                //}
            }

        }


        //if on a turning sign, force turn, unless has ignore next sign
        bool hitSign = Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, 1, signLayer);
        if (hitSign)
        {
            if (ignoreNextSign)
            {
                ignoreNextSign = false;

                EventPool.Trigger<int>("turnedInstructionOff", 2);
            }
            else
            {

                if (playerMove.canMove(ref moveState, Vector3.up * -90, ref playerMove.visuallyNextPositions, isInSimulating, isSwiming))
                {
                    if (!isInSimulating)
                    {
                        FMODUnity.RuntimeManager.PlayOneShot("event:/sign");
                    }
                    return;
                }
            }

        }


        //todo improve this
        // pull a level if needed
        // if rotating, wont pull the level twice

        if (!isInSimulating && playerMove.startedMoving)
        {
            if (!lastIsMoveBack)
        {
            //check if has lever
            RaycastHit levelHit;
            bool hitLever = Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, out levelHit, 1, leverLayer);
            if (hitLever)
            {
                if (ignoreNextSign)
                {
                    ignoreNextSign = false;

                    EventPool.Trigger<int>("turnedInstructionOff", 2);
                }
                else
                {
                    levelHit.collider.GetComponent<Lever>().rotate();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/lever");
                    }
                }
            }
        }

        moveState.lastIsMoveBack = false;




        if (playerMove.canMove(ref moveState, Vector3.zero, ref playerMove.visuallyNextPositions, isInSimulating))
        {
            LogManager.log("move forward next");
        }
        else if (playerMove.canMove(ref moveState, Vector3.up * 90, ref playerMove.visuallyNextPositions, isInSimulating))
        {

            LogManager.log("move right next");
        }
        else if (playerMove.canMove(ref moveState, -Vector3.up * 90, ref playerMove.visuallyNextPositions, isInSimulating))
        {
            LogManager.log("move left next");
        }
        else if (playerMove.canMove(ref moveState, Vector3.zero, ref playerMove.visuallyNextPositions, isInSimulating, isSwiming))
        {

            LogManager.log("swim forward next");
        }
        else if (playerMove.canMove(ref moveState, Vector3.up * 90, ref playerMove.visuallyNextPositions, isInSimulating, isSwiming))
        {

            LogManager.log("swim right next");
        }
        else if (playerMove.canMove(ref moveState, -Vector3.up * 90, ref playerMove.visuallyNextPositions, isInSimulating, isSwiming))
        {

            LogManager.log("swim left next");
        }
        else
        {
            LogManager.log("move back next");
            playerMove.moveBack(ref moveState, ref playerMove.visuallyNextPositions, isInSimulating);
        }
    }

    public void decideNextMove()
    {
        visuallyNextPositions.Clear();
        visuallyNextPositions.Add(new VisuallyPosition(moveState.targetTransform.position + moveState.targetTransform.up * 0.1f, moveState.targetTransform.right));
        //if has collectable, collect


        if (isAuto)
        {
            if (currentHint != null && autoStep < currentHint.actionList.Count)
            {
                var nextAction = currentHint.actionList[autoStep];
                LevelValidation.playerMoveBasedOnHint(this, nextAction);
                autoStep++;
            }

        }


        RaycastHit hitedCollectable = new RaycastHit();
        bool hitCollectable = Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, out hitedCollectable, 1, collectableLayer);

        if (hitCollectable)
        {
            if (startedMoving)
            {
                Destroy(hitedCollectable.collider.gameObject);
                FMODUnity.RuntimeManager.PlayOneShot("event:/collect 2");
            }
            StageLevelManager.Instance.addCollectable();
        }

        // if has tutorial, unlock tutorial
        if (startedMoving)
        {
            RaycastHit tutorialHit;
            bool hitTutorial = Physics.Raycast(moveState.targetTransform.position + moveState.targetTransform.up * 0.5f, -moveState.targetTransform.up, out tutorialHit, 1, tutorialLayer);
            if (hitTutorial)
            {

                TutorialManager.Instance.unlockTutorial(tutorialHit.collider.GetComponent<TutorialGiver>().tutorialString);
            }
        }

        calculateNextMove(this, ref moveState, ref visuallyNextPositions, false);
        if (startedMoving)
        {

            simulate(ref visuallyNextPositions, this, moveState, simulateCount);
        }
        else
        {

            simulate(ref visuallyNextPositions, this, moveState, 0);
        }


    }

    public void simulateByAction()
    {
        //don't change position until the one the same to next position, then simulate
        int i = 0;
        for (; i < visuallyNextPositions.Count; i++)
        {
            if ((visuallyNextPositions[i].position - moveState.targetTransform.position).magnitude < 0.3f)
            {
                i++;
                break;
            }
        }
        while (visuallyNextPositions.Count > i)
        {
            visuallyNextPositions.RemoveAt(i);
        }
        //tempMoveState.copy(moveState);
        simulate(ref visuallyNextPositions, this, moveState, simulateCount);
    }

    public bool gameEnd()
    {

        bool hitSign = Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, 1, endLayer);
        if (hitSign)
        {
            if (startedMoving)
            {

                animator.SetTrigger("victory");
                if (StageLevelManager.Instance.showDialogue)
                {
                    if (StageLevelManager.Instance.collectedAllInLevel())
                    {

                        DialogueManager.StartConversation($"{StageLevelManager.Instance.currentLevel.id}_end");
                    }
                    else
                    {

                        DialogueManager.StartConversation($"unfinished");
                    }
                }
                startedMoving = false;

                StageLevelManager.Instance.finishLevel();
            }


            return true;
        }
        return false;



    }

    public LevelValidationPadView getPadStandingOn()
    {

        RaycastHit hitTest;
        bool test = Physics.Raycast(moveState.targetTransform.position + moveState.targetTransform.up * 0.5f, -moveState.targetTransform.up, out hitTest, 1, validationPadLayer);
        if (test)
        {
            return hitTest.collider.GetComponent<LevelValidationPadView>();
        }
        Debug.LogError("why no pad standing on?");
        return null;
    }

    public void turnAround()
    {
        moveState.turnAroundNext = !moveState.turnAroundNext;
        simulateByAction();
    }
    public void ignoreSign()
    {
        moveState.ignoreNextSign = !moveState.ignoreNextSign;
        simulateByAction();
    }
    public void swim()
    {
        moveState.swimNext = !moveState.swimNext;
        simulateByAction();
    }
    public void startMove()
    {
        StartCoroutine(waitStartMove(2));

    }
    IEnumerator waitStartMove(float time)
    {
        yield return new WaitForSeconds(time);
        startedMoving = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale *= 2;
            if (Time.timeScale > 4)
            {
                Time.timeScale = 1;
            }
        }

        if (startedMoving)
        {
            rotateCoolDownTimer += Time.deltaTime;
            if (nextPositions.Count == 0)
            {
                if (gameEnd())
                {

                    return;
                }
                else
                {

                    decideNextMove();
                }
            }
            if (nextPositions.Count != nextRotations.Count)
            {
                Debug.LogError("positions not the same count");
            }
            if (nextPositions.Count > 0)
            {
                //move
                transform.Translate((nextPositions[0] - transform.position).normalized * moveSpeed * Time.deltaTime, Space.World);
                float donePercentage = Mathf.Min(1F, Time.deltaTime / (moveSpeed));

                if (nextRotations.Count == 0)
                {
                    Debug.LogError("hmmm?");
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, nextRotations[0], donePercentage);
                //var deltaQuaternion = transform.rotation * Quaternion.Inverse(nextRotations[0]);
                // transform.Rotate(deltaQuaternion.eulerAngles*Time.deltaTime* moveSpeed, Space.World);
                // transform.rotation = Quaternion.Slerp(transform.rotation, nextRotations[0], Time.deltaTime * (1 / rotateCoolDown));
                if ((nextPositions[0] - transform.position).sqrMagnitude <= stopDistance && rotateCoolDownTimer >= rotateCoolDown)
                {
                    moveState.targetTransform.position = nextPositions[0];
                    nextPositions.RemoveAt(0);
                    visuallyNextPositions.RemoveAt(0);
                    moveState.targetTransform.rotation = nextRotations[0];
                    nextRotations.RemoveAt(0);
                    rotateCoolDownTimer = 0;
                }


            }

        }
    }


    void calculateMoreSteps()
    {
        //  moreNextPositions
    }

    public static List<Vector3> improveVisualPoints(List<VisuallyPosition> visuallyNextPositions)
    {
        var res = new List<Vector3>();
        //res.Add(transform.position + transform.up * 0.1f);
        bool hasTurnAround = false;
        HashSet<Vector3> existedPositions = new HashSet<Vector3>();
        for (int i = 0; i < visuallyNextPositions.Count; i++)
        {
            if (existedPositions.Contains(visuallyNextPositions[i].position))
            {
                hasTurnAround = true;
                break;
            }
            existedPositions.Add(visuallyNextPositions[i].position);
        }
        //bool onRight = false;
        HashSet<Vector3> leftRightPositions = new HashSet<Vector3>();
        int lineIndex = 0;
        for (int i = 0; i < visuallyNextPositions.Count; i++)
        {
            var p = visuallyNextPositions[i];

            if (hasTurnAround)
            {

                if (leftRightPositions.Contains(p.position))
                {
                    lineIndex = 1 - lineIndex;
                    leftRightPositions.Clear();
                }
                leftRightPositions.Add(p.position);
                var position = p.position;


                //if (i == 0 && visuallyNextPositions.Count >= 1)
                //{
                //    var originDistance = (visuallyNextPositions[0].position - visuallyNextPositions[1].position).magnitude;
                //    var moveProgress = (transform.position - visuallyNextPositions[1].position).magnitude / originDistance;
                //    moveProgress = Mathf.Clamp(moveProgress, 0, 1);
                //    position = visuallyNextPositions[1].position + (visuallyNextPositions[0].position - visuallyNextPositions[1].position) * moveProgress;

                //}

                var right = p.right;
                if (i != visuallyNextPositions.Count - 1 && visuallyNextPositions[i + 1].right != right && visuallyNextPositions[i + 1].right != -right)
                {
                    right += visuallyNextPositions[i + 1].right;
                }
                position -= right * 0.08f;
                //if (lineIndex == 0)
                //{
                //    position -= right * 0.1f;
                //}
                //else
                //{

                //    position += right * 0.1f;
                //}
                res.Add(position);
            }
            else
            {

                res.Add(p.position);
            }
        }


        return res;
    }

    public List<Vector3> nextPoints()
    {
        return improveVisualPoints(visuallyNextPositions);
    }
}

