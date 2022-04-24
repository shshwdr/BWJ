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
    bool toResetCamera = false;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        text = resetCamera.GetComponentInChildren<Text>();
        startGame.onClick.AddListener(delegate {
            LevelManager.Instance.startGame();
            startGame.gameObject.SetActive(false);
        });
        resetCamera.onClick.AddListener(delegate {
            resetCameraFunc();
        });
        resetCamera.gameObject.SetActive(false);
        EventPool.OptIn("StartGame", setCamera);
        EventPool.OptIn("StartMoveCamera", showResetCameraButton);
    }

    public void StartFreeCamera()
    {
        if (toResetCamera)
        {

            if (LevelManager.Instance.isLevelGameStarted)
            {
                text.text = "Reset Camera";
            }
            EventPool.Trigger("FreeCamera");

            resetCamera.gameObject.SetActive(true);
            toResetCamera = !toResetCamera;
        }
    }

    public void resetCameraFunc()
    {
        if (!LevelManager.Instance.isLevelGameStarted)
        {
            resetCamera.gameObject.SetActive(false);
            EventPool.Trigger("ResetCamera");
        }
        else
        {
            if (toResetCamera)
            {

            }
            else //to reset camera
            {
                resetCamera.gameObject.SetActive(false);
                text.text = "Free Camera";

                EventPool.Trigger("ResetCamera");
            }
        }



        toResetCamera = !toResetCamera;
    }
    void setCamera()
    {
        resetCamera.gameObject.SetActive(true);
        text.text = "Free Camera";
        toResetCamera = true;
    }
    void showResetCameraButton()
    {
        StartFreeCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
