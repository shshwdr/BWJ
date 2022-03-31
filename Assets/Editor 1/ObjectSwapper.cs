using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ObejctSwapper : MonoBehaviour
{

    public GameObject Prefab;

    public GameObject[] ObjectsToExchange;

    public bool SwapNow;

    static void Swap(string item)
    {
        var selects = Selection.gameObjects;
        foreach (var select in selects)
        {
            var pref = Resources.Load("LevelItem/"+item);
            if (pref)
            {
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(pref);
                newObject.transform.parent = select.transform.parent;
                newObject.transform.position = select.transform.position;
                newObject.transform.rotation = select.transform.rotation;
                newObject.transform.localScale = select.transform.localScale;
                DestroyImmediate(select);
            }
        }
    }
    static void Turn(int degree)
    {
        var selects = Selection.gameObjects;
        foreach (var select in selects)
        {
            select.transform.rotation *= Quaternion.Euler(Vector3.up * degree);
        }
    }


    [MenuItem("LevelCreator/ForestGrass")]
    static void ForestGrass()
    {
        Swap("ForestGrass");
    }
    [MenuItem("LevelCreator/ForestCrossingFour &4")]
    static void ForestCrossingFour()
    {
        Swap("ForestCrossingFour");
    }
    [MenuItem("LevelCreator/ForestCrossingThree &3")]
    static void ForestCrossingThree()
    {
        Swap("ForestCrossingThree");
    }
    [MenuItem("LevelCreator/ForestCurve &2")]
    static void ForestCurve()
    {
        Swap("ForestCurve");
    }
    [MenuItem("LevelCreator/ForestEnd &0")]
    static void ForestEnd()
    {
        Swap("ForestEnd");
    }
    [MenuItem("LevelCreator/ForestStraight &1")]
    static void ForestStraight()
    {
        Swap("ForestStraight");
    }




    [MenuItem("LevelCreator/TurnRight &P", false,100)]
    static void TurnRight()
    {
        Turn(90);
    }

    [MenuItem("LevelCreator/TurnLeft &I", false, 100)]
    static void TurnLeft()
    {
        Turn(270);
    }

    [MenuItem("LevelCreator/Flip &O", false, 100)]
    static void Flip()
    {
        Turn(180);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TurnRight();
        }
    }
}