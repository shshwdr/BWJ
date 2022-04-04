using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsMenu : MonoBehaviour
{
    PlayerCubeGridMove player;
    public Button instruction1;
    public Button instruction2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerCubeGridMove>();

        instruction1.onClick.AddListener(delegate { turnAround(); });
        if (StageLevelManager.Instance.hasEverCollected)
        {

            instruction1.gameObject.SetActive(true);
        }
        else
        {
            EventPool.OptIn("firstCollect", showInstruction1);
            instruction1.gameObject.SetActive(false);
        }

        if(StageLevelManager.Instance.starCountInTotal > 2)
        {

            instruction2.gameObject.SetActive(true);
        }
        else
        {

            //EventPool.OptIn("firstCollect", showInstruction2);
            instruction2.gameObject.SetActive(false);
        }

        instruction2.onClick.AddListener(delegate { ignoreSign(); });
    }

    public void showInstruction1()
    {
        instruction1.gameObject.SetActive(true);
    }
    public void showInstruction2()
    {
        instruction2.gameObject.SetActive(true);
    }

    public void turnAround()
    {
        player.turnAround();
    }
    public void ignoreSign()
    {
        player.ignoreSign();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
