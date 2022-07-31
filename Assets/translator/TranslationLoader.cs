// TranslationLoader.cs
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

public class TranslationLoader : Singleton<TranslationLoader>
{

    private int progress = 0;
    List<string> languages = new List<string>();
    System.Action<string> callbackOnCompleted;
    private void Awake()
    {
        System.Action<string> localCallback = new System.Action<string>(delegate (string error)
        {
            Debug.Log("loaded translation");
            //if (error == null)
            //{
            //    ServiceLocator.instance.GetService<Translator>().SetDisplayLanguage(index);
            //    if (ServiceLocator.instance.GetService<Settings>().collectAllDialogueTermData)
            //        ServiceLocator.instance.GetService<TranslationLoader>().DebugCollectAllDialogueTermData();
            //}
            //else
            //{
            //    Debug.LogError("Langauge set failed: " + error);
            //}

            //if (globalCallback != null)
            //    globalCallback(error);
        });
        Load(localCallback);
    }
    // Returns an error string (if any)
    public void Load(System.Action<string> callback)
    {
        callbackOnCompleted = callback;
        AfterDownload(Resources.Load<TextAsset>("translation").text);
       // StartCoroutine(CSVDownloader.DownloadData(AfterDownload));
    }

    public void AfterDownload(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            Debug.LogError("Was not able to download data or retrieve stale data.");
            // TODO: Display a notification that this is likely due to poor internet connectivity
        }
        else
        {
            ProcessData(data, AfterProcessData);
        }
    }

    private void AfterProcessData(string errorMessage)
    {
        if (null != errorMessage)
        {
            Debug.LogError("Was not able to process data: " + errorMessage);
        }
        UnityEngine.Assertions.Assert.IsNotNull(callbackOnCompleted);
        callbackOnCompleted(errorMessage);
    }

    public void ProcessData(string data, System.Action<string> onCompleted)
    {

        Translator.Instance.termData = new TermData.Terms();

        // Line level
        int currLineIndex = 0;
        bool inQuote = false;
        int linesSinceUpdate = 0;
        int kLinesBetweenUpdate = 15;

        // Entry level
        string currEntry = "";
        int currCharIndex = 0;
        bool currEntryContainedQuote = false;
        List<string> currLineEntries = new List<string>();

        // "\r\n" means end of line and should be only occurence of '\r' (unless on macOS/iOS in which case lines ends with just \n)
        char lineEnding = '\r';// Utils.IsIOS() ? '\n' : '\r';
        int lineEndingLength = 2;  //Utils.IsIOS() ? 1 : 2;

        while (currCharIndex < data.Length)
        {
            if (!inQuote && (data[currCharIndex] == lineEnding))
            {
                // Skip the line ending
                currCharIndex += lineEndingLength;

                // Wrap up the last entry
                // If we were in a quote, trim bordering quotation marks
                if (currEntryContainedQuote)
                {
                    currEntry = currEntry.Substring(1, currEntry.Length - 2);
                }

                currLineEntries.Add(currEntry);
                currEntry = "";
                currEntryContainedQuote = false;

                // Line ended
                ProcessLineFromCSV(currLineEntries, currLineIndex);
                currLineIndex++;
                currLineEntries = new List<string>();

                linesSinceUpdate++;
                if (linesSinceUpdate > kLinesBetweenUpdate)
                {
                    linesSinceUpdate = 0;
                }
            }
            else
            {
                if (data[currCharIndex] == '"')
                {
                    inQuote = !inQuote;
                    currEntryContainedQuote = true;
                }

                // Entry level stuff
                {
                    if (data[currCharIndex] == ',')
                    {
                        if (inQuote)
                        {
                            currEntry += data[currCharIndex];
                        }
                        else
                        {
                            // If we were in a quote, trim bordering quotation marks
                            if (currEntryContainedQuote)
                            {
                                currEntry = currEntry.Substring(1, currEntry.Length - 2);
                            }

                            currLineEntries.Add(currEntry);
                            currEntry = "";
                            currEntryContainedQuote = false;
                        }
                    }
                    else if(data[currCharIndex] == '\\')
                    {
                        currEntry += '\n';
                        currCharIndex++;
                    }
                    else
                    {
                        currEntry += data[currCharIndex];
                    }
                }
                currCharIndex++;
            }

            progress = (int)((float)currCharIndex / data.Length * 100.0f);
        }

        onCompleted(null);
    }

    // Extracts translation data from a CSV line into Manager.instance.translator.termData.termTranslations 
    private void ProcessLineFromCSV(List<string> currLineElements, int currLineIndex)
    {
        Translator translator = Translator.Instance;

        // This line contains the column headers, telling us which languages are in which column
        if (currLineIndex == 0)
        {
            languages = new List<string>();
            for (int columnIndex = 0; columnIndex < currLineElements.Count; columnIndex++)
            {
                string currLanguage = currLineElements[columnIndex];
                Assert.IsTrue((columnIndex != 0 || currLanguage == "English"), "First column first row was:" + currLanguage);
                Assert.IsFalse(translator.termData.languageIndicies.ContainsKey(currLanguage));
                languages.Add(currLanguage);

                // English is given a -1 index to convey it is the key of termTranslations rather than within its array-value
                translator.termData.languageIndicies.Add(currLanguage, columnIndex - 1);
            }
            UnityEngine.Assertions.Assert.IsFalse(languages.Count == 0);
        }
        // This is a normal node
        else if (currLineElements.Count > 1)
        {
            string englishSpelling = null;
            string[] nonEnglishSpellings = new string[languages.Count - 1];

            for (int columnIndex = 0; columnIndex < currLineElements.Count; columnIndex++)
            {
                string currentTerm = currLineElements[columnIndex];
                if (columnIndex == 0)
                {
                    Assert.IsFalse(translator.termData.termTranslations.ContainsKey(currentTerm), "Saw this term twice: " + currentTerm);

                    // Note: optionally compare with case insensitivity
                    englishSpelling = currentTerm;
                }
                else
                {
                    nonEnglishSpellings[columnIndex - 1] = currentTerm;
                }
            }
            translator.termData.termTranslations[englishSpelling] = nonEnglishSpellings;
        }
        else
        {
            Debug.LogError("Database line did not fall into one of the expected categories.");
        }
    }

    //public void DebugCollectAllDialogueTermData()
    //{
    //    List<string> dialogueNames = DebugGetAllDialogueNames();
    //    foreach (string dialogue in dialogueNames)
    //    {
    //        var twineText = Resources.Load<TextAsset>("Conversations/" + dialogue);
    //        UnityEngine.Assertions.Assert.IsNotNull(twineText);
    //        var curDialogue = new DialogueObject.Dialogue(twineText.text);
    //        curDialogue.DebugTranslateAllNodes();
    //    }
    //}

    //private List<string> DebugGetAllDialogueNames()
    //{
    //    List<string> names = new List<string>();

    //    string assetsFolder = Application.dataPath;
    //    DirectoryInfo dir = new DirectoryInfo(assetsFolder + "/Resources/Conversations/");
    //    FileInfo[] info = dir.GetFiles("*.txt");
    //    foreach (FileInfo f in info)
    //    {
    //        names.Add(f.Name.Substring(0, f.Name.IndexOf(".")));
    //    }
    //    return names;
    //}
}
