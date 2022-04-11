
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

        FMODUnity.RuntimeManager.PlayOneShot("event:/menu in");
        //audioSource = GameObject.Find("sfx").GetComponent<AudioSource>();
        //audioSource.PlayOneShot(showAudio);
    }
    public virtual void hideView()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/menu out");
        //audioSource = GameObject.Find("sfx").GetComponent<AudioSource>();
        //audioSource.PlayOneShot(hideAudio);
    }
}