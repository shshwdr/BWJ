using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



[System.Serializable]
public class SerializedGame : CSSerializedObject
{
    //public int progress = 0;
    //public SerializedVector playerPosition;
    //public bool isPlayerUnderground;
    //public SerializedDictionary<string, bool> plantUnlocked;
    //public SerializedDictionary<string, bool> plantHinted;
    //public SerializedDictionary<string, bool> isTriggered;
    //public bool isTutorialHidden;
    //public float musicParam;
    //public bool isGameFinished;
    public List<string> maxTutorial;
    public List<int> levelStars;
    public List<bool> unlockedHint;
    public int maxUnlockedLevel;
    public int totalCollected;
    public int languageIndex;
    public bool hasEverCollected;
}
public class SaveLoadManager : Singleton<SaveLoadManager>
{
    static int currentVersion = 0;
    PlayerMovement player;
    bool isGameStarted = false;
    public static void saveGame()
    {

        // 1
        // create save game
        SerializedGame save = new SerializedGame();
        save.version = currentVersion;
        StageLevelManager.Instance.Save(save);

        SaveLoadUtil.Save(save, Application.persistentDataPath + "/gamesave.save");
    }


    //SerializedGame getCurrentSave()
    //{

    //    if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
    //    {
    //        FileStream file = null;
    //        try
    //        {

    //            BinaryFormatter bf = new BinaryFormatter();
    //            file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    //            SerializedGame save = (SerializedGame)bf.Deserialize(file);
    //            return save;
    //        }
    //        catch (System.Exception e)
    //        {
    //            Debug.LogError(e);

    //        }
    //        finally
    //        {

    //            file.Close();
    //        }
    //    }
    //    return new SerializedGame();
    //}

    public static void LoadGame()
    {
        var save = SaveLoadUtil.Load(Application.persistentDataPath + "/gamesave.save");
        if(save!=null)
        {
            StageLevelManager.Instance.Load((SerializedGame)save);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)){
            saveGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
        LoadGame();
        isGameStarted = true;
    }


    public static void clearSavedData()
    {
        SaveLoadUtil.clearSavedData(Application.persistentDataPath + "/gamesave.save");
    }

    public static bool hasSavedData()
    {
        return SaveLoadUtil.hasSavedData(Application.persistentDataPath + "/gamesave.save");
    }


    bool savingbeforequit = false;

    //private void OnDestroy()
    //{
    //    if (isGameStarted)
    //    // if (!savingbeforequit)if (!isGameStarted)
    //    {
    //        savingbeforequit = true;

    //        saveGame();

    //        Debug.Log("OnDestroy executed");
    //    }
    //}

    private void OnApplicationQuit()
    {
        if (isGameStarted)
        //if (!savingbeforequit)
        {
            savingbeforequit = true;

            saveGame();

            Debug.Log("OnApplicationQuit executed");
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (isGameStarted)
        //if (!savingbeforequit)
        {
            savingbeforequit = true;

            saveGame();

            Debug.Log("OnApplicationPause executed");
        }
    }

    void exitApp()
    {
        if (isGameStarted)
        // if (!savingbeforequit)
        {
            savingbeforequit = true;

            saveGame();

            Debug.Log("exit button");
        }
        Application.Quit();
    }
}
