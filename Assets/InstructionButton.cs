using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionButton : MonoBehaviour
{
    public GameObject onText;
    public GameObject offText;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn<int>("turnedInstructionOff", turnedInstructionOff);
        turnedInstructionOff(index);
    }
    void turnedInstructionOff(int i)
    {
        if (i == index)
        {
            offText.SetActive(true);
            onText.SetActive(false);
        }
    }
    public void turnedInstructionOn(int i)
    {
        if (i == index)
        {
            offText.SetActive(false);
            onText.SetActive(true);
        }
    }

    public void turnInstruction(int i, bool on)
    {
        if (on)
        {
            turnedInstructionOn(i);
        }
        else
        {
            turnedInstructionOff(i);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
