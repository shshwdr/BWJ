#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class LevelValidation : MonoBehaviour
{

    Dictionary<ValidationStep, bool> visitedValidationSteps;
    List<bool> collectedItem;
    List<int> triggeredTime;
    Dictionary<GameObject, int> collecItemToId;

    [MenuItem("LevelCreator/Test Level Hide &U", false, 200)]
    static void hide()
    {

        foreach (var pad in GameObject.FindObjectsOfType<LevelValidationPadView>(true))
        {
            pad.hide();
        }
    }
    static void prepareValidation(LevelValidation validation)
    {
        //

        PlayerCubeGridMove player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        Transform startTrans = GameObject.Find("StartPoint").transform;
        player.startPosition(startTrans.position, startTrans.rotation);


        foreach (var pad in GameObject.FindObjectsOfType<LevelValidationPadView>(true))
        {
            pad.clear();
        }


        validation.collecItemToId = new Dictionary<GameObject, int>();
        var allItems = GameObject.FindGameObjectsWithTag("human");
        for (int i = 0; i < allItems.Length; i++)
        {
            validation.collecItemToId[allItems[i]] = i;
        }
            var collectableCount = allItems.Length;
        validation.collectedItem = new List<bool>();
        for (int i = 0;i< collectableCount; i++)
        {
            validation.collectedItem.Add(false);
        }
    }
    bool collectedAll()
    {
        foreach (var v in collectedItem)
        {
            if (!v)
            {
                return false;
            }
        }
        return true;
    }
    static void updateValidationPad(LevelValidationPadView pad)
    {
        pad.setValidationInfo();
    }

    [MenuItem("LevelCreator/Test Level Step &T", false, 200)]
    static void testLevel()
    {
        PlayerCubeGridMove player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        createInstance(player, 3f);


    }



    [MenuItem("LevelCreator/Test Level Fast &Y", false, 200)]
    static void testLevel2()
    {
        PlayerCubeGridMove player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        createInstance(player, 0);



    }


    [MenuItem("LevelCreator/Test Level Manual &Y", false, 200)]
    static void testLevelManual()
    {
        PlayerCubeGridMove player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        createInstance(player, 0,KeyCode.M);



    }

    void updateCollectedData(PlayerCubeGridMove player)
    {
        RaycastHit hitedCollectable = new RaycastHit();
        bool hitCollectable = Physics.Raycast(player.transform.position + player.transform.up * 0.5f, -player.transform.up, out hitedCollectable, 1, player.collectableLayer);
        if (hitCollectable)
        {
            if (collecItemToId.ContainsKey(hitedCollectable.collider.gameObject))
            {
                collectedItem[collecItemToId[hitedCollectable.collider.gameObject]] = true;
            }
            else
            {
                Debug.LogError("collectable not prepared");
            }
        }
    }
    

    static void createInstance(PlayerCubeGridMove player, float time,KeyCode key = KeyCode.None)
    {
        LevelValidation instance = new LevelValidation();
        instance.visitedValidationSteps = new Dictionary<ValidationStep, bool>();
        prepareValidation(instance);
        EditorCoroutineUtility.StartCoroutine(instance.waitMove(player, time,key), instance);
    }


    struct ValidationStep : System.IEquatable<ValidationStep>
    {
        public Vector3 position;
        public Quaternion rotation;
        public List<int> triggerState;
        public List<bool> collected;


        public List<ValidationStep> previousSteps;
        public List<VisuallyPosition> previousPositions;

        public ValidationStep(Vector3 position, Quaternion rotation, List<int> triggerState, List<bool> collected)
        {
            this.position = position;
            this.rotation = rotation;
            this.triggerState = new List<int>(triggerState);
            this.collected = new List<bool>( collected);
            previousSteps = new List<ValidationStep>();
            previousPositions = new List<VisuallyPosition>();

        }

        public ValidationStep(ValidationStep other)
        {
            this.position = other.position;
            this.rotation = other.rotation;
            this.triggerState = new List<int>(other.triggerState);

            this.collected = new List<bool>(other.collected);
            previousSteps = new List<ValidationStep>(other.previousSteps);
            previousPositions = new List<VisuallyPosition>(other.previousPositions);
        }


        public bool Equals(ValidationStep other)
        {
            return position == other.position
                && (rotation.eulerAngles == other.rotation.eulerAngles || rotation.Equals(other.rotation))
               //&&
               //Utils.ListEqual( triggerState, other.triggerState) &&
               && Utils.ListEqual(collected, other.collected);
        }
        public override int GetHashCode()
        {
            var hash = position.GetHashCode();
            hash = hash * 31 + rotation.eulerAngles. GetHashCode();

            hash = hash * 31;
            foreach (var c in collected)
            {
                hash = hash + (c?1:0);
                hash = hash * 2;
            }
            //hash = hash * 10 + triggerState.GetHashCode();

            return (int)hash;
        }

        //public override int GetHashCode()
        //{
        //    var hash = new HashCode();
        //}
    }



    IEnumerator waitMove(PlayerCubeGridMove player, float time,KeyCode key)
    {

        int x = 0;
        yield return EditorCoroutineUtility.StartCoroutine(bfs(player, new ValidationStep(player.transform.position, player.transform.rotation, new List<int>(), collectedItem), 200, time,key), this);
        // yield return EditorCoroutineUtility.StartCoroutine(dfs(player, new ValidationStep(player.transform.position, player.transform.rotation, new List<int>(), 0), 3, time), this);
    }
    //IEnumerator WaitForKeyDown(KeyCode keyCode)
    //{
    //    while (true)
    //    {
    //        Event e = Event.current;
    //        switch (e.type)
    //        {
    //            case EventType.KeyDown:
    //                yield break;
    //                break;
    //        }
    //        yield return new WaitForFixedUpdate();
    //    }
    //}

    void updatePlayerFromStep(PlayerCubeGridMove player, ValidationStep currentStep)
    {


        player.transform.position = currentStep.position;
        player.transform.rotation = currentStep.rotation;
        player.updateOtherData();
        collectedItem = new List<bool>( currentStep.collected);
        //StageLevelManager.Instance.setCollectable(currentStep.collected);
        StageLevelManager.Instance.isLevelFinishedValidation = false;
    }

    void updatePlayerToStep(PlayerCubeGridMove player,ref  ValidationStep newValidationStep)
    {
        updateCollectedData(player);
        newValidationStep.position = player.moveState.targetTransform.position;
        newValidationStep.rotation = player.moveState.targetTransform.rotation;
        newValidationStep.collected = new List<bool>(collectedItem);
        //newValidationStep.collected = StageLevelManager.Instance.currentCollected;

    }

    IEnumerator bfs(PlayerCubeGridMove player, ValidationStep validationStep, int stepLeft, float time,KeyCode key = KeyCode.None)
    {

        Queue<ValidationStep> steps = new Queue<ValidationStep>();
        steps.Enqueue(validationStep);

        while (steps.Count > 0 && stepLeft>0)
        {
            var currentStep = steps.Dequeue();

            if (visitedValidationSteps.ContainsKey(currentStep))
            {
                continue;
            }
            visitedValidationSteps[currentStep] = true;
            //Debug.Log("log current step " + currentStep);

            updatePlayerFromStep(player, currentStep);
            LevelValidationPadView playerStandingOnPad = player.getPadStandingOn();
            if (playerStandingOnPad)
            {
                updateValidationPad(playerStandingOnPad);
            }


            if (player.gameEnd())
            {
                Debug.Log("arrive end with collected " + currentStep.collected+" left step "+stepLeft);
                //if collected enough
                if (collectedAll())
                {
                    //show path..?
                    Debug.Log("finished path");
                    List<Vector3> res = new List<Vector3>();
                    //foreach (var p in currentStep.previousPositions)
                    //{
                    //    Debug.Log(p.position);
                    //    res.Add(p.position);
                    //}
                    res = PlayerCubeGridMove.improveVisualPoints(currentStep.previousPositions);
                    GameObject.FindObjectOfType<PathLine>().showLine(res);
                    yield break;
                }
            }

            
                yield return new EditorWaitForSeconds(time);




            stepLeft--;
            for (int i = 0; i < 2; i++)
            {
                bool isused = false;
                if (i == 1)
                {
                    player.turnAround();
                    isused = true;
                }

                updatePlayerFromStep(player, currentStep);

                player.moveNextMove();

                var newValidationStep = new ValidationStep(currentStep);
                updatePlayerToStep(player, ref newValidationStep);
                newValidationStep.previousSteps.Add(currentStep);
                foreach(var v in player.visuallyNextPositions)
                {

                    newValidationStep.previousPositions.Add(v);
                }

                if (visitedValidationSteps.ContainsKey(newValidationStep))
                {
                    continue;
                }

                steps.Enqueue(newValidationStep);
            }


        }
    }
    
    //IEnumerator dfs(PlayerCubeGridMove player, ValidationStep validationStep, int stepLeft, float time)
    //{
    //    if (stepLeft < 0)
    //    {
    //        yield break;
    //    }

    //    if (visitedValidationSteps.ContainsKey(validationStep))
    //    {
    //        yield break;
    //    }
    //    visitedValidationSteps[validationStep] = true;

    //    stepLeft--;
    //    //Vector3 originalPositon = player.transform.position;
    //    //Quaternion originalRotation = player.transform.rotation;
    //    for (int i = 0; i < 2; i++)
    //    {
    //        bool isused = false;
    //        if (i == 1)
    //        {
    //            player.turnAround();
    //            isused = true;
    //        }
    //        Debug.Log("step left " + stepLeft);
    //        Debug.Log("i " + i);

    //        player.moveNextMove();

    //        LevelValidationPadView playerStandingOnPad = player.getPadStandingOn();
    //        if (playerStandingOnPad)
    //        {
    //            updateValidationPad(playerStandingOnPad);
    //        }

    //        var newValidationStep = new ValidationStep(validationStep);
    //        newValidationStep.position = player.transform.position;
    //        newValidationStep.rotation = player.transform.rotation;
    //        newValidationStep.collected = StageLevelManager.Instance.currentCollected;
    //        if (StageLevelManager.Instance.isLevelFinishedValidation)
    //        {
    //            Debug.Log("arrive end with collected "+ newValidationStep.collected);
    //        }

    //        if (visitedValidationSteps.ContainsKey(newValidationStep))
    //        {
    //            player.transform.position = newValidationStep.position;
    //            player.transform.rotation = newValidationStep.rotation;
    //            player.updateOtherData();
    //            StageLevelManager.Instance.setCollectable(validationStep.collected);
    //            StageLevelManager.Instance.isLevelFinishedValidation = false;
    //            yield break;
    //        }

    //        yield return new EditorWaitForSeconds(time);
    //        yield return EditorCoroutineUtility.StartCoroutine(dfs(player, newValidationStep, stepLeft, time), this);
    //        player.transform.position = newValidationStep.position;
    //        player.transform.rotation = newValidationStep.rotation;
    //        player.updateOtherData();
    //        StageLevelManager.Instance.setCollectable(validationStep.collected);
    //        StageLevelManager.Instance.isLevelFinishedValidation = false;
    //    }
    //}
}
#endif