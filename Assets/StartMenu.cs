using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : Singleton<StartMenu>
{
    public Button newGameButton;
    public Button continueButton;
    public Button levelButton;
    // Start is called before the first frame update
    void Start()
    {
        SaveLoadManager.LoadGame();
        if (SaveLoadManager.hasSavedData())
        {
            continueButton.gameObject.SetActive(true);
            levelButton.gameObject.SetActive(true);
        }
        else
        {

            continueButton.gameObject.SetActive(false);
            levelButton.gameObject.SetActive(false);
        }
        newGameButton.onClick.AddListener(delegate {
            if (SaveLoadManager.hasSavedData())
            {
                SaveLoadManager.clearSavedData();

            }
        
        });

        levelButton.onClick.AddListener(delegate { StageLevelManager.Instance.selectLevel(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
