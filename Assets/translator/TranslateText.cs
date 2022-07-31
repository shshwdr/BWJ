// TranslateText.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
//[RequireComponent(typeof(Text),typeof(TMP_Text))]

// Place on all t:TextMeshProUGUI or t:Text
public class TranslateText : MonoBehaviour
{
    bool init = false;
    string englishText;
    int lastLanguageUpdated = -1;
    Text txtDisplay;
    TMP_Text txtDisplay_TMP;

    void Start()
    {
        Init();
        UpdateTextDisplay();
    }

    private void OnEnable()
    {
        if (init)
            UpdateTextDisplay();
    }

    public void SetEnglishText(string newText)
    {
        Init();

        englishText = newText;

        if (txtDisplay_TMP)
        {

            txtDisplay_TMP.text = englishText;
        }
        else
        {

            txtDisplay.text = englishText;
        }

        lastLanguageUpdated = -1;

        UpdateTextDisplay();
    }

    void Init()
    {
        if (init)
            return;

        txtDisplay = GetComponent<Text>();
        txtDisplay_TMP = GetComponent<TMP_Text>();
        if (!txtDisplay && !txtDisplay_TMP)
        {
            Debug.LogError(" no text! "+name);
        }
        // UnityEngine.Assertions.Assert.IsNotNull(txtDisplay);
        if (txtDisplay_TMP)
        {

            englishText = txtDisplay_TMP.text;
        }
        else
        {

            englishText = txtDisplay.text;
        }

        lastLanguageUpdated = -1;
        init = true;

        Translator.Instance.DisplayLanguageChanged += UpdateTextDisplay;
    }

    internal void ManuallySetTextToLoading(int displayLanguage)
    {
        Init();
        string[] loadingTranslations = {"Loading...", "Chargement...", "Caricamento in corso...", "Wird geladen...",
        "Cargando...",  "载入中...",   "Loading...",   "Carregando..."};
        int langIndex = displayLanguage + 1;
        UnityEngine.Assertions.Assert.IsTrue(langIndex >= 0 && langIndex < loadingTranslations.Length);
        if (txtDisplay_TMP)
        {

            txtDisplay_TMP.text = loadingTranslations[langIndex];
        }
        else
        {

            txtDisplay.text = loadingTranslations[langIndex];
        }
    }

    void UpdateTextDisplay()
    {
        if (this == null)
            return;

        Init();
        Translator translator = Translator.Instance;
        int displayLanguage = translator.GetUserDisplayLanguageIndex();

        string translatedText = translator.Translate(englishText.Trim(), displayLanguage);
        if (displayLanguage == lastLanguageUpdated)
            return;

        lastLanguageUpdated = displayLanguage;
        if (translator.IsDisplayingEnglish())
        {
            if (txtDisplay_TMP)
            {

                txtDisplay_TMP.text = englishText;
            }
            else
            {

                txtDisplay.text = englishText;
            }
        }
        else
        {
            UnityEngine.Assertions.Assert.IsNotNull(translatedText);
            if (txtDisplay_TMP)
            {

                UnityEngine.Assertions.Assert.IsNotNull(txtDisplay_TMP, gameObject.name);
                txtDisplay_TMP.text = translatedText;
            }
            else
            {

                UnityEngine.Assertions.Assert.IsNotNull(txtDisplay, gameObject.name);
                txtDisplay.text = translatedText;
            }
        }
    }
}
