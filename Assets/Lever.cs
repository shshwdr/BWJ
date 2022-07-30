using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lever : MonoBehaviour
{
    public Transform rotateObject;
    Quaternion targetRotation;
    bool isTriggered = false;

    public LayerMask  walkableLayer;
    public void rotate()
    {
        GetComponentInChildren<Animator>().SetTrigger("pull");
        
        var target = rotateObject.transform.rotation * Quaternion.Euler(Vector3.up * 90);
        rotateObject.transform.DORotateQuaternion(target, 0.5f);
        //rotateObject.transform.rotation = Quaternion.Slerp(rotateObject.transform.rotation, target, Time.deltaTime);
        //rotateObject.transform.DO
        // targetRotation = rotateObject.transform.rotation * Quaternion.Euler(Vector3.up * 90);
    }

    public void forceRotate()
    {
        rotateObject.transform.rotation *= Quaternion.Euler(Vector3.up * 90);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!rotateObject)
        {
            Debug.LogError("rotate object is null");
            return;
        }
        //targetRotation = transform.rotation;
        // DOTween.Init();
        drawLine();

        GetComponentInChildren<MoveWithLine>().init(GetComponentInChildren<LineRenderer>());
    }

    // Update is called once per frame
    void Update()
    {

        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
    }

    void drawLine()
    {
        //find out line renderer
        LineRenderer line = GetComponentInChildren<LineRenderer>();
        List<Vector3> linePoints = new List<Vector3>();
        // find out start and end point
        var startPosition = transform.parent.position;
        var endPosition = rotateObject.transform.position;
        //if(rotateObject.tag == "ladder")
        //{
        //    endPosition = rotateObject.transform.parent.position;
        //}
        var up = transform.parent.up;
        var lineUp = up * 0.1f;

        linePoints.Add(startPosition + lineUp);

        //only two direction is available.
        var dir =  endPosition - startPosition;
        List<Vector3> dirs  = new List<Vector3>();

        Vector3 notDirAndNotUp = Vector3.zero;
        var dirx = new Vector3(Mathf.Sign(dir.x), 0, 0);
        var diry = new Vector3(0, Mathf.Sign(dir.y), 0);
        var dirz = new Vector3(0, 0, Mathf.Sign(dir.z));
        if (Mathf.Abs(dir.x) > 0.2f)
        {
            dirs.Add(dirx * 0.5f);
        }
        else
        {
            if(dirx != up && dirx  != -up)
            {
                notDirAndNotUp = dirx * 0.5f;
            }
        }
        if (Mathf.Abs(dir.y) > 0.2f)
        {
            dirs.Add(new Vector3( 0, Mathf.Sign(dir.y), 0) * 0.5f);
        }
        else
        {
            if (diry != up && diry != -up)
            {
                notDirAndNotUp = diry * 0.5f;
            }
        }
        if (Mathf.Abs(dir.z) > 0.2f)
        {
            dirs.Add(new Vector3(0, 0, Mathf.Sign(dir.z)) * 0.5f);
        }
        else
        {
            if (dirz != up && dirz != -up)
            {
                notDirAndNotUp = dirz * 0.5f;
            }
        }
        if (dirs.Count == 1)
        {
            bool hitRoad = Physics.Raycast(startPosition + notDirAndNotUp*2, -up, 1);
            if (hitRoad)
            {
                dirs.Add(notDirAndNotUp);
            }
            else
            {

                hitRoad = Physics.Raycast(startPosition - notDirAndNotUp * 2, -up, 1);
                dirs.Add(-notDirAndNotUp);
            }

        }
        if(dirs.Count != 2)
        {
            Debug.LogError("dirs is wrong for lever");
            return;
        }
        dirs.Add(dirs[0] + dirs[1]);

        // step is 0.5, move if there is no road there
        int i = 20;
        while (i > 0)
        {

            bool hasPathWithoutRoad = false;
            foreach (var d in dirs)
            {
                var newPosition = startPosition + d*0.6666667f;
                var upNewPosition = newPosition + up*0.5f;
                bool hitRoad = Physics.Raycast(upNewPosition, -up, 1, walkableLayer);
                if (!hitRoad)
                {

                    startPosition = newPosition;


                    linePoints.Add(startPosition + lineUp);
                    //if it is the first point, move start point out of the road
                    if(linePoints.Count == 2)
                    {
                        linePoints[0] = (linePoints[0] + linePoints[1]) / 2;
                    }





                    hasPathWithoutRoad = true;
                    break;
                }
                else
                {
                }
            }
            if (!hasPathWithoutRoad)
            {

                Debug.LogError("no path without road");
                break;
            }
            i--;

            // if it is the end point, move end point out of the road
            if ((startPosition - endPosition).magnitude < 0.5f)
            {
                linePoints.Add(endPosition + lineUp);
                var endIndex = linePoints.Count - 1;
                linePoints[endIndex] = (linePoints[endIndex] + linePoints[endIndex-1]) / 2;
                //linePoints.Add(endPosition + lineUp);
                break;
            }
        }
        if(i == 0)
        {
            Debug.LogError("infinite loop for lever");
            return;
        }


        //draw line
        line.positionCount = linePoints.Count;
        line.SetPositions(linePoints.ToArray());
    }
}
