using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionManager : MonoBehaviour
{
    public Transform[] reflections;
    Transform player;
    float currentX, currentY, currentZ;
    GameObject currentReflection;
    // Start is called before the first frame update
    void Start()
    {
        //reflections = GetComponentsInChildren<Transform>();
        foreach(var reflec in reflections)
        {
            reflec.gameObject.SetActive(false);
        }
        player = Camera.main.transform;

        updateOneProb(0);
        updateOneProb(1);
        updateOneProb(2);
        updatePosition();

    }

    void updateOneProb(int index)
    {
        bool shouldUpdate = false;
        if(index == 0)
        {
            shouldUpdate =( currentX<1.4f&& player.position.x>=1.4f);
        }else if (index == 1)
        {

            shouldUpdate = (currentY < 1.4f && player.position.y >= 1.4f);
        }
        else
        {

            shouldUpdate = (currentZ < 1.4f && player.position.z >= 1.4f);
        }
        if (shouldUpdate)
        {
            if (currentReflection)
            {
                currentReflection.SetActive(false);
            }
            reflections[index].gameObject.SetActive(true);
            currentReflection = reflections[index].gameObject;
        }
    }

    void updatePosition()
    {

        currentX = player.transform.position.x;
        currentY = player.transform.position.y;
        currentZ = player.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {

        updateOneProb(0);
        updateOneProb(1);
        updateOneProb(2);
        updatePosition();
    }
}
