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
            if (!LevelManager.Instance.isLevelGameStarted)
            {
                resetCamera.gameObject.SetActive(false);
                EventPool.Trigger("ResetCamera");
            }
            else
            {
                if (toResetCamera)
                {
                    if (LevelManager.Instance.isLevelGameStarted)
                    {
                        text.text = "Reset Camera";
                    }
                    EventPool.Trigger("FreeCamera");

                }
                else //to reset camera
                {

                        text.text = "Free Camera";
                    
                    EventPool.Trigger("ResetCamera");
                }
            }


                
            toResetCamera = !toResetCamera;
        });
        resetCamera.gameObject.SetActive(false);
        EventPool.OptIn("StartGame", setCamera);
        EventPool.OptIn("StartMoveCamera", showResetCameraButton);
    }
    void setCamera()
    {
        resetCamera.gameObject.SetActive(true);
        text.text = "Free Camera";
        toResetCamera = true;
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
