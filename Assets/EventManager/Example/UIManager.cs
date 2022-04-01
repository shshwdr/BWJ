using UnityEngine;
using UnityEngine.UI;
using Pool;

namespace PoolExample
{
    public class UIManager : MonoBehaviour
    {
        public Text text;
        public Text buttonText;

        private void OnEnable()
        {
            EventPool.OptIn<string, int, float>("MousePos", MousePosCallback);
        }

        private void OnDisable()
        {
            EventPool.OptOut<string, int, float>("MousePos", MousePosCallback);
        }

        private void Start()
        {
            text.text = "Press \"s\" key or hit the button";
        }

        private void MousePosCallback(string s, int i, float f)
        {
            text.text = "mouse position: " + s + " " + i + " " + f;
            Debug.Log(text.text);
        }

        public void UpdateButtonText()
        {
            float magnitude = InfoPool.Request<float>("MousePositionMagnitude");
            string magnitudeText = "magnitude: " + (Mathf.RoundToInt(magnitude * 10) / 10f).ToString();
            buttonText.text = magnitudeText;
            Debug.Log(magnitudeText);
        }
    }
}