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
        GameObject.FindObjectOfType<PlayerCubeGridMove>().startPosition(GameObject.Find("StartPoint").transform.position);
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
        
    }
}
