using DG.Tweening;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : BaseView
{

    public Button nextLevelButton;
    public Button returnButton;
    public Button levelSelectionButton;
    public Text levelText;
    public GameObject panel;
    public Transform[] stars;
    public AudioClip[] starClips;
    public Text description;
    public Text title;

    public override void showReward()
    {
        base.showReward();
        GetComponent<UIView>().Show();
        panel.SetActive(true);
        levelText.text = $"Level {StageLevelManager.Instance.currentLevel.displayName}";
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
        var test = StageLevelManager.Instance.hasNextLevel();
        if (test)
        {
            nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            nextLevelButton.gameObject.SetActive(false);

            description.text = "Thanks for Playing!";

        }

        if (StageLevelManager.Instance.currentLevelId == 0)
        {
            nextLevelButton.gameObject.SetActive(false);

            levelSelectionButton.gameObject.SetActive(false);
        }
        StartCoroutine(showStars());
    }

    IEnumerator showStars()
    {
        yield return null;
        int starCount = StageLevelManager.Instance.starCount();
        for (int i = 0; i < starCount; i++)
        {
            //GameManager.popup(stars[i],true);
            audioSource.PlayOneShot(starClips[i]);
            yield return new WaitForSecondsRealtime(0.7f);
            //Sequence mySequence = DOTween.Sequence();
            //mySequence.Append(stars[i].DOScale(Vector3.one*1.5f, 0.7f))
            //  .Append(stars[i].DOScale(Vector3.one * 1f, 0.7f));
            //stars[i].DoScale(new Vector3(1, 1, 1),1);
            // stars[i].transform
        }
    }
    public override void hideReward()
    {
        base.hideReward();
        GetComponent<UIView>().Hide();
        panel.SetActive(false);
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        nextLevelButton.onClick.AddListener(delegate {
            //if (StageLevelManager.Instance.getMainTargetFinish())
            //{
            //    StageLevelManager.Instance.addLevel();
            //}
            
            StageLevelManager.Instance.startNextLevel(); });
        //returnButton.onClick.AddListener(delegate { StageLevelManager.Instance.returnHome(); });
        levelSelectionButton.onClick.AddListener(delegate { StageLevelManager.Instance.selectLevel(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
