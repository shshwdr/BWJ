using UnityEngine;
using UnityEngine.UI;
using Pool;

namespace PoolExample
{
    public class MouseManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                // Triggering an event can be done from anywhere in your code.
                EventPool.Trigger<string, int, float>("MousePos", Input.mousePosition.x.ToString(), Mathf.RoundToInt(Input.mousePosition.y), Input.mousePosition.z);
            }
        }

        private void OnEnable()
        {
            InfoPool.Provide<float>("MousePositionMagnitude", GetMousePositionMagnitude);
        }

        private void OnDisable()
        {
            InfoPool.Unprovide<float>("MousePositionMagnitude", GetMousePositionMagnitude);
        }

        private float GetMousePositionMagnitude()
        {
            return Input.mousePosition.magnitude;
        }
    }
}