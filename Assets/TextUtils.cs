using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUtils : MonoBehaviour
{
    public static Dictionary<string, string> textMap = new Dictionary<string, string>()
    {



        { "dragZoom","Pan to look around\nPinch to zoom\nPress Start to move" },
        { "useFirstSkill","Click U turn to turn back." },
        //{ "useFirstSkill_video","Click 'Turn Back' to use the skill and turn back." },

        {"dragInGame","You can press Reset Camera to follow player." },
        {"moveLogic","Move priority at intersection: Forward, Right, Left, Turn Back. If one direction is blocked, you will try the next in order." },
        {"leverFirst","You will pull levers to rotate the road when you walk by." },
        {"learnSign","Signs will force turning in that direction... If you can" },
        {"signLogic","Turning Left is based on robot orientation" },
        {"swimTutorial","Click Swim button to.. swim" },
        {"swimTurnOn","Sometimes you might want to turn off swimming" },
        {"restart","Do you want to clear your previous data and restart the game?" },
        {"getHint","Do you want to watch an ad and get hint? " },
        {"startHint","Do you want to play hint? " },
        {"stopHint","Do you want to stop playing hint?" },
        {"warningToRestart","The level will restart and play automatically for you." },
        {"thanks", "Thanks For supporting us!"},

    };
    public static HashSet<string> videoSet = new HashSet<string>
    {
        "useFirstSkill",
    };

    //static public string getVideo(string name)
    //{
    //    if (!textMap.ContainsKey(name+ "_video"))
    //    {
    //        return "";
    //    }
    //    return textMap[name + "_video"];
    //}

    static public string getText(string name)
    {
        if (!textMap.ContainsKey(name))
        {
            Debug.LogError("no text named " + name);
            return "";
        }
        return textMap[name];
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
