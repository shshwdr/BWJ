using PixelCrushers.DialogueSystem;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
public class PlayerCubeGridMove : MonoBehaviour
{


    public class PlayerMoveState
    {
        //public Transform transform = new GameObject().transform;

        public bool ignoreNextSign;
        public bool turnAroundNext;
        public bool swimNext;

        public bool isSwiming;
        public bool lastIsMoveBack;

        public Transform targetTransform = new GameObject().transform;

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
    }

    public PlayerMoveState moveState;

    public float gridSize = 1;
    public float moveSpeed = 3;
    public float rotateSpeed = 10;
    public LayerMask walkableLayer;
    public LayerMask swimLayer;
    public LayerMask signLayer;
    public LayerMask collectableLayer;
    public LayerMask endLayer;
    public LayerMask leverLayer;
    public LayerMask tutorialLayer;
    bool startedMoving = false;
    public Transform frontDetection;
    public float rotateCoolDown = 0.1f;
    float rotateCoolDownTimer = 100;
    List<Vector3> nextPositions = new List<Vector3>();
    List<Vector3> visuallyNextPositions = new List<Vector3>();
    List<Quaternion> nextRotations = new List<Quaternion>();
    public float stopDistance = 0.001f;



    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        // Time.timeScale = 2;
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
    }

    bool canMove(ref PlayerMoveState state, Vector3 dir, bool isSimulating, bool forceSwim = false)
    {

        LayerMask canWalkLayer = walkableLayer;
        if (state.swimNext || forceSwim)
        {
            canWalkLayer |= swimLayer;
        }

        state.targetTransform.rotation *= Quaternion.Euler(dir);
        Quaternion targetRotation = state.targetTransform.rotation;
        //check if next grid is moveable
        var nextPosition = Utils.snapToGrid(state.targetTransform.position + targetRotation * Vector3.forward * gridSize);
        var nextPosition1 = Utils.snapToGrid(state.targetTransform.position + targetRotation * Vector3.forward * gridSize*0.33f);
        var nextPosition2 = Utils.snapToGrid(state.targetTransform.position + targetRotation * Vector3.forward * gridSize*0.66f);
        bool hitAny = Physics.Raycast(nextPosition + state.targetTransform.up * 0.5f, -state.targetTransform.up, 1);
        if (hitAny)
        {

            //if next is hitted, check if hit on ground
            RaycastHit hit;
            bool hitRoad = Physics.Raycast(nextPosition + state.targetTransform.up * 0.5f, -state.targetTransform.up, out hit,1, canWalkLayer);
            bool hitRoad1 = Physics.Raycast(nextPosition1 + state.targetTransform.up * 0.5f, -state.targetTransform.up, 1, canWalkLayer);
            bool hitRoad2 = Physics.Raycast(nextPosition2 + state.targetTransform.up * 0.5f, -state.targetTransform.up, 1, canWalkLayer);
            if (hitRoad && hitRoad1 && hitRoad2)
            {
                nextPositions.Add(nextPosition);
                visuallyNextPositions.Add(nextPosition + state.targetTransform.up * 0.1f);
                nextRotations.Add(targetRotation);
                if(1<<hit.collider.gameObject.layer == swimLayer)
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
                    return true;
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
                    return true;
                }
            }
            else
            {
                //if (startedMoving)
                //{
                //    animator.SetBool("walk", false);
                //}
                state.targetTransform.rotation *= Quaternion.Euler(-dir);
                return false;
            }
        }
        else
        {
            //check rotate position

            RaycastHit hit;
            nextPosition = Utils.snapToGrid(state.targetTransform.position + targetRotation * Vector3.forward * gridSize * 0.5f);
            nextPosition1 = Utils.snapToGrid(state.targetTransform.position + targetRotation * Vector3.forward * gridSize * 0.33f);
            var nnPosition = Utils.snapToGrid(nextPosition - state.targetTransform.up * gridSize * 0.5f);
            nextPosition2 = Utils.snapToGrid(nnPosition + state.targetTransform.up * gridSize * 0.33f);
            //if next is hitted, check if hit on ground
            bool hitRoad = Physics.Raycast(nnPosition + targetRotation * Vector3.forward * 0.5f, -(targetRotation * Vector3.forward),out hit, 1, canWalkLayer);
            bool hitRoad1 = Physics.Raycast(nextPosition1 + targetRotation * Vector3.up * 0.5f, -(targetRotation * Vector3.up), 1, canWalkLayer);
            bool hitRoad2 = Physics.Raycast(nextPosition2 + targetRotation * Vector3.forward * 0.5f, -(targetRotation * Vector3.forward), 1, canWalkLayer);
            if (hitRoad && hitRoad1 && hitRoad2)
            {
                nextPositions.Add(nextPosition);

                visuallyNextPositions.Add(nextPosition + state.targetTransform.forward * 0.1f);
                nextRotations.Add(targetRotation);
                nextPositions.Add(nnPosition);

                visuallyNextPositions.Add(nextPosition + state.targetTransform.up * 0.1f + state.targetTransform.forward * 0.1f);
                state.targetTransform.rotation *= Quaternion.Euler(Vector3.right * 90);
                nextRotations.Add(state.targetTransform.rotation);
                if (1<<hit.collider.gameObject.layer == swimLayer)
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


    void moveBack(ref PlayerMoveState state)
    {
        state.lastIsMoveBack = true;
        var targetTransform = state.targetTransform;
        nextPositions.Add(targetTransform.position);

        visuallyNextPositions.Add(targetTransform.position + targetTransform.up * 0.1f);
        targetTransform.rotation *= Quaternion.Euler(Vector3.up * 90);
        nextRotations.Add(targetTransform.rotation);
        nextPositions.Add(targetTransform.position);

        visuallyNextPositions.Add(targetTransform.position + targetTransform.forward * 0.1f);
        targetTransform.rotation *= Quaternion.Euler(Vector3.up * 90);
        nextRotations.Add(targetTransform.rotation);
    }

    public void moveNextMove()
    {
        nextPositions.Clear();
        visuallyNextPositions.Clear();
        nextRotations.Clear();
        decideNextMove();
        Assert.AreEqual(nextPositions.Count,nextRotations.Count);
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


    static void calculateNextMove(PlayerCubeGridMove playerMove, ref PlayerMoveState moveState, bool isInSimulating)
    {
        var transform = moveState.targetTransform;
        var signLayer = playerMove.signLayer;
        var leverLayer = playerMove.leverLayer;
        var ignoreNextSign = moveState.ignoreNextSign;
        var isSwiming = moveState.isSwiming;
        var lastIsMoveBack = moveState.lastIsMoveBack;

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

                if (playerMove.canMove(ref moveState, Vector3.up * -90, isInSimulating, isSwiming))
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
                    if (!isInSimulating)
                    {
                        FMODUnity.RuntimeManager.PlayOneShot("event:/lever");
                    }
                }
            }
        }

        moveState. lastIsMoveBack = false;




        if (playerMove.canMove(ref moveState, Vector3.zero,isInSimulating))
        {
            LogManager.log("move forward next");
        }
        else if (playerMove.canMove(ref moveState, Vector3.up * 90, isInSimulating))
        {

            LogManager.log("move right next");
        }
        else if (playerMove.canMove(ref moveState, -Vector3.up * 90, isInSimulating))
        {
            LogManager.log("move left next");
        }
        else if (playerMove.canMove(ref moveState, Vector3.zero, isInSimulating, isSwiming))
        {

            LogManager.log("swim forward next");
        }
        else if (playerMove.canMove(ref moveState, Vector3.up * 90, isInSimulating, isSwiming))
        {

            LogManager.log("swim right next");
        }
        else if (playerMove.canMove(ref moveState, -Vector3.up * 90, isInSimulating, isSwiming))
        {

            LogManager.log("swim left next");
        }
        else
        {
            LogManager.log("move back next");
            playerMove.moveBack(ref moveState);
        }
    }

    public void decideNextMove()
    {
        //if has collectable, collect
        if (startedMoving)
        {
            RaycastHit hitedCollectable = new RaycastHit();
            bool hitCollectable = Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, out hitedCollectable, 1, collectableLayer);

            if (hitCollectable)
            {
                Destroy(hitedCollectable.collider.gameObject);
                StageLevelManager.Instance.addCollectable();
                FMODUnity.RuntimeManager.PlayOneShot("event:/collect 2");
            }
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

        //if force turn around
        if (moveState.turnAroundNext)
        {
            LogManager.log("used skill to move back");
            moveBack(ref moveState);
            moveState.turnAroundNext = false;

            EventPool.Trigger<int>("turnedInstructionOff", 1);
            return;
        }

        calculateNextMove(this, ref moveState, false);



    }

    public bool gameEnd()
    {

        bool hitSign = Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, 1, endLayer);
        if (hitSign)
        {
            if (startedMoving)
            {

                animator.SetTrigger("victory");
            }
            if (StageLevelManager.Instance. showDialogue)
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
            return true;
        }
        return false;



    }



    public void turnAround()
    {
        moveState.turnAroundNext = !moveState.turnAroundNext;
    }
    public void ignoreSign()
    {
        moveState.ignoreNextSign = !moveState.ignoreNextSign;
    }
    public void swim()
    {
        moveState.swimNext = !moveState.swimNext;
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
                Debug.LogWarning("positions not the same count");
            }
            if (nextPositions.Count > 0)
            {
                //move
                transform.Translate((nextPositions[0] - transform.position).normalized * moveSpeed * Time.deltaTime, Space.World);
                float donePercentage = Mathf.Min(1F, Time.deltaTime / (moveSpeed));

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
    public List<Vector3> nextPoints()
    {
        var res = new List<Vector3>();
        res.Add(transform.position + transform.up * 0.1f);

        foreach (var p in visuallyNextPositions)
        {
            //this need to add based on 
            res.Add(p);
        }

        return res;
    }
}

