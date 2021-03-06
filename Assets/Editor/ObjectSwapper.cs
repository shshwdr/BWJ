using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ObejctSwapper : MonoBehaviour
{

    public GameObject Prefab;

    public GameObject[] ObjectsToExchange;

    public bool SwapNow;

    static void move(Vector3 dir)
    {

        var selects = Selection.gameObjects;
        List<GameObject> sele = new List<GameObject>();
        foreach (var select in selects)
        {
            select.transform.position = select.transform.position + selects[0].transform.rotation * dir;


            sele.Add(select);
            //Selection.activeGameObject = select;
            EditorUtility.SetDirty(select);

        }
        Selection.objects = sele.ToArray();
    }

    
    static void duplicate(Vector3 dir)
    {
        var selects = Selection.gameObjects;
        List<GameObject> sele = new List<GameObject>();
        foreach (var select in selects)
        {
            GameObject newObject = GameObject.Instantiate(select, select.transform.parent);
            newObject.transform.position = newObject.transform.position + selects[0].transform.rotation * dir;

            if(dir == Vector3.up)
            {
                newObject.name = "UP " + newObject.name;
            }
            if (dir == Vector3.down)
            {
                newObject.name = "DOWN " + newObject.name;
            }
            sele.Add(newObject);
            //Selection.activeGameObject = newObject;
            EditorUtility.SetDirty(newObject);

        }
        Selection.objects = sele.ToArray();
    }
    static void Swap(string item)
    {
        var selects = Selection.gameObjects;
        foreach (var select in selects)
        {
            if(selects.Length == 1 && select.name == item)
            {
                TurnRight();
            }


            var pref = Resources.Load("LevelItem/"+item);
            if (pref)
            {
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(pref);
                newObject.transform.parent = select.transform.parent;
                newObject.transform.position = select.transform.position;
                newObject.transform.rotation = select.transform.rotation;
                newObject.transform.localScale = select.transform.localScale;
                DestroyImmediate(select); 
                Selection.activeGameObject = newObject;
                EditorUtility.SetDirty(newObject);
            }
        }
    }

    static void SwapAll(string item)
    {
        var selects = Selection.gameObjects;
        foreach (var s in selects)
        {
            var parent = s.transform.parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                var select = parent.GetChild(0).gameObject;

                var pref = Resources.Load("LevelItem/" + item);
                if (pref)
                {
                    GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(pref);
                    newObject.transform.parent = select.transform.parent;
                    newObject.transform.position = select.transform.position;
                    newObject.transform.rotation = select.transform.rotation;
                    newObject.transform.localScale = select.transform.localScale;
                    DestroyImmediate(select);
                    Selection.activeGameObject = newObject;
                    EditorUtility.SetDirty(newObject);
                }
            }
        }
    }
    static void Turn(int degree)
    {
        var selects = Selection.gameObjects;
        foreach (var select in selects)
        {
            select.transform.rotation *= Quaternion.Euler(Vector3.up * degree);
            EditorUtility.SetDirty(select);
        }
    }

    static void Add(string item)
    {
        var selects = Selection.gameObjects;
        foreach (var select in selects)
        {
            var pref = Resources.Load("LevelItem/" + item);
            if (pref)
            {
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(pref);
                newObject.transform.parent = select.transform;
                newObject.transform.position = select.transform.position;
                newObject.transform.rotation = select.transform.rotation;
                EditorUtility.SetDirty(select);
                // newObject.transform.localScale = select.transform.localScale;
                //DestroyImmediate(select);
            }
        }
    }

    static void Remove()
    {
        //var selects = Selection.gameObjects;
        //foreach (var select in selects)
        //{
        //    foreach (Transform child in select.transform)
        //    {
        //        GameObject.DestroyImmediate(child.gameObject);
        //    }
        //}
    }



    [MenuItem("LevelCreator/DuplicateUp &E")]
    static void DuplicateAndMoveUp()
    {
        duplicate(Vector3.up);
    }
    [MenuItem("LevelCreator/DuplicateDown &Q")]
    static void DuplicateAndMoveDown()
    {
        duplicate(Vector3.down);
    }
    [MenuItem("LevelCreator/DuplicateRight &D")]
    static void DuplicateAndMoveRight()
    {
        duplicate(Vector3.right);
    }
    [MenuItem("LevelCreator/DuplicateLeft &A")]
    static void DuplicateAndMoveLeft()
    {
        duplicate(Vector3.left);
    }
    [MenuItem("LevelCreator/DuplicateFront &W")]
    static void DuplicateAndMoveFront()
    {
        duplicate(Vector3.forward);
    }
    [MenuItem("LevelCreator/DuplicateBack &S")]
    static void DuplicateAndMoveBack()
    {
        duplicate(Vector3.back);
    }

    [MenuItem("LevelCreator/moveUp #E")]
    static void MoveUp()
    {
        move(Vector3.up);
    }
    [MenuItem("LevelCreator/moveDown #Q")]
    static void MoveDown()
    {
        move(Vector3.down);
    }
    [MenuItem("LevelCreator/moveRight #D")]
    static void MoveRight()
    {
        move(Vector3.right);
    }
    [MenuItem("LevelCreator/moveLeft #A")]
    static void MoveLeft()
    {
        move(Vector3.left);
    }
    [MenuItem("LevelCreator/moveFront #W")]
    static void MoveFront()
    {
        move(Vector3.forward);
    }
    [MenuItem("LevelCreator/moveBack #S")]
    static void MoveBack()
    {
        move(Vector3.back);
    }

    [MenuItem("LevelCreator/ForestGrass &9")]
    static void ForestGrass()
    {
        Swap("ForestGrass");
    }

    [MenuItem("LevelCreator/ForestAllGrass &7")]
    static void ForestAllGrass()
    {
        SwapAll("ForestGrass");
    }
    [MenuItem("LevelCreator/ForestCrossingFour &4")]
    static void ForestCrossingFour()
    {
        Swap("ForestCrossingFour");
    }
    [MenuItem("LevelCreator/waterCrossingFour &#4")]
    static void waterCrossingFour()
    {
        Swap("waterCrossingFour");
    }
    [MenuItem("LevelCreator/ForestCrossingThree &3")]
    static void ForestCrossingThree()
    {
        Swap("ForestCrossingThree");
    }
    [MenuItem("LevelCreator/waterCrossingThree &#3")]
    static void waterCrossingThree()
    {
        Swap("waterCrossingThree");
    }
    [MenuItem("LevelCreator/ForestCurve &2")]
    static void ForestCurve()
    {
        Swap("ForestCurve");
    }
    [MenuItem("LevelCreator/waterCurve &#2")]
    static void waterCurve()
    {
        Swap("waterCurve");
    }
    [MenuItem("LevelCreator/ForestEnd &0")]
    static void ForestEnd()
    {
        Swap("ForestEnd");
    }
    [MenuItem("LevelCreator/waterEnd &#0")]
    static void waterEnd()
    {
        Swap("waterEnd");
    }
    [MenuItem("LevelCreator/ForestStraight &1")]
    static void ForestStraight()
    {
        Swap("ForestStraight");
    }
    [MenuItem("LevelCreator/waterStraight &#1")]
    static void waterStraight()
    {
        Swap("waterStraight");
    }


    [MenuItem("LevelCreator/AddHuman &C")]
    static void AddHuman()
    {
        Add("Human");
    }


    [MenuItem("LevelCreator/AddTutorial &N")]
    static void AddTutorial()
    {
        Add("TutorialGiver");
    }
    [MenuItem("LevelCreator/AddLever &L")]
    static void AddLever()
    {
        Add("Lever");
    }
    [MenuItem("LevelCreator/AddLadder &#L")]
    static void AddLadder()
    {
        Add("Ladder");
    }

    [MenuItem("LevelCreator/AddTarget &T")]
    static void AddTarget()
    {
        Add("Target");
    }
    [MenuItem("LevelCreator/AddStart &M")]
    static void AddStart()
    {
        Add("Start");
    }
    [MenuItem("LevelCreator/AddTurnRight &J")]
    static void AddTurnRight()
    {
        Add("TurnRight");
    }
    [MenuItem("LevelCreator/RemoveAll &H")]
    static void RemoveAll()
    {
        Remove();
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