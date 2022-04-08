using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        showDialogue();
        StageLevelManager.Instance.startLevel();

    }

    public void showDialogue()
    {
        gameObject.SetActive(false);
    }
    public void hideDialogue()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
