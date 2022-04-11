using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using Kino;
using DG.Tweening;
using FMODUnity;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Syntax: CamShake(shakeAmount, decreaseFactor, shakeDuration)
    /// </summary>
    public class SequencerCommandFMODEvent : SequencerCommand
    {
        string eventName;
        bool turnon;

        public void Start()
        {
            turnon = GetParameterAsBool(1);
            if (turnon)
            {

                GameObject.Find("glitchSound").GetComponent<StudioEventEmitter>().Play();
            }
            else
            {

                GameObject.Find("glitchSound").GetComponent<StudioEventEmitter>().Stop();
            }
            //FMODUnity.RuntimeManager.PlayOneShot("event:/"+eventName);
            //scanLineJitter = GetParameterAsFloat(0);
            //colorDrift = GetParameterAsFloat(1);
            //glitchObject = Camera.main.gameObject.GetComponent<AnalogGlitch>();//Sequencer.SequencerCamera.gameObject;
            //                                                                   // glitchObject.scanLineJitter = scanLineJitter;
            //                                                                   //  glitchObject.colorDrift = colorDrift;
            ////DOTween.Clear();
            //DOTween.To(() => glitchObject.scanLineJitter, x => glitchObject.scanLineJitter = x, scanLineJitter, 0.5f).SetUpdate(true);
            //DOTween.To(() => glitchObject.colorDrift, x => glitchObject.colorDrift = x, colorDrift, 0.5f).SetUpdate(true);

        }

        void Update()
        {
        }

        void OnDestroy()
        {

        }
    }
}