using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelValidationPadView : MonoBehaviour
{
    LevelValidationPadCell[] cells;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValidationInfo(int id, string data)
    {
        if(cells == null || cells.Length == 0)
        {
            cells = GetComponentsInChildren<LevelValidationPadCell>(true);

        }
        cells[id].gameObject.SetActive(true);
    }
}
