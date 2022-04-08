using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneshottest : MonoBehaviour
{
    // Start is called before the first frame update
    


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/collect 2");
        }
    }
}

