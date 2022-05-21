using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineArrow : MonoBehaviour
{
    public float scrollSpeed = 1f;
    public LineRenderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<LineRenderer>();
        _renderer.alignment = LineAlignment.TransformZ;
    }

    // Update is called once per frame
    void Update()
    {
        float vOffset = Time.time * scrollSpeed;
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(vOffset, 0));
    }
}
