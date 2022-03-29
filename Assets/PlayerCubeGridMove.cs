using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCubeGridMove : MonoBehaviour
{
    public float moveSpeed = 3;
    public LayerMask walkableLayer;
    bool startedMoving = false;
    public Transform frontDetection;
    float rotateCoolDown = 0.5f;
    float rotateCoolDownTimer = 100;
    public void startPosition(Vector3 position)
    {
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
            rotateCoolDownTimer += Time.deltaTime;
            if (rotateCoolDownTimer < rotateCoolDown)
            {
                var rotation = transform.rotation;
                rotation *= Quaternion.Euler(90, 0, 0); // this adds a 90 degrees Y rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (1/rotateCoolDown));
                return;
            }
            //force rotation
            transform.rotation.SetEulerAngles(Utils.roundTo90(transform.rotation.eulerAngles));
            //if(rotate)
            bool hit = Physics.Raycast(frontDetection.position, -transform.up, 1, walkableLayer);
            bool hitAny = Physics.Raycast(frontDetection.position, -transform.up, 1);
            if (!hitAny)
            {
                //transform.Rotate(90, 0, 0);

                rotateCoolDownTimer = 0;
            }
            else
            {

                if (hit)
                {

                    transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
                }
            }
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
