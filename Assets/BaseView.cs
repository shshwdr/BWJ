
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseView: MonoBehaviour
{
    public AudioClip showAudio;
    public AudioClip hideAudio;
    protected AudioSource audioSource;
    float originTimeScale;

    protected virtual void Start()
    {
        //audioSource = GameObject.Find("sfx").GetComponent<AudioSource>();
    }
    public virtual void showView()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/menu in");
        originTimeScale = Time.timeScale;
        Time.timeScale = 0;
        //audioSource = GameObject.Find("sfx").GetComponent<AudioSource>();
        //audioSource.PlayOneShot(showAudio);
    }
    public virtual void hideView()
    {
        Time.timeScale = originTimeScale;
        FMODUnity.RuntimeManager.PlayOneShot("event:/menu out");
        //audioSource = GameObject.Find("sfx").GetComponent<AudioSource>();
        //audioSource.PlayOneShot(hideAudio);
    }
}