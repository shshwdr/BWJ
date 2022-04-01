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

    [MenuItem("LevelCreator/Test Level Step &T")]
    static void testLevel()
    {
        PlayerCubeGridMove player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        Transform startTrans = GameObject.Find("StartPoint").transform;
        player.startPosition(startTrans.position, startTrans.rotation);
        waitMove(player, 500);


    }

    [MenuItem("LevelCreator/Test Level Fast &Y")]
    static void testLevel2()
    {
        PlayerCubeGridMove player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        Transform startTrans = GameObject.Find("StartPoint").transform;
        player.startPosition(startTrans.position, startTrans.rotation);
        waitMove(player, 0);



    }

    async static void waitMove(PlayerCubeGridMove player,int time)
    {
        int x = 0;
        //todo: detect loop and stop

        //no instruction
        while (x < 50)
        {
            x++;


            //paint the object
            LayerMask mask = LayerMask.GetMask("validationPad");
            RaycastHit hit;
            bool hitPad = Physics.Raycast(player.transform.position + player.transform.up * 0.5f, -player.transform.up, out hit, 1, mask);
            if (hitPad)
            {
                hit.collider.GetComponent<LevelValidationPadView>().setValidationInfo(0, "");
            }

            if (player.gameEnd())
            {
                break;
            }
            await Task.Delay(time);
            player.moveNextMove();

        }
    }
}
