using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : BaseView
{

    public Button restartButton;
    public Button returnButton;
    public Button levelSelectionButton;
    public Button resumeButton;
    public Text levelText;
    public GameObject panel;

    public override void showView()
    {
        base.showView();
       // Time.timeScale = 0;
        GetComponent<UIView>().Show();
        panel.SetActive(true);
        //StageLevelManager.Instance.addLevel();
        restartButton.gameObject.SetActive(true);
        
        levelText.text = $"LEVEL {StageLevelManager.Instance.currentLevel.displayName}";
        //GameManager.Instance.saveAnimalInLevel();

    }
    public override void hideView()
    {
        base.hideView();
       //Time.timeScale = 1;

        GetComponent<UIView>().Hide();
        //panel.SetActive(false);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        restartButton.onClick.AddListener(delegate {
            StageLevelManager.Instance.startNextLevel();
        });
        resumeButton.onClick.AddListener(delegate {
            hideView();
        });
        //returnButton.onClick.AddListener(delegate { StageLevelManager.Instance.returnHome(); });
        levelSelectionButton.onClick.AddListener(delegate { StageLevelManager.Instance.selectLevel(); });
    }
}
