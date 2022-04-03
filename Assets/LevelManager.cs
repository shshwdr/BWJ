using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public bool isLevelGameStarted = false;
    protected override void Awake()
    {
        base.Awake();
        //set player position
        Transform startTrans = GameObject.Find("StartPoint").transform;
        GameObject.FindObjectOfType<PlayerCubeGridMove>().startPosition(startTrans.position, startTrans.rotation);
    }

    public void reset()
    {
        isLevelGameStarted = false;
    }


    public void startGame()
    {
        isLevelGameStarted = true;
        EventPool.Trigger("StartGame");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startGame();
        }
    }
}
