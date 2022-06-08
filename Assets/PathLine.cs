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
        showLine(playerMove.nextPoints());
    }
    // Start is called before the first frame update
    void Start()
    {
        pathLine = GetComponent<BGCurve>();
        playerMove = GameObject.FindObjectOfType <PlayerCubeGridMove>();
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
    public void showLine(List<Vector3> points)
    {

        var pathLine = GetComponent<LineRenderer>();
        pathLine.numPositions = points.Count;
        pathLine.SetPositions(points.ToArray());
        
    }
    public void showLine2(List<Vector3> points)
    {

        pathLine = GetComponent<BGCurve>();
        int i = 0;
        foreach (var p in points)
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
}
