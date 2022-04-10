using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button newGameButton;
    public Button continueButton;
    public Button levelButton;
    // Start is called before the first frame update
    void Start()
    {

        levelButton.onClick.AddListener(delegate { StageLevelManager.Instance.selectLevel(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
