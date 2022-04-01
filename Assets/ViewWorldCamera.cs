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


    
     public float _sensitivity;
    public float damping;
    Quaternion targetRotation;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;

    public Transform playerCamera;
    bool cameraRotated;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        resetCamera();
        EventPool.OptIn("StartGame", switchToPlayerCamera);
        EventPool.OptIn("ResetCamera", resetCamera);
        targetRotation = transform.rotation;
    }

    public void resetCamera()
    {
        transform.rotation = playerCamera.rotation;
        targetRotation = transform.rotation;
        cameraRotated = false;
    }

    public void switchToPlayerCamera()
    {
        playerCamera.gameObject.SetActive(true);
        camera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistance = camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
        currentDistance -= Input.mouseScrollDelta.y;
        currentDistance = Mathf.Clamp(currentDistance,minDistance, maxDistance);
        camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = currentDistance;

        if (Input.GetMouseButtonDown(0))
        {
            _mouseReference = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            if (!cameraRotated)
            {

                EventPool.Trigger("StartMoveCamera");
            }
            cameraRotated = true;
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);

            // apply rotation
            //_rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;
            //_rotation = Quaternion.Euler(dir);
            // rotate
            Vector3 rotateDegree = new Vector3();
            if(Mathf.Abs(_mouseOffset.y)> Mathf.Abs(_mouseOffset.x)){
                rotateDegree = new Vector3(-_mouseOffset.y, 0, 0);
            }
            else
            {

                rotateDegree = new Vector3(0, _mouseOffset.x, 0);
            }
            targetRotation *= Quaternion.Euler(rotateDegree * _sensitivity);

            // store mouse
            _mouseReference = Input.mousePosition;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,damping);
    }
}
