using Doozy.Engine.UI;
using Pool;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class StartMenu : Singleton<StartMenu>, IUnityAdsShowListener
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

    public void newGame()
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
    }

    public void Continue(){
        
            StageLevelManager.Instance.setToLatestLevel();
            moveToMainLevel();
}
    public void openSettings()
    {

        GameObject.FindObjectOfType<SettingView>(true).init(false);
        GameObject.FindObjectOfType<SettingView>(true).showView();
    }
    public void openCredits()
    {

        GameObject.FindObjectOfType<Popup>(true).Init("Flavedo - programmer/designer\nsourlyx - sound designer/composer\nChapstic593 - 3d artist/ modeler\nToucan - narrative", null, "Got It");

        GameObject.FindObjectOfType<Popup>(true).showView();
    }

    public void openLevels()
    {

        StageLevelManager.Instance.selectLevel();
    }
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

        Time.timeScale = 1;

        if (StageLevelManager.Instance.currentLevelId != 0)
        {
            testNextLevel();
        }

        //EventPool.OptIn("startlevel", startlevel);
    }
    public void supportUs()
    {

        GameObject.FindObjectOfType<Popup>(true).Init("Do you want to watch ads to support us?", () => {
            //play ads
        });

        GameObject.FindObjectOfType<Popup>(true).showView();
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


    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Unity Ads  show click.");
    }
    bool isActive = false;//a bad bad bad idea but I don't know how to fix this
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (isActive && placementId.Equals(AdsManager.Instance._unitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            // say thanks!
            GameObject.FindObjectOfType<Popup>(true).Init(TextUtils.getText("thanks"),null,"return"
                );

            GameObject.FindObjectOfType<Popup>(true).showView();
        }
        else
        {

        }
        isActive = false;
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        isActive = false;
        Debug.LogError($"Unity Ads Show Failed: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Unity Ads Start show.");
    }
}
