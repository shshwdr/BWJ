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
        levelText.text = $"Level {StageLevelManager.Instance.currentLevel.displayName}";
        collectedTotalText.text = $"Collected In Total: {StageLevelManager.Instance.totalCollected}/{StageLevelManager.Instance.totalCanCollect}";
        collectedLevelText.text = $"Collected In Level: {StageLevelManager.Instance.currentLevel.collectedCount}/{StageLevelManager.Instance.currentLevel.itemCount}";
        //GameManager.Instance.saveAnimalInLevel();

        //if (StageLevelManager.Instance.getTargetFinish())
        //{

        //    title.text = "Exellent Work";
        //    description.text = "Every life got saved!";
        //}
        //else if (StageLevelManager.Instance.getMainTargetFinish())
        //{
        //    title.text = "Great Work";
        //    description.text = "You saved some lifes but missed some. Try to save them next time!";

        //}
        //else
        //{
        //    title.text = "Try Again";
        //    description.text = "Lifes are waiting for you to be saved!";
        //}


        //StageLevelManager.Instance.addLevel();
        var hasNextLevel = StageLevelManager.Instance.hasNextLevel();
        nextLevelButton.onClick.RemoveAllListeners();
        if (hasNextLevel)
        {

            nextLevelButton.onClick.AddListener(delegate
            {

                StageLevelManager.Instance.addLevel();
                StageLevelManager.Instance.startNextLevel();

            });
            nextLevelButton.GetComponentInChildren<Text>().text = "Next";
        }
        else
        {
            nextLevelButton.GetComponentInChildren<Text>().text = "End";


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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
