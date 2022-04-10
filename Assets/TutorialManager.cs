using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public List<string> unlockedTutorialList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        unlockTutorial("dragZoom");

    }
    public void unlockTutorial(string str)
    {
        if (!unlockedTutorialList.Contains(str))
        {
            unlockedTutorialList.Add(str);
            Pool.EventPool.Trigger("updateTutorial");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
