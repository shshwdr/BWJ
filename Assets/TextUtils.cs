using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUtils : MonoBehaviour
{
    static Dictionary<string, string> textMap = new Dictionary<string, string>()
    {
        { "dragZoom","Drag to look around\nScroll to zoom\n" },
        { "useFirstSkill","Click Turn Back To Use Skill" },
        { "swimTutorial","Use swim to.. swim" },
        {"swimTurnOn","Use swim again when you need" },
        {"dragInGame","You can look around by clicking Free Camera after start." },
        {"moveLogic","Move Priority: Forward Right Left" },
        {"pullLeverTutorial","Pull the Lever" },
        {"leverFirst","Pull lever would rotate the road" }
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
