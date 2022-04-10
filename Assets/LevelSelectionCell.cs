using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionCell : MonoBehaviour
{
    public Button button;
    public Text text;
    public Transform[] stars;
    public Text collected;

    public void init(LevelInfo info) {
        text.text = info.displayName;
        button.onClick.AddListener(delegate
        {
            if (StartMenu.Instance.hasStartedMainLevel)
            {

                StageLevelManager.Instance.startLevel(info.id);
            }
            else
            {
                StageLevelManager.Instance.currentLevelId = info.id;
                StartMenu.Instance.moveToMainLevel();
            }


            LevelSelectionView view = GameObject.FindObjectOfType<LevelSelectionView>(true);
            view.hideView();
        });

        for (int i = 0; i < StageLevelManager.Instance.starCountInLevel(info.sceneName); i++)
        {
            stars[i].gameObject.SetActive(true);
        }
        collected.text = $"{info.collectedCount}/{info.itemCount}";


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
