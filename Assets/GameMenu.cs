using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    PlayerCubeGridMove player;
    public Button startGame;
    public Button resetCamera;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        startGame.onClick.AddListener(delegate {
            LevelManager.Instance.startGame();
            startGame.gameObject.SetActive(false);
        });
        resetCamera.onClick.AddListener(delegate {
            EventPool.Trigger("ResetCamera");

            resetCamera.gameObject.SetActive(false);
        });
        resetCamera.gameObject.SetActive(false);
        EventPool.OptIn("StartMoveCamera", showResetCameraButton);
    }

    void showResetCameraButton()
    {
        resetCamera.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
