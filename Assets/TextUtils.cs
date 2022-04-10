using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUtils : MonoBehaviour
{
    public static Dictionary<string, string> textMap = new Dictionary<string, string>()
    {
        { "dragZoom","Drag to look around\nScroll to zoom\n" },
        { "useFirstSkill","Click Turn Back To Use Skill" },
        {"dragInGame","You can look around by clicking Free Camera after start." },
        {"moveLogic","Move Priority: Forward Right Left" },
        {"leverFirst","You will pull lever and rotate the road when you walk by" },
        {"learnSign","Left Sign would force Turn Left..If you can" },
        {"signLogic","Left is based on your orientation" },
        //{"signLogic","Left Sign would only turn left if you can" },
        {"swimTutorial","Use swim to.. swim" },
        {"swimTurnOn","Swim will turn off automatically when you leave the water" },



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
