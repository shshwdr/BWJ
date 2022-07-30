using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlwaysHud : MonoBehaviour
{
    public Text collectedText;
    public Button pauseButton;

    public GameObject hintButton;
    public GameObject autoButton;
    public GameObject autoButtonOn;

    int collect = 0;
    bool isAutoOn = false;
    // Start is called before the first frame update
    void Start()
    {
        updateCollectedText();

        pauseButton.onClick.AddListener(delegate
        {
            GameObject.FindObjectOfType<SettingView>(true).showView();
        });
        EventPool.OptIn("updateCollected", updateCollectedText);


        updateAutoState(StageLevelManager.Instance.currentLevel.unlockedHint, StageLevelManager.Instance.playHintNext);
        StageLevelManager.Instance.playHintNext = false;
    }


    List<int> speedList = new List<int>()
    {
        0,1,2,4
    };
    public Text speedText;
    int currentSpeedIndex = 1;
    public void speedUp()
    {
        currentSpeedIndex++;
        if (currentSpeedIndex >= speedList.Count)
        {
            currentSpeedIndex = 0;
        }
        Time.timeScale = speedList[currentSpeedIndex];
        speedText.text = $"x{speedList[currentSpeedIndex] }";
    }

    public void speedDown()
    {
        currentSpeedIndex--;
        if (currentSpeedIndex < 0)
        {
            currentSpeedIndex = speedList.Count - 1;
        }
        Time.timeScale = speedList[currentSpeedIndex];
        speedText.text = $"x{speedList[currentSpeedIndex] }";

    }

    public void resumeSpeed()
    {
        Time.timeScale = speedList[currentSpeedIndex];
    }
    public void updateCollectedText()
    {
        collectedText.text = $"{StageLevelManager.Instance.currentCollected} / {StageLevelManager.Instance.currentLevel.itemCount}";
    }
    private void OnDestroy()
    {
        pauseButton.onClick.RemoveAllListeners();
    }
    //public void addCollectable()
    //{
    //    collect++;
    //    updateCollectedText();
    //}
    // Update is called once per frame
    public void getHint()
    {
        var text = TextUtils.getText("getHint");
        if (GameObject.FindObjectOfType<PlayerCubeGridMove>().startedMoving)
        {
            text += TextUtils.getText("warningToRestart");
        }

            GameObject.FindObjectOfType<Popup>(true).Init(text, () =>
        {
            //after ads play
            StageLevelManager.Instance.unlockHint();
            SaveLoadManager.saveGame();
            isAutoOn = true;
            updateAutoState(true, isAutoOn);
            GameObject.FindObjectOfType<InstructionsMenu>().hideInstructions();
        });

        GameObject.FindObjectOfType<Popup>(true).showView();
    }

    public void turnOnHint()
    {
        if (isAutoOn)
        {
            GameObject.FindObjectOfType<Popup>(true).Init(TextUtils.getText("stopHint"), () =>
            {
                isAutoOn = !isAutoOn;
                //StageLevelManager.Instance.playHint();
                GameObject.FindObjectOfType<InstructionsMenu>().showInstructions();
                updateAutoState(true, isAutoOn);
            });

            GameObject.FindObjectOfType<Popup>(true).showView();
        }
        else
        {
            var text = TextUtils.getText("startHint");
            if (GameObject.FindObjectOfType<PlayerCubeGridMove>().startedMoving)
            {
                text += TextUtils.getText("warningToRestart");
            }
            GameObject.FindObjectOfType<Popup>(true).Init(text, () =>
            {
                isAutoOn = !isAutoOn;
                StageLevelManager.Instance.playHint();
                updateAutoState(true, isAutoOn);
                GameObject.FindObjectOfType<InstructionsMenu>().hideInstructions();
            });

            GameObject.FindObjectOfType<Popup>(true).showView();

        }
    }


    public void updateAutoState(bool isHintUnlocked, bool isAutoOn)
    {
        if (!isHintUnlocked)
        {
            hintButton.SetActive(true);
            autoButton.SetActive(false);
        }
        else
        {

            hintButton.SetActive(false);
            autoButton.SetActive(true);
            autoButtonOn.SetActive(isAutoOn);
            this.isAutoOn = isAutoOn;
        }
    }
}
