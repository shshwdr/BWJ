using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialPopup : MonoBehaviour
{
    UIView view;
    [SerializeField]
    Text label;
    VideoPlayer videoPlayer;


    public void show(string text, string videoName)
    {
        view = GetComponent<UIView>();
        videoPlayer = GameObject.FindObjectOfType<VideoPlayer>();
        view.Show();
        label.text = text;
        videoPlayer.clip = Resources.Load<VideoClip>("video/" + videoName);  
    }
    public void hide()
    {
        view.Hide();
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
