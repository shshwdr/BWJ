using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    protected override void Awake()
    {
        base.Awake();
        //set player position
        GameObject.FindObjectOfType<PlayerCubeGridMove>().startPosition(GameObject.Find("StartPoint").transform.position);
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
