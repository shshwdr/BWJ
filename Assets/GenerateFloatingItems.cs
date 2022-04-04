using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloatingItems : MonoBehaviour
{
    public GameObject[] prefabs;
    public int loadItemCount = 10;
    public int loadItemCountFar = 10;
    public float shortRadius = 100;
    public float longRadius = 1000;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < loadItemCount; i++)
        {
            var position = Utils.randomVector3(Vector3.zero, shortRadius);
            var target = Utils.randomVector3(Vector3.zero, shortRadius);

            var radius = (position - target).magnitude;
            if (radius <= 10||(target).magnitude - radius <= 10)
            {
                i--;
                continue;
            }

            var go = Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.identity, transform);

            go.GetComponent<RotateAround>().target = target;
            go.GetComponent<RotateAround>().speed = Random.Range(1, 10);
        }
        for (int i = 0; i < loadItemCountFar; i++)
        {
            var position = Utils.randomVector3(Vector3.zero, longRadius);
            var target = Utils.randomVector3(Vector3.zero, longRadius);

            var radius = (position - target).magnitude;

            if (radius <= 10 || (target).magnitude - radius <= 10)
            {
                i--;
                continue;
            }

            var go = Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.identity, transform);

            go.GetComponent<RotateAround>().target = target;
            go.GetComponent<RotateAround>().speed = Random.Range(1, 10);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
