using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using Kino;
using DG.Tweening;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Syntax: CamShake(shakeAmount, decreaseFactor, shakeDuration)
    /// </summary>
    public class SequencerCommandGlitch : SequencerCommand
    {

        float scanLineJitter = 0;
        float colorDrift = 0;

        private AnalogGlitch glitchObject;

        public void Start()
        {
            scanLineJitter = GetParameterAsFloat(0);
            colorDrift = GetParameterAsFloat(1);
            glitchObject = Camera.main.gameObject.GetComponent<AnalogGlitch>();//Sequencer.SequencerCamera.gameObject;
                                                                               // glitchObject.scanLineJitter = scanLineJitter;
                                                                               //  glitchObject.colorDrift = colorDrift;
            //DOTween.Clear();
            DOTween.To(() => glitchObject.scanLineJitter, x => glitchObject.scanLineJitter = x, scanLineJitter, 0.5f).SetUpdate(true);
            DOTween.To(() => glitchObject.colorDrift, x => glitchObject.colorDrift = x, colorDrift, 0.5f).SetUpdate(true);

        }

        void Update()
        {
        }

        void OnDestroy()
        {

        }
    }
}