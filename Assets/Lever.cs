using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lever : MonoBehaviour
{
    public Transform rotateObject;
    Quaternion targetRotation;
    bool isTriggered = false;

    public void rotate()
    {
        GetComponentInChildren<Animator>().SetTrigger("pull");
        
        var target = rotateObject.transform.rotation * Quaternion.Euler(Vector3.up * 90);
        rotateObject.transform.DORotateQuaternion(target, 0.5f);
        //rotateObject.transform.rotation = Quaternion.Slerp(rotateObject.transform.rotation, target, Time.deltaTime);
        //rotateObject.transform.DO
        // targetRotation = rotateObject.transform.rotation * Quaternion.Euler(Vector3.up * 90);
    }
    // Start is called before the first frame update
    void Start()
    {
        //targetRotation = transform.rotation;
       // DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {

        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
    }
}
