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
