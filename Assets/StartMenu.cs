using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : Singleton<StartMenu>
{
    public Button newGameButton;
    public Button continueButton;
    public Button levelButton;
    public GameObject camera;
    
    public bool hasStartedMainLevel = false;
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
            moveToMainLevel();

        });

        levelButton.onClick.AddListener(delegate { StageLevelManager.Instance.selectLevel(); });
    }

    async void moveToMainLevel()
    {
        StageLevelManager.Instance.startNextLevel(false);
        camera.SetActive(false);
        await Task.Delay(2000);
        GameObject.FindObjectOfType<LevelStart>(true).gameObject.SetActive(true);

        StageLevelManager.Instance.startLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
