using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    static bool shouldLog = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public void log(string text)
    {
        if (shouldLog)
        {
            Debug.Log(text);
        }
    }
}
