using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUtils : MonoBehaviour
{
    public static Dictionary<string, string> textMap = new Dictionary<string, string>()
    {
        //{ "dragZoom","Drag to look around\nScroll to zoom" },
        //{ "useFirstSkill","Click Turn Back To Use Skill" },
        
        //{"dragInGame","You can look around by clicking Free Camera after start." },
        //{"moveLogic","Move Priority: Forward Right Left" },
        //{"leverFirst","You will pull lever and rotate the road when you walk by" },
        //{"learnSign","Left Sign would force Turn Left..If you can" },
        //{"signLogic","Left is based on your orientation" },
        //{"swimTutorial","Use swim to.. swim" },
        //{"swimTurnOn","Sometimes you might want to turn off swim" },
        //{"restart","Do you want to clear your previous data and restart the game?" },



        { "dragZoom","Drag to look around\nScroll to zoom\nPress Start to move" },
        { "useFirstSkill","Click 'Turn Back' to use this skill." },

        {"dragInGame","You can look around by toggling Free Camera" },
        {"moveLogic","Move priority at intersection: Forward, Right, Left. If one direction is blocked, you will try the next in order" },
        {"leverFirst","You will pull levers to rotate the road when you walk by." },
        {"learnSign","Signs will force turning in that direction... If you can" },
        {"signLogic","Turning Left is based on robot orientation" },
        {"swimTutorial","Use swim to.. swim" },
        {"swimTurnOn","Sometimes you might want to turn off swimming" },
        {"restart","Do you want to clear your previous data and restart the game?" },
    };


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
