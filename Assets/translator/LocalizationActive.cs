using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationActive : MonoBehaviour
{
    public bool showOnlyInChina = true;
    // Start is called before the first frame update
    void Start()
    {
        //Translator.Instance.DisplayLanguageChanged += UpdateTextDisplay;
        if (!AdsManager.isChina)
        {
            gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
