using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionView : BaseView
{
    public Button nextLevelButton;
    public Button returnButton;
    public Button levelSelectionButton;
    public Text levelText;
    public GameObject panel;
    public Text collectedTotalText;
    public override void showView()
    {
        base.showView();
        //panel.SetActive(true);

        collectedTotalText.text = $"Collected In Total: {StageLevelManager.Instance.totalCollected}/{StageLevelManager.Instance.totalCanCollect}";
        GetComponent<UIView>().Show();
        var levelButtons = GetComponentsInChildren<LevelSelectionCell>(); 
        int i = 0;
        for (; i < levelButtons.Length; i++)
        {
            levelButtons[i].gameObject.SetActive(true);
            levelButtons[i].init(StageLevelManager.Instance.levelInfoList[i]);
        }
        //for(;i< levelButtons.Length; i++)
        //{
        //    levelButtons[i].gameObject.SetActive(false);
        //}
    }
    public override void hideView()
    {
        base.hideView();

        GetComponent<UIView>().Hide();
        //panel.SetActive(false);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //nextLevelButton.onClick.AddListener(delegate { StageLevelManager.Instance.startNextLevel(); });
        returnButton.onClick.AddListener(delegate { hideView(); });
    }
}
