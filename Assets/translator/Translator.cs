// Translator.cs
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Translator : Singleton<Translator>
{
    public static readonly string kUnTranslatedTermsListPath = "unTranslatedTermsList.txt";

    public delegate void OnDisplayLanguageChangedHandler();
    public event OnDisplayLanguageChangedHandler DisplayLanguageChanged = delegate { };

    public TermData.Terms termData;
    private const int kEnglishIndex = -1;
    private const int kChineseIndex = 0;
    private int currentLanguageIndex = 0;// chinese

    private string unTranslatedTerms = "";

    public int GetUserDisplayLanguageIndex()
    {
        return currentLanguageIndex;
    }

    public bool IsDisplayingEnglish()
    {
        return currentLanguageIndex == kEnglishIndex;
    }

    public void InvokeDisplayLanguageChanged()
    {
        DisplayLanguageChanged();
    }

    //public void EnsureLoadThenSetDisplayLanguage(int index, Action<string> globalCallback)
    //{
    //    System.Action<string> localCallback = new System.Action<string>(delegate (string error)
    //    {
    //        if (error == null)
    //        {
    //            ServiceLocator.instance.GetService<Translator>().SetDisplayLanguage(index);
    //            if (ServiceLocator.instance.GetService<Settings>().collectAllDialogueTermData)
    //                ServiceLocator.instance.GetService<TranslationLoader>().DebugCollectAllDialogueTermData();
    //        }
    //        else
    //        {
    //            Debug.LogError("Langauge set failed: " + error);
    //        }

    //        if (globalCallback != null)
    //            globalCallback(error);
    //    });

    //    ServiceLocator.instance.GetService<Translator>().EnsureTranslationsLoaded(localCallback);
    //}

    public void SetDisplayLanguage(int index)
    {
        PlayerPrefs.SetInt("displayLanguage", index);
        UnityEngine.Assertions.Assert.IsTrue(termData != null);
        currentLanguageIndex = index;
        DisplayLanguageChanged();
        DialogueManager.SetLanguage(index == -1 ? "en" : "zh");
    }

    private void Awake()
    {
        if (AdsManager.isChina)
        {
            if (Application.systemLanguage == SystemLanguage.English)
            {
                currentLanguageIndex = kEnglishIndex;
            }
            else
            {
                currentLanguageIndex = kChineseIndex;
            }
        }
        else
        {

            if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage ==SystemLanguage.ChineseSimplified || Application.systemLanguage == SystemLanguage.ChineseTraditional)
            {
                currentLanguageIndex = kChineseIndex;
            }
            else
            {
                currentLanguageIndex = kEnglishIndex;
            }
        }
    }

    public bool NeedsLoad()
    {
        return termData == null;
    }

    // Returns an error string (if any)
    //public void EnsureTranslationsLoaded(Action<string> callback)
    //{
    //    if (termData == null)
    //        ServiceLocator.instance.GetService<TranslationLoader>().Load(callback);
    //    else
    //        callback(null);
    //}

    // Deprecated
    private void SetDisplayLanguage(string selectedLanguage)
    {
        UnityEngine.Assertions.Assert.IsTrue(termData.languageIndicies.ContainsKey(selectedLanguage));
        int chosenIndex = termData.languageIndicies[selectedLanguage];
        SetDisplayLanguage(chosenIndex);
    }



    internal string Translate(string englishText, int targetLanguageIndex)
    {
        if (string.IsNullOrEmpty(englishText))
            return "";

        if (englishText == "\"\"")
            return englishText;

        if (englishText.Length < 2)
            return englishText;

        if (englishText.StartsWith("<"))
        {
            //Debug.LogWarning("Ignoring translation request for: " + englishText);
            return englishText;
        }

        //if (englishText.Contains("\n"))
            //UnityEngine.Assertions.Assert.IsTrue(true, "Term included line breaks: '" + englishText + "'");

        // Ignore translation requests that come before the termData has loaded 
        if (termData == null)
            return englishText;

        // Note: optionally compare with case insensitivity
        string englishTextLower = englishText;
        if (termData.termTranslations.ContainsKey(englishTextLower))
        {
            if (targetLanguageIndex == kEnglishIndex)
                return englishText;

            bool inBounds = termData.termTranslations[englishTextLower].Length > targetLanguageIndex && targetLanguageIndex >= 0;
            UnityEngine.Assertions.Assert.IsTrue(inBounds);
            return termData.termTranslations[englishTextLower][targetLanguageIndex];
        }
        else
        {
            //remove later
            Debug.LogWarning("No translation for: '" + englishTextLower + "'");
            AddStringToListOfTermsToTranslate(englishTextLower);
            //englishTextLower = "<<" + englishTextLower + ">>";
            return englishTextLower;
        }
    }

    internal string Translate(string englishText)
    {
        return Translate(englishText, currentLanguageIndex);
    }

    internal string GetDisplayLanguage()
    {
        foreach (var lang in termData.languageIndicies.Keys)
        {
            if (termData.languageIndicies[lang] == currentLanguageIndex)
                return lang;
        }
        Debug.LogError("Display language not found");
        return "";
    }

    void AddStringToListOfTermsToTranslate(string term)
    {
        if (term.Length < 2)
            return;

        UnityEngine.Assertions.Assert.IsFalse(termData.termTranslations.ContainsKey(term));
        string unTranslatedTermsListPath = Path.Combine(Application.persistentDataPath, kUnTranslatedTermsListPath);

        try
        {
            if (!File.Exists(unTranslatedTermsListPath))
                File.Create(unTranslatedTermsListPath);

            if (string.IsNullOrEmpty(unTranslatedTerms))
                unTranslatedTerms = File.ReadAllText(unTranslatedTermsListPath);

            if (!unTranslatedTerms.Contains(term))
            {
                unTranslatedTerms += term;
                File.AppendAllText(unTranslatedTermsListPath, term + System.Environment.NewLine);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Could not write untranslated term:" + e.Message);
        }
    }

    internal string[] SupportedLanguages()
    {
        string[] keys = new string[termData.languageIndicies.Keys.Count];
        termData.languageIndicies.Keys.CopyTo(keys, 0);
        return keys;
    }
}
