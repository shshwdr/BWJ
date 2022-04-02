using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionCell : MonoBehaviour
{
    public Button button;
    public Text text;
    public Transform[] stars;

    public void init(LevelInfo info) {
        text.text = info.displayName;
        button.onClick.AddListener(delegate
        {
            
            StageLevelManager.Instance.startLevel(info.id);


            LevelSelectionView view = GameObject.FindObjectOfType<LevelSelectionView>(true);
            view.hideReward();
        });

        for (int i = 0; i < StageLevelManager.Instance.starCountInLevel(info.sceneName); i++)
        {
            stars[i].gameObject.SetActive(true);
        } 
    
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
