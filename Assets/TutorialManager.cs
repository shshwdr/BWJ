using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public Dictionary<string, bool> tutorialUnlocked = new Dictionary<string, bool>();
    public List<string> unlockedTutorialList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        unlockTutorial("dragZoom");

    }
    public void unlockTutorial(string str)
    {
        if (!tutorialUnlocked.ContainsKey(str))
        {
            unlockedTutorialList.Add(str);
            tutorialUnlocked[str] = true;
            Pool.EventPool.Trigger("updateTutorial");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
