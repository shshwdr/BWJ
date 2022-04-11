using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlwaysHud : MonoBehaviour
{
    public Text collectedText;
    public Button pauseButton;
    int collect = 0;
    // Start is called before the first frame update
    void Start()
    {
        updateCollectedText();

        pauseButton.onClick.AddListener(delegate
        {
            GameObject.FindObjectOfType<SettingView>(true).showView();
        });
        EventPool.OptIn("updateCollected", updateCollectedText);
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
        if(currentSpeedIndex>= speedList.Count)
        {
            currentSpeedIndex = 0;
        }
        Time.timeScale = speedList[currentSpeedIndex];
        speedText.text = $"x{speedList[currentSpeedIndex] }";
    }

    public void speedDown()
    {
        currentSpeedIndex--;
        if (currentSpeedIndex <0)
        {
            currentSpeedIndex = speedList.Count-1;
        }
        Time.timeScale = speedList[currentSpeedIndex];
        speedText.text = $"x{speedList[currentSpeedIndex] }";

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
    void Update()
    {
        
    }
}
