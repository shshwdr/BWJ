using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void onclicked()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/just button");
    }
    public void hover()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/hover");
    }
}
