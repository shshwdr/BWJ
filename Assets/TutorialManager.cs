using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public List<string> unlockedTutorialList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {

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
            bool hasVideo = TextUtils.videoSet.Contains(str);
            //string videoString = TextUtils.getVideo(str);
            GameObject.FindObjectOfType<TutorialPopup>(true).show(hasVideo,str);
            Pool.EventPool.Trigger("updateTutorial");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
