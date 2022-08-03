using DG.Tweening;
using Doozy.Engine.UI;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : BaseView
{

    public Button nextLevelButton;
    public Button restartButton;
    public Button levelSelectionButton;
    public Text levelText;
    public Text collectedTotalText;
    public Text collectedLevelText;
    public GameObject panel;
    public Transform[] stars;
    public AudioClip[] starClips;
    public Text description;
    public Text title;



    public override void showView()
    {
        base.showView();
        GetComponent<UIView>().Show();
        panel.SetActive(true);

        UpdateTextDisplay();
        
        var hasNextLevel = StageLevelManager.Instance.hasNextLevel();
        nextLevelButton.onClick.RemoveAllListeners();
        if (hasNextLevel)
        {

            nextLevelButton.onClick.AddListener(delegate
            {

                StageLevelManager.Instance.addLevel();
                StageLevelManager.Instance.startNextLevel();

            });
            nextLevelButton.GetComponentInChildren<Text>().text = Translator.Instance.Translate( "Next");
        }
        else
        {
            nextLevelButton.GetComponentInChildren<Text>().text = Translator.Instance.Translate("End");


            nextLevelButton.onClick.AddListener(delegate
            {

                if (StageLevelManager.Instance.collectedAll())
                {

                    DialogueManager.StartConversation($"final_good");
                }
                else
                {

                    DialogueManager.StartConversation($"final_bad");
                }

            });
        }

        //if (StageLevelManager.Instance.currentLevelId == 0)
        //{
        //    nextLevelButton.gameObject.SetActive(false);

        //    levelSelectionButton.gameObject.SetActive(false);
        //}
        StartCoroutine(showStars());
    }

    IEnumerator showStars()
    {
        yield return null;
        int starCount = StageLevelManager.Instance.starCount();
        for (int i = 0; i < starCount; i++)
        {
            //GameManager.popup(stars[i],true);
            // audioSource.PlayOneShot(starClips[i]);
            yield return new WaitForSecondsRealtime(0.7f);
            //Sequence mySequence = DOTween.Sequence();
            //mySequence.Append(stars[i].DOScale(Vector3.one*1.5f, 0.7f))
            //  .Append(stars[i].DOScale(Vector3.one * 1f, 0.7f));
            //stars[i].DoScale(new Vector3(1, 1, 1),1);
            // stars[i].transform
        }
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
        restartButton.onClick.AddListener(delegate
        {
            StageLevelManager.Instance.startNextLevel();
        });
        //returnButton.onClick.AddListener(delegate { StageLevelManager.Instance.returnHome(); });
        levelSelectionButton.onClick.AddListener(delegate { StageLevelManager.Instance.selectLevel(); });
        Translator.Instance.DisplayLanguageChanged += UpdateTextDisplay;
    }

    void UpdateTextDisplay()
    {
        Translator translator = Translator.Instance;
        int displayLanguage = translator.GetUserDisplayLanguageIndex();



        levelText.text = $"{translator.Translate("Level", displayLanguage)}: {StageLevelManager.Instance.currentLevel.displayName}";
        collectedTotalText.text = $"{translator.Translate("Collected In Total", displayLanguage)}: {StageLevelManager.Instance.totalCollected}/{StageLevelManager.Instance.totalCanCollect}";
        collectedLevelText.text = $"{translator.Translate("Collected In Level", displayLanguage)}: {StageLevelManager.Instance.currentLevel.collectedCount}/{StageLevelManager.Instance.currentLevel.itemCount}";
    }

}
