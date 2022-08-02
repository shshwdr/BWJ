using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventPool.Trigger("startLevel");

        //before, when restart level, it would not start level

        if (!StartMenu.Instance.hasStartedMainLevel)
        {
            gameObject.SetActive(false);
        }
        else
       {
            StageLevelManager.Instance.startLevel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
