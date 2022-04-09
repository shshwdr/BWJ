using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGiver : MonoBehaviour
{
    public string tutorialString;
    // Start is called before the first frame update
    void Start()
    {
        if(tutorialString == "")
        {
            Debug.LogError("tutorialString is empty");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
