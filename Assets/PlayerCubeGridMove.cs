using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCubeGridMove : MonoBehaviour
{
    public float gridSize = 1;
    public float moveSpeed = 3;
    public float rotateSpeed = 10;
    public LayerMask walkableLayer;
    public LayerMask signLayer;
    bool startedMoving = false;
    public Transform frontDetection;
    float rotateCoolDown = 0.5f;
    float rotateCoolDownTimer = 100;
    Quaternion targetRotation = Quaternion.identity;
    List<Vector3> nextPositions = new List<Vector3>();
    List<Quaternion> nextRotations = new List<Quaternion>();
    float stopDistance = 0.005f;

    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void startPosition(Vector3 position)
    {
        Utils.gridSize = gridSize / 4f;
        transform.position = position;

        startMoving();
    }

    void startMoving()
    {
        startedMoving = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startedMoving)
        {
            if (nextPositions.Count == 0)
            {

                //check if currently need to rotate

                bool hitSign= Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, 1, signLayer);
                if (hitSign)
                {
                    targetRotation *= Quaternion.Euler(Vector3.up * 90);
                    nextRotations.Add(targetRotation);
                }
                else
                {
                    nextRotations.Add(targetRotation);

                }

                //check if next grid is moveable
                var nextPosition = Utils.snapToGrid( transform.position + targetRotation*Vector3.forward * gridSize);

                bool hitAny = Physics.Raycast(nextPosition+transform.up*0.5f, -transform.up, 1);
                if (hitAny)
                {

                    //if next is hitted, check if hit on ground
                    bool hitRoad = Physics.Raycast(nextPosition + transform.up * 0.5f, -transform.up, 1, walkableLayer);
                    if (hitRoad)
                    {
                        nextPositions.Add(nextPosition);
                        animator.SetBool("walk", true);
                    }
                    else
                    {

                        animator.SetBool("walk", false);
                    }
                }
                else
                {
                    //check rotate position

                    nextPosition = Utils.snapToGrid(transform.position + targetRotation * Vector3.forward * gridSize * 0.5f);
                    var nnPosition = Utils.snapToGrid(nextPosition - transform.up * gridSize * 0.5f);
                    //if next is hitted, check if hit on ground
                    bool hitRoad = Physics.Raycast(nnPosition + transform.forward * 0.5f, -transform.forward, 1, walkableLayer);
                    if (hitRoad)
                    {
                        nextPositions.Add(nextPosition);
                        nextPositions.Add(nnPosition);
                        targetRotation *= Quaternion.Euler(Vector3.right * 90);
                        nextRotations.Add(targetRotation);
                        animator.SetBool("walk", true);
                    }
                    else
                    {

                        animator.SetBool("walk", false);
                    }
                }

            }
            if (nextPositions.Count > 0)
            {
                //move
                transform.Translate((nextPositions[0] - transform.position).normalized * moveSpeed * Time.deltaTime, Space.World);
                var deltaQuaternion = transform.rotation * Quaternion.Inverse(nextRotations[0]);
                //transform.Rotate(deltaQuaternion.eulerAngles*Time.deltaTime*rotateSpeed,Space.World);
                transform.rotation = Quaternion.Slerp(transform.rotation, nextRotations[0], Time.deltaTime * (1 / rotateCoolDown));
                if ((nextPositions[0] - transform.position).sqrMagnitude <= stopDistance)
                {
                    nextPositions.RemoveAt(0);
                    nextRotations.RemoveAt(0);
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
