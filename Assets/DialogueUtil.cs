using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUtil : MonoBehaviour
{

    public void endConversation()
    {
        GameObject.FindObjectOfType<LevelStart>(true).hideDialogue();
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
