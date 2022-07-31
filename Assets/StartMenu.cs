using Doozy.Engine.UI;
using Pool;
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
    public Button settingsButtons;
    public Button creditsButtons;
    public Button supportUsButtons;
    public GameObject camera;
    public UIView canvas;

    public bool hasStartedMainLevel = false;
    // Start is called before the first frame update
    void Start()
    {
        hasStartedMainLevel = false;
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
        newGameButton.onClick.AddListener(delegate
        {

            if (SaveLoadManager.hasSavedData())
            {
                GameObject.FindObjectOfType<Popup>(true).Init(TextUtils.getText("restart"), () =>
                {

                    SaveLoadManager.clearSavedData();
                    moveToMainLevel();
                }
                );

                GameObject.FindObjectOfType<Popup>(true).showView();
            }
            else
            {

                moveToMainLevel();
            }

        });

        continueButton.onClick.AddListener(delegate
        {
            StageLevelManager.Instance.setToLatestLevel();
            moveToMainLevel();
        });

        settingsButtons.onClick.AddListener(delegate
        {
            GameObject.FindObjectOfType<SettingView>(true).init(false);
            GameObject.FindObjectOfType<SettingView>(true).showView();
        }); 
        creditsButtons.onClick.AddListener(delegate
        {
            GameObject.FindObjectOfType<Popup>(true).Init("Flavedo - programmer/designer\nsourlyx - sound designer/composer\nChapstic593 - 3d artist/ modeler\nToucan - narrative",null,"Got It");

            GameObject.FindObjectOfType<Popup>(true).showView();
        }); 
        supportUsButtons.onClick.AddListener(delegate
        {

            GameObject.FindObjectOfType<Popup>(true).Init("Do you want to watch ads to support us?", ()=>{
                //play ads
        });

            GameObject.FindObjectOfType<Popup>(true).showView();
        });

        levelButton.onClick.AddListener(delegate
        {
            StageLevelManager.Instance.selectLevel();
            //moveToMainLevel();
        });
        Time.timeScale = 1;

        if (StageLevelManager.Instance.currentLevelId != 0)
        {
            testNextLevel();
        }

        //EventPool.OptIn("startlevel", startlevel);
    }

    async void testNextLevel()
    {
        StageLevelManager.Instance.startNextLevel();
        canvas.Hide();
        await Task.Delay(500);
        camera.SetActive(false);
        hasStartedMainLevel = true;
        await Task.Delay(2000);
        GameObject.FindObjectOfType<LevelStart>(true).gameObject.SetActive(true);
        StageLevelManager.Instance.startLevel();
    }

    public async void moveToMainLevel()
    {
        StageLevelManager.Instance.startNextLevel(StageLevelManager.Instance.currentLevelId != 0);
        canvas.Hide();
        await Task.Delay(500);
        hasStartedMainLevel = true;
        camera.SetActive(false);
        await Task.Delay(2000);
        GameObject.FindObjectOfType<LevelStart>(true).gameObject.SetActive(true);

        StageLevelManager.Instance.startLevel();
    }

    //void startlevel()
    //{

    //    StageLevelManager.Instance.startLevel();
    //}

    // Update is called once per frame
    void Update()
    {

    }
}
