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
    float rotationSpeed = 1;

    bool startDrag = false;
    
     public float _sensitivity;
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

            EventPool.Trigger("StartMoveCamera");
            playerCamera.gameObject.SetActive(false);

            if (reflectionProbe)
            {
                reflectionProbe.GetComponent<ReflectionManager>(). hide();
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

    // Update is called once per frame
    void Update()
    {

        if(!isCameraFree && LevelManager.Instance.isLevelGameStarted)
        {
            return;
        }

        float currentDistance = camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
        currentDistance -= Input.mouseScrollDelta.y;
        currentDistance = Mathf.Clamp(currentDistance,minDistance, maxDistance);
        camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = currentDistance;

        if (Input.GetMouseButtonUp(0))
        {
            startDrag = false;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseReference = Input.mousePosition;
                startDrag = true;
            }
            if (Input.GetMouseButton(0)&& startDrag)
            {
                if (!cameraRotated)
                {

                    EventPool.Trigger("StartMoveCamera");
                //    playerCamera.gameObject.SetActive(false);

                //    if (reflectionProbe)
                //    {
                //        reflectionProbe.SetActive(false);
                //    }
                //    onlyResetCameraRotation();
                }
                cameraRotated = true;
                // offset
                _mouseOffset = (Input.mousePosition - _mouseReference);

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
                _mouseReference = Input.mousePosition;
            }
        }

        

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,damping);
    }
}
