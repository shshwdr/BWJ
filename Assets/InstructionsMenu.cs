using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsMenu : MonoBehaviour
{
    PlayerCubeGridMove player;
    public Button instruction1;//turn back
    public Button instruction2;//ignore equipment
    public Button instruction3;//can swim

    public int inst2RequireCount = 3;
    public int inst3RequireCount = 6;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerCubeGridMove>();

        instruction1.onClick.AddListener(delegate { turnAround(); });
        instruction2.onClick.AddListener(delegate { ignoreSign(); });
        instruction2.onClick.AddListener(delegate { swim(); });
        if (StageLevelManager.Instance.hasEverCollected)
        {

            instruction1.gameObject.SetActive(true);
        }
        else
        {
            EventPool.OptIn("firstCollect", showInstruction1);
            instruction1.gameObject.SetActive(false);
        }

        if (StageLevelManager.Instance.starCountInTotal > inst2RequireCount)
        {

            instruction2.gameObject.SetActive(true);
        }
        else
        {

            EventPool.OptIn("updateTotalCollected", showInstruction2);
            instruction2.gameObject.SetActive(false);
        }

        if (StageLevelManager.Instance.starCountInTotal > inst3RequireCount)
        {

            instruction3.gameObject.SetActive(true);
        }
        else
        {

            EventPool.OptIn("updateTotalCollected", showInstruction3);
            instruction3.gameObject.SetActive(false);
        }

    }

    public void showInstruction1()
    {
        instruction1.gameObject.SetActive(true);
    }
    public void showInstruction2()
    {
        if (StageLevelManager.Instance.totalCollected >= inst2RequireCount)
        {
            instruction2.gameObject.SetActive(true);
        }
    }
    public void showInstruction3()
    {
        if (StageLevelManager.Instance.totalCollected >= inst3RequireCount)
        {
            instruction3.gameObject.SetActive(true);
        }
    }

    public void turnAround()
    {
        player.turnAround();
    }
    public void ignoreSign()
    {
        player.ignoreSign();
    }

    public void swim()
    {
        player.swim();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
