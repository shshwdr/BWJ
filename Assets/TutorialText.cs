using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour
{
    public string name;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>().text = TextUtils.getText(name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
