using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUtil : MonoBehaviour
{

    public void endConversation()
    {

        Time.timeScale = 1;
        GameObject.FindObjectOfType<LevelStart>(true).gameObject.SetActive(true);
        DialogueManager.PlaySequence("Glitch(0,0)");
        DialogueManager.PlaySequence("FMODEvent(event:/glitch 2,false)"); 
        TutorialManager.Instance. unlockTutorial("dragZoom", true);
        //GameObject.Find("MC").GetComponent<Animator>().SetTrigger("getBack");
    }
    public void startConversation()
    {

        Time.timeScale = 0;
       // GetComponent<AudioSource>().Play();
        GameObject.FindObjectOfType<LevelStart>(true).gameObject.SetActive(false);
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
