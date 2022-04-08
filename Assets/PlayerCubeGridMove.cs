using PixelCrushers.DialogueSystem;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
public class PlayerCubeGridMove : MonoBehaviour
{
    public float gridSize = 1;
    public float moveSpeed = 3;
    public float rotateSpeed = 10;
    public LayerMask walkableLayer;
    public LayerMask swimLayer;
    public LayerMask signLayer;
    public LayerMask collectableLayer;
    public LayerMask endLayer;
    public LayerMask leverLayer;
    bool startedMoving = false;
    public Transform frontDetection;
    public float rotateCoolDown = 0.1f;
    float rotateCoolDownTimer = 100;
    Quaternion targetRotation = Quaternion.identity;
    List<Vector3> nextPositions = new List<Vector3>();
    List<Quaternion> nextRotations = new List<Quaternion>();
    public float stopDistance = 0.001f;

    Transform targetTransform;

    public bool ignoreNextSign;
    public bool turnAroundNext;
    public bool swimNext;

    bool isSwiming;

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
       targetTransform = new GameObject().transform;
        targetTransform.position = transform.position;
        targetTransform.rotation = transform.rotation;
        targetRotation = rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("StartGame", startMove);
    }

    bool canMove(Vector3 dir, bool forceSwim = false)
    {

        LayerMask canWalkLayer = walkableLayer;
        if (swimNext || forceSwim)
        {
            canWalkLayer |= swimLayer;
        }

        targetRotation *= Quaternion.Euler(dir);
        //check if next grid is moveable
        var nextPosition = Utils.snapToGrid(targetTransform.position + targetRotation * Vector3.forward * gridSize);
        var nextPosition1 = Utils.snapToGrid(targetTransform.position + targetRotation * Vector3.forward * gridSize*0.33f);
        var nextPosition2 = Utils.snapToGrid(targetTransform.position + targetRotation * Vector3.forward * gridSize*0.66f);
        bool hitAny = Physics.Raycast(nextPosition + targetTransform.up * 0.5f, -targetTransform.up, 1);
        if (hitAny)
        {

            //if next is hitted, check if hit on ground
            RaycastHit hit;
            bool hitRoad = Physics.Raycast(nextPosition + targetTransform.up * 0.5f, -targetTransform.up, out hit,1, canWalkLayer);
            bool hitRoad1 = Physics.Raycast(nextPosition1 + targetTransform.up * 0.5f, -targetTransform.up, 1, canWalkLayer);
            bool hitRoad2 = Physics.Raycast(nextPosition2 + targetTransform.up * 0.5f, -targetTransform.up, 1, canWalkLayer);
            if (hitRoad && hitRoad1 && hitRoad2)
            {
                nextPositions.Add(nextPosition);
                nextRotations.Add(targetRotation);
                if(1<<hit.collider.gameObject.layer == swimLayer)
                {

                    if (startedMoving)
                    {
                        animator.SetBool("swim", true);
                        isSwiming = true;
                    }
                    return true;
                }
                else
                {
                    if (startedMoving)
                    {
                        animator.SetBool("swim", false);
                        animator.SetBool("walk", true);
                        if (isSwiming)
                        {
                            swimNext = false;
                            isSwiming = false;


                            EventPool.Trigger<int>("turnedInstructionOff", 3);
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
                targetRotation *= Quaternion.Euler(-dir);
                return false;
            }
        }
        else
        {
            //check rotate position

            RaycastHit hit;
            nextPosition = Utils.snapToGrid(targetTransform.position + targetRotation * Vector3.forward * gridSize * 0.5f);
            nextPosition1 = Utils.snapToGrid(targetTransform.position + targetRotation * Vector3.forward * gridSize * 0.33f);
            var nnPosition = Utils.snapToGrid(nextPosition - targetTransform.up * gridSize * 0.5f);
            nextPosition2 = Utils.snapToGrid(nnPosition + targetTransform.up * gridSize * 0.33f);
            //if next is hitted, check if hit on ground
            bool hitRoad = Physics.Raycast(nnPosition + targetRotation * Vector3.forward * 0.5f, -(targetRotation * Vector3.forward),out hit, 1, canWalkLayer);
            bool hitRoad1 = Physics.Raycast(nextPosition1 + targetRotation * Vector3.up * 0.5f, -(targetRotation * Vector3.up), 1, canWalkLayer);
            bool hitRoad2 = Physics.Raycast(nextPosition2 + targetRotation * Vector3.forward * 0.5f, -(targetRotation * Vector3.forward), 1, canWalkLayer);
            if (hitRoad && hitRoad1 && hitRoad2)
            {
                nextPositions.Add(nextPosition);
                nextRotations.Add(targetRotation);
                nextPositions.Add(nnPosition);
                targetRotation *= Quaternion.Euler(Vector3.right * 90);
                nextRotations.Add(targetRotation);
                if (1<<hit.collider.gameObject.layer == swimLayer)
                {

                    if (startedMoving)
                    {
                        animator.SetBool("swim", true);
                        isSwiming = true;
                    }
                }
                else
                {
                    if (startedMoving)
                    {

                        animator.SetBool("swim", false);
                        animator.SetBool("walk", true); 
                        if (isSwiming)
                        {
                            swimNext = false;
                            isSwiming = false;


                            EventPool.Trigger<int>("turnedInstructionOff", 3);

                        }
                    }
                }
                return true;
            }
            else
            {
                if (startedMoving)
                {
                    animator.SetBool("walk", false);
                }
                targetRotation *= Quaternion.Euler(-dir);
                return false;
            }
        }
    }



    void moveBack()
    {

        nextPositions.Add(targetTransform.position);
        targetRotation *= Quaternion.Euler(Vector3.up * 90);
        nextRotations.Add(targetRotation);
        nextPositions.Add(targetTransform.position);
        targetRotation *= Quaternion.Euler(Vector3.up * 90);
        nextRotations.Add(targetRotation);
    }

    public void moveNextMove()
    {
        nextPositions.Clear();
        nextRotations.Clear();
        decideNextMove();
        Assert.AreEqual(nextPositions.Count,nextRotations.Count);
        for (int i = 0; i < nextPositions.Count; i++)
        {

            transform.position = nextPositions[i];
            transform.rotation = nextRotations[i];
        }
        targetTransform.position = transform.position;
        targetTransform.rotation = transform.rotation;
    }

    public void updateOtherData()
    {

        targetTransform.position = transform.position;
        targetTransform.rotation = transform.rotation;
        targetRotation = transform.rotation;
    }
    public void decideNextMove()
    {
        //if has npc
        RaycastHit hitedCollectable = new RaycastHit();
        if (startedMoving)
        {

            bool hitCollectable = Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, out hitedCollectable, 1, collectableLayer);

            if (hitCollectable)
            {
                Destroy(hitedCollectable.collider.gameObject);
                StageLevelManager.Instance.addCollectable();
                FMODUnity.RuntimeManager.PlayOneShot("event:/collect");
            }
        }

        //if force turn around
        if (turnAroundNext)
        {
            LogManager.log("used skill to move back");
            moveBack();
            turnAroundNext = false;

            EventPool.Trigger<int>("turnedInstructionOff", 1);
            return;
        }

        //check if currently need to rotate
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

                if (canMove(Vector3.up * -90, isSwiming))
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/sign");
                    return;
                }
            }

        }


        //check if has lever
        RaycastHit levelHit;
        bool hitLever = Physics.Raycast(transform.position + transform.up * 0.5f,- transform.up, out levelHit, 1, leverLayer);
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


        if (canMove(Vector3.zero))
        {
            LogManager.log("move forward next");
        }
        else if (canMove(Vector3.up * 90))
        {

            LogManager.log("move right next");
        }
        else if (canMove(-Vector3.up * 90))
        {
            LogManager.log("move left next");
        }
        else if (canMove(Vector3.zero, isSwiming))
        {

            LogManager.log("swim forward next");
        }
        else if (canMove(Vector3.up * 90, isSwiming))
        {

            LogManager.log("swim right next");
        }
        else if (canMove(-Vector3.up * 90, isSwiming))
        {

            LogManager.log("swim left next");
        }
        else
        {
            LogManager.log("move back next");
            moveBack();
        }
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
                if (StageLevelManager.Instance.collectedAll())
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
        turnAroundNext = !turnAroundNext;
    }
    public void ignoreSign()
    {
        ignoreNextSign = !ignoreNextSign;
    }
    public void swim()
    {
        swimNext = !swimNext;
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
                    targetTransform.position = nextPositions[0];
                    nextPositions.RemoveAt(0);
                    targetTransform.rotation = nextRotations[0];
                    nextRotations.RemoveAt(0);
                    rotateCoolDownTimer = 0;
                }


            }

            

            //rotateCoolDownTimer += Time.deltaTime;
            //if (rotateCoolDownTimer < rotateCoolDown)
            //{
            //    var rotation = transform.rotation;
            //    rotation *= Quaternion.Euler(90, 0, 0); // this adds a 90 degrees Y rotation
            //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (1/rotateCoolDown));
            //    return;
            //}
            ////force rotation
            //transform.rotation.SetEulerAngles(Utils.roundTo90(transform.rotation.eulerAngles));
            ////if(rotate)
            //bool hit = Physics.Raycast(frontDetection.position, -transform.up, 1, walkableLayer);
            //bool hitAny = Physics.Raycast(frontDetection.position, -transform.up, 1);
            //if (!hitAny)
            //{
            //    //transform.Rotate(90, 0, 0);

            //    rotateCoolDownTimer = 0;
            //}
            //else
            //{

            //    if (hit)
            //    {

            //        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
            //    }
            //}
            //else
            //{
            //    transform.Rotate(90, 0, 0);
            //    //bool hitAny = Physics.Raycast(frontDetection.position, -transform.up, 1);
            //    //if(hitAny)
            //    //{

            //    //    transform.Rotate(90, 0, 0);
            //    //    rotateCoolDownTimer = 0;
            //    //}
            //    //var rotation = transform.rotation;
            //    //rotation *= Quaternion.Euler(90, 0, 0); // this adds a 90 degrees Y rotation
            //    //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.5f);
            //}
        }
    }
}

