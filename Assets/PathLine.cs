using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLine : MonoBehaviour
{

    BGCurve pathLine;
    PlayerCubeGridMove playerMove;
    void updatePathLine()
    {
        int i = 0;
        foreach(var p in playerMove.nextPoints())
        {

            BGCurvePoint BGp = new BGCurvePoint(pathLine, p, true);
            if (i < pathLine.Points.Length)
            {

                pathLine.Points[i] = (BGp);
            }
            else
            {
                pathLine.AddPoint(BGp);
            }
            i++;
        }
        while (pathLine.Points.Length > i)
        {
            pathLine.Delete(i);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pathLine = GetComponent<BGCurve>();
        playerMove = GetComponentInParent<PlayerCubeGridMove>();
        pathLine.Clear();
        //pathLine.Clear();
        updatePathLine();
    }

    // Update is called once per frame
    void Update()
    {
        updatePathLine();
        //updatePathLine();
    }
}
