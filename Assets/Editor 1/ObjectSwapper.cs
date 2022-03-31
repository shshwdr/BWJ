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
    [MenuItem("LevelCreator/ForestCrossingFour")]
    static void ForestCrossingFour()
    {
        Swap("ForestCrossingFour");
    }
    [MenuItem("LevelCreator/ForestCrossingThree")]
    static void ForestCrossingThree()
    {
        Swap("ForestCrossingThree");
    }
    [MenuItem("LevelCreator/ForestCurve")]
    static void ForestCurve()
    {
        Swap("ForestCurve");
    }
    [MenuItem("LevelCreator/ForestEnd")]
    static void ForestEnd()
    {
        Swap("ForestEnd");
    }
    [MenuItem("LevelCreator/ForestStraight")]
    static void ForestStraight()
    {
        Swap("ForestStraight");
    }




    [MenuItem("LevelCreator/TurnRight", false,100)]
    static void TurnRight()
    {
        Turn(90);
    }

    [MenuItem("LevelCreator/TurnLeft", false, 100)]
    static void TurnLeft()
    {
        Turn(270);
    }

    [MenuItem("LevelCreator/Flip", false, 100)]
    static void Flip()
    {
        Turn(180);
    }

    void Update()
    {
        if (SwapNow)
        {
            for (int i = 0; i < ObjectsToExchange.Length; i++)
            {

                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(Prefab);
                newObject.transform.parent = ObjectsToExchange[i].transform.parent;
                newObject.transform.position = ObjectsToExchange[i].transform.position;
                newObject.transform.rotation = ObjectsToExchange[i].transform.rotation;
                newObject.transform.localScale = ObjectsToExchange[i].transform.localScale;
            }
            SwapNow = false;
        }
    }
}