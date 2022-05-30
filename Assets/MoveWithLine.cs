using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    public float speed = 1f;
    private Vector3[] pos;
    private int index = 0;
    Vector3 up;
    public float upValue = 0.07f;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void init(LineRenderer line)
    {

        lineRenderer = line;
        pos = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(pos);
        up = transform.parent.up;
        up *= upValue;
        transform.position = pos[0] + up;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position,
                                                pos[index] + up,
                                                speed * Time.deltaTime);

        if (transform.position == pos[index] + up)
        {
            index += 1;
        }

        if (index == pos.Length)
        {
            index = 0;
            transform.position = pos[0] + up;

        }
    }
}
