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
    }
    public void updateCollectedText()
    {
        collectedText.text = $"{collect} / 3";
    }
    public void addCollectable()
    {
        collect++;
        updateCollectedText();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
