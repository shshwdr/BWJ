
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseView: MonoBehaviour
{
    public AudioClip showAudio;
    public AudioClip hideAudio;
    protected AudioSource audioSource;


    protected virtual void Start()
    {
        //audioSource = GameObject.Find("sfx").GetComponent<AudioSource>();
    }
    public virtual void showView()
    {
        //audioSource = GameObject.Find("sfx").GetComponent<AudioSource>();
        //audioSource.PlayOneShot(showAudio);
    }
    public virtual void hideView()
    {
        //audioSource = GameObject.Find("sfx").GetComponent<AudioSource>();
        //audioSource.PlayOneShot(hideAudio);
    }
}