using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using Pool;

public class ViewWorldCamera : MonoBehaviour
{
    public float maxDistance = 10;
    public float minDistance = 4;
    CinemachineVirtualCamera camera;

    bool startDrag = false;
    bool startPinch = false;

    public float damping;
    Quaternion targetRotation;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;

    public Transform playerCamera;
    bool cameraRotated;
    bool isCameraFree = false;
    GameObject reflectionProbe;
    // Start is called before the first frame update
    void Start()
    {

        foreach (var canvas in GameObject.FindObjectsOfType<Canvas>(true))
        {
            canvas.worldCamera = Camera.main;
        }

        camera = GetComponent<CinemachineVirtualCamera>();
        if (GameObject.FindObjectOfType<ReflectionManager>(true))
        {

            reflectionProbe = GameObject.FindObjectOfType<ReflectionManager>(true).gameObject;
        }
        resetCamera();
        EventPool.OptIn("StartGame", resetCamera);
        EventPool.OptIn("FreeCamera", freeCamera);
        EventPool.OptIn("ResetCamera", resetCamera);
        targetRotation = transform.rotation;
    }

    public void onlyResetCameraRotation()
    {
        transform.rotation = playerCamera.rotation;
        targetRotation = transform.rotation;
    }

    void freeCamera()
    {
        isCameraFree = true;

        //EventPool.Trigger("StartMoveCamera");
        playerCamera.gameObject.SetActive(false);

        if (reflectionProbe)
        {
            reflectionProbe.GetComponent<ReflectionManager>().hide();
        }
        onlyResetCameraRotation();
    }

    public void resetCamera()
    {
        isCameraFree = false;
        if (LevelManager.Instance.isLevelGameStarted)
        {
            switchToPlayerCamera();
        }
        else
        {
            onlyResetCameraRotation();

        }
        cameraRotated = false;
    }

    public void switchToPlayerCamera()
    {
        if (reflectionProbe)
        {
            reflectionProbe.GetComponent<ReflectionManager>().clearPosition();
            reflectionProbe.GetComponent<ReflectionManager>().show();
        }
        playerCamera.gameObject.SetActive(true);
        //camera.enabled = false;
    }
#if UNITY_ANDROID && !UNITY_EDITOR
    int oneFinger = 1;
    float _sensitivity = 0.4f;
#else
    int oneFinger = 2;
    float _sensitivity = 0.6f;
#endif
    public void fingerDown()
    {
        var fingers = Lean.Touch.LeanTouch.Fingers;
        if (fingers.Count > oneFinger)
        {
            Debug.Log("finger count "+fingers.Count);
            startDrag = false;
        }
        _mouseReference = Input.mousePosition;
        startDrag = true;
        Debug.Log("finger down");

    }

    public void fingerUp()
    {

        startDrag = false;
        Debug.Log("finger up");
    }

    public void fingerMoveFinger(Lean.Touch.LeanFinger finger)
    {
        var fingers = Lean.Touch.LeanTouch.Fingers;
        if (fingers.Count > oneFinger)
        {
            startDrag = false;
        }
        var pos = finger.ScreenPosition;
        Debug.Log("finger Move " + finger.ScreenPosition);
        Debug.Log("mouse Move " + (Input.mousePosition ));
        if (startDrag)
        {
            if (!cameraRotated)
            {

                cameraRotated = true;
                EventPool.Trigger("StartMoveCamera");
                //    playerCamera.gameObject.SetActive(false);

                //    if (reflectionProbe)
                //    {
                //        reflectionProbe.SetActive(false);
                //    }
                //    onlyResetCameraRotation();
            }
            // offset
            _mouseOffset = (Vector3)pos - _mouseReference;

            // apply rotation
            //_rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;
            //_rotation = Quaternion.Euler(dir);
            // rotate
            Vector3 rotateDegree = new Vector3();
            float distance = _mouseOffset.magnitude;
            if (Mathf.Abs(_mouseOffset.y) > Mathf.Abs(_mouseOffset.x))
            {
                rotateDegree = new Vector3(-distance * Mathf.Sign(_mouseOffset.y), 0, 0);
            }
            else
            {

                rotateDegree = new Vector3(0, distance * Mathf.Sign(_mouseOffset.x), 0);
            }
            targetRotation *= Quaternion.Euler(rotateDegree * _sensitivity);

            // store mouse
            _mouseReference = pos;
        }
    }

    public void scaleCamrea(float delta)
    {


        float currentDistance = camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
        currentDistance += delta;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = currentDistance;
    }

    private void Update()
    {

        if (!isCameraFree && LevelManager.Instance.isLevelGameStarted)
        {
            return;
        }

        var fingers = Lean.Touch.LeanTouch.Fingers;
        if (fingers.Count > oneFinger)
        {
            startDrag = false;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
        }



        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, damping);
    }
}
