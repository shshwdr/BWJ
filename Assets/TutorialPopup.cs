using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialPopup : BaseView
{
    UIView view;
    [SerializeField]
    Text label;
    VideoPlayer videoPlayer;
    GameObject video;

    // todo: how to improve this..
    public void show(bool hasVideo, string videoName)
    {
        view = GetComponent<UIView>();
        view.Show();
        label.text = TextUtils.getText(videoName);
        video.SetActive(hasVideo);
        if (hasVideo)
        {
            videoPlayer = GameObject.FindObjectOfType<VideoPlayer>();
            videoPlayer.clip = Resources.Load<VideoClip>("video/" + videoName);

        }
        Time.timeScale = 0;
    }
    public void hide()
    {
        view.Hide();
        GameObject.FindObjectOfType<AlwaysHud>().resumeSpeed();
    }

    public override void showView()
    {
        base.showView();

    }

    public override void hideView()
    {
        base.hideView();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
