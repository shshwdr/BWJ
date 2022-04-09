using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialView : MonoBehaviour
{
    public Button previousButton;
    public Button nextButton;
    public Text tutorialText;
    int currentId = -1;
    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("updateTutorial",updateTutorial);
        previousButton.onClick.AddListener(delegate
        {
            currentId--;
            updateUI();
        });
        nextButton.onClick.AddListener(delegate
        {
            currentId++;
            updateUI();
        });
        updateTutorial();
    }

    public void updateUI()
    {

        if(TutorialManager.Instance.unlockedTutorialList.Count == 0)
        {
            return;
        }
        if (currentId < 0)
        {
            Debug.LogError("current id less than 0");
            currentId = 0;
        }
        if (currentId >= TutorialManager.Instance.unlockedTutorialList.Count)
        {
            Debug.LogError("current id larger than max");
            currentId = TutorialManager.Instance.unlockedTutorialList.Count-1;
        }
        tutorialText.text = TextUtils.getText(TutorialManager.Instance.unlockedTutorialList[currentId]);
        if (currentId == TutorialManager.Instance.unlockedTutorialList.Count - 1)
        {
            nextButton.gameObject.SetActive(false);
        }
        else
        {

            nextButton.gameObject.SetActive(true);
        }
        previousButton.gameObject.SetActive(currentId != 0);
    }
    public void updateTutorial()
    {
        currentId = TutorialManager.Instance.unlockedTutorialList.Count-1;

        updateUI();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
