using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.parent.GetComponentsInChildren<Teleport>().Length == 2)
        {
            foreach(var t in transform.parent.parent.GetComponentsInChildren<Teleport>())
            {
                if (t != this)
                {
                    teleportTarget = t.transform;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("teleport should appear in pair");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
