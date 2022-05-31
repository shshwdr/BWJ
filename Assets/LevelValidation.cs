#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class LevelValidation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Dictionary<ValidationStep, bool> visitedValidationSteps;


    [MenuItem("LevelCreator/Test Level Hide &U", false, 200)]
    static void hide()
    {

        foreach (var pad in GameObject.FindObjectsOfType<LevelValidationPadView>(true))
        {
            pad.hide();
        }
    }
    [MenuItem("LevelCreator/Test Level Step &T", false, 200)]
    static void testLevel()
    {
        PlayerCubeGridMove player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        prepareValidation();
        createInstance(player, 1);


    }



    [MenuItem("LevelCreator/Test Level Fast &Y", false, 200)]
    static void testLevel2()
    {
        PlayerCubeGridMove player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        prepareValidation();

        createInstance(player, 0);



    }

    static void createInstance(PlayerCubeGridMove player, int time)
    {
        LevelValidation instance = new LevelValidation();
        instance.visitedValidationSteps = new Dictionary<ValidationStep, bool>();
        EditorCoroutineUtility.StartCoroutine(instance.waitMove(player, time), instance);
    }

    static void prepareValidation()
    {

        PlayerCubeGridMove player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        Transform startTrans = GameObject.Find("StartPoint").transform;
        player.startPosition(startTrans.position, startTrans.rotation);

        foreach (var pad in GameObject.FindObjectsOfType<LevelValidationPadView>(true))
        {
            pad.clear();
        }
    }

    struct ValidationStep : System.IEquatable<ValidationStep>
    {
        public  Vector3 position;
        public Quaternion rotation;
        public List<int> triggerState;
        public int collected;

        public ValidationStep(Vector3 position, Quaternion rotation, List<int> triggerState, int collected)
        {
            this.position = position;
            this.rotation = rotation;
            this.triggerState = new List<int>(triggerState);
            this.collected = collected;

        }

        public ValidationStep(ValidationStep other)
        {
            this.position = other.position;
            this.rotation = other.rotation;
            this.triggerState = new List<int>(other.triggerState);
            this.collected = other.collected;
        }

        public bool Equals(ValidationStep other)
        {
            return position == other.position
                && (rotation == other.rotation || rotation.Equals(other.rotation))
            //&&
               //Utils.ListEqual( triggerState, other.triggerState) &&
               && collected == other.collected;
        }
        public override int GetHashCode()
        {
            var hash = position.GetHashCode();
            hash = hash*31 + rotation.GetHashCode();

            hash = hash * 31 + collected;
            //hash = hash * 10 + triggerState.GetHashCode();

            return (int)hash;
        }

        //public override int GetHashCode()
        //{
        //    var hash = new HashCode();
        //}
    }



        IEnumerator waitMove(PlayerCubeGridMove player,int time)
    {


        int x = 0;
        //todo: detect loop and stop

        ////no instruction
        //while (x < 50)
        //{
        //    x++;


        //    //paint the object
        //    LayerMask mask = LayerMask.GetMask("validationPad");
        //    RaycastHit hit;
        //    bool hitPad = Physics.Raycast(player.transform.position + player.transform.up * 0.5f, -player.transform.up, out hit, 1, mask);
        //    if (hitPad)
        //    {
        //        hit.collider.GetComponent<LevelValidationPadView>().setValidationInfo(-1, "");
        //    }

        //    if (player.gameEnd())
        //    {
        //        break;
        //    }
        //    await Task.Delay(time);
        //    player.moveNextMove();

        //}


        // with instruction turn around
        int maxUsage = 1;
        yield return EditorCoroutineUtility.StartCoroutine(dfs(player, new ValidationStep(player.transform.position,player.transform.rotation,new List<int>(),0), 5, time),this);
        //for (int usage  = 0; usage < maxUsage; usage++)
        //{
        //    int currentUse = 0;

        //    //EditorCoroutineUtility.StartCoroutine(CountEditorUpdates(), this);
        //    //dfs(player,1,10,500);

        //    //while (x < 50)
        //    //{
        //    //    x++;


        //    //    //paint the object
        //    //    LayerMask mask = LayerMask.GetMask("validationPad");
        //    //    RaycastHit hit;
        //    //    bool hitPad = Physics.Raycast(player.transform.position + player.transform.up * 0.5f, -player.transform.up, out hit, 1, mask);
        //    //    if (hitPad)
        //    //    {
        //    //        hit.collider.GetComponent<LevelValidationPadView>().setValidationInfo(-1, "");
        //    //    }

        //    //    if (player.gameEnd())
        //    //    {
        //    //        break;
        //    //    }
        //    //    await Task.Delay(time);
        //    //    player.moveNextMove();

        //    //}
        //}
    }

    IEnumerator dfs(PlayerCubeGridMove player, ValidationStep validationStep, int stepLeft,int time)
    {
        if (stepLeft < 0)
        {
            yield break;
        }

        if (visitedValidationSteps.ContainsKey(validationStep))
        {
            yield break;
        }
        visitedValidationSteps[validationStep] = true;

        stepLeft--;
        //Vector3 originalPositon = player.transform.position;
        //Quaternion originalRotation = player.transform.rotation;
        for(int i = 0; i < 2; i++)
        {
            bool isused = false;
            if(i == 1)
            {

                //if (usageLeft > 0)
                //{
                    //do next with action
                    player.turnAround();
                   // usageLeft--;
                    isused = true;

                //}
                //else
                //{
                //    continue;
                //}
            }
            Debug.Log("step left " + stepLeft);
            Debug.Log("i " + i);

            player.moveNextMove();
            var newValidationStep = new ValidationStep(validationStep);
            newValidationStep.position = player.transform.position;
            newValidationStep.rotation = player.transform.rotation;


            if (visitedValidationSteps.ContainsKey(newValidationStep))
            {
                player.transform.position = newValidationStep.position;
                player.transform.rotation = newValidationStep.rotation;
                player.updateOtherData();
                yield break;
            }

            yield return new EditorWaitForSeconds(time);
            yield return EditorCoroutineUtility.StartCoroutine(dfs(player, newValidationStep, stepLeft, time),this);
            player.transform.position = newValidationStep.position;
            player.transform.rotation= newValidationStep.rotation;
            player.updateOtherData();
        }
    }
}
#endif