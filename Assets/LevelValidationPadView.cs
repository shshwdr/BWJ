//#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void clear()
    {

        gameObject.SetActive(true);
        if (cells == null || cells.Length == 0)
        {
            cells = GetComponentsInChildren<LevelValidationPadCell>(true);

        }
        foreach(var cell in cells)
        {
            cell.gameObject.SetActive(false);
        }
        cells[0].transform.parent.parent.GetComponentInParent<Image>().color = new Color(0, 0, 0, 0.5f);
    }
    public void setValidationInfo()
    {
        //if(canArrive)
        {
            cells[0].transform.parent.parent.GetComponentInParent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
        //else
        //{

        //    cells[id].gameObject.SetActive(true);
        //}
    }
}
//#endif