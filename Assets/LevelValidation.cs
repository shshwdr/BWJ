using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        waitMove(player, 500);


    }



    [MenuItem("LevelCreator/Test Level Fast &Y", false, 200)]
    static void testLevel2()
    {
        PlayerCubeGridMove player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        prepareValidation();

        waitMove(player, 0);



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

    async static void waitMove(PlayerCubeGridMove player,int time)
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
        for(int usage  = 0; usage < maxUsage; usage++)
        {
            int currentUse = 0;

            //EditorCoroutineUtility.StartCoroutine(CountEditorUpdates(), this);
            //dfs(player,1,10,500);

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
        }
    }

    async static void dfs(PlayerCubeGridMove player, int usageLeft, int stepLeft,int time)
    {
        if (stepLeft < 0)
        {
            return;
        }
        stepLeft--;
        Vector3 originalPositon = player.transform.position;
        Quaternion originalRotation = player.transform.rotation;
        for(int i = 0; i < 2; i++)
        {
            bool isused = false;
            if(i == 1)
            {

                if (usageLeft > 0)
                {
                    //do next with action
                    player.turnAround();
                    usageLeft--;
                    isused = true;

                }
                else
                {
                    continue;
                }
            }
            await Task.Delay(time);
            player.moveNextMove();
            dfs(player, usageLeft, stepLeft, time);
            if (isused)
            {
                usageLeft++;
            }
            player.transform.position = originalPositon;
            player.transform.rotation = originalRotation;
            player.updateOtherData();
        }
    }
}
