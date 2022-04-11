using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public List<string> unlockedTutorialList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        unlockTutorial("dragZoom",false);

    }
    public void unlockTutorial(string str, bool sound = true)
    {
        if (!unlockedTutorialList.Contains(str))
        {
            if (sound)
            {

                FMODUnity.RuntimeManager.PlayOneShot("event:/collect");
            }
            unlockedTutorialList.Add(str);

            Pool.EventPool.Trigger("updateTutorial");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
