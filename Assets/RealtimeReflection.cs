using UnityEngine;
using System.Collections;

public class RealtimeReflection : MonoBehaviour
{

    ReflectionProbe probe;
    Transform cameraTrans;
    public int type = 0;
    void Awake()
    {
        probe = GetComponent<ReflectionProbe>();
        cameraTrans = Camera.main.transform;
    }

    void Update()
    {
        var cameraPosition = cameraTrans.position;
        bool flipX, flipY, flipZ;
        //flipX = Mathf.Abs(playerPosition.x) > 0.9f;
        //flipY = Mathf.Abs(playerPosition.y) > 0.9f;
        //flipZ = Mathf.Abs(playerPosition.z) > 0.9f;
        //if (flipX && !flipY && !flipZ)
        //{
        //    probe.transform.position = new Vector3(
        //        Mathf.Sign(cameraPosition.x) * (Mathf.Abs(cameraPosition.x) - 2),
        //        cameraPosition.y,
        //       cameraPosition.z
        //    );
        //}
        //if (flipY && !flipX && !flipZ)
        //{
        //    probe.transform.position = new Vector3(
        //        cameraPosition.x,
        //        Mathf.Sign(cameraPosition.y) * (Mathf.Abs(cameraPosition.y) - 2),
        //       cameraPosition.z
        //    );
        //}
        //if (flipZ && !flipY && !flipX)
        //{
        //    probe.transform.position = new Vector3(
        //        cameraPosition.x,
        //        cameraPosition.y,
        //       Mathf.Sign(cameraPosition.z) * (Mathf.Abs(cameraPosition.z) - 2)
        //    );
        //}
        if(type == 0)
        {
            probe.transform.position = new Vector3(
                Mathf.Sign(cameraPosition.x) * (1.5f - (Mathf.Abs(cameraPosition.x) - 1.5f)),
                cameraPosition.y,
               cameraPosition.z
            );
        }
        else if(type == 1)
        {
            probe.transform.position = new Vector3(
                cameraPosition.x,
                Mathf.Sign(cameraPosition.y) * (1.5f - (Mathf.Abs(cameraPosition.y) - 1.5f)),
               cameraPosition.z
            );
        }
        else
        {
            probe.transform.position = new Vector3(
                    cameraPosition.x,
                    cameraPosition.y,
                   Mathf.Sign(cameraPosition.z) * (1.5f - (Mathf.Abs(cameraPosition.z) - 1.5f))
                );

        }
        probe.RenderProbe();
    }
}