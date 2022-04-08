using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StageLevelManager.Instance.startLevel();
        EventPool.Trigger("startLevel");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
