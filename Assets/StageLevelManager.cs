using Cinemachine;
using Pool;
using Sinbad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfo
{
    public string sceneName;
    public string displayName;
    public int id;
    public int itemCount;
    public int itemToUnlock;
    public bool[] collectedItem;
}
public class StageLevelManager : Singleton<StageLevelManager>
{
    public int currentLevelId;
    public int maxUnlockedLevel = 0;

    bool isFinished;

    // bool isHome;


    AudioSource audioSource;
    public bool isGameFinished { get { return isFinished; } }
    // public bool isInHome { get { return isHome; } }

    Dictionary<string, int> levelToStarCount = new Dictionary<string, int>();


    public int starCountInLevel(string sceneName)
    {
        if (!levelToStarCount.ContainsKey(sceneName))
        {
            Debug.LogWarning("star not generate");
            return 0;
        }
        return levelToStarCount[sceneName];
    }


    public LevelInfo currentLevel { get { if (currentLevelId >= 0) return levelInfoList[currentLevelId]; return null; } }
    public Dictionary<string, LevelInfo> levelInfoByName = new Dictionary<string, LevelInfo>();
    public List<LevelInfo> levelInfoList = new List<LevelInfo>();
    public bool hasNextLevel()
    {
        if (currentLevelId + 1 >= levelInfoList.Count)
        {
            return false;
        }
        return true;
    }

    public void restart()
    {
        startNextLevel();
    }
    public void unlockNextLevel()
    {

        maxUnlockedLevel = Mathf.Max(currentLevelId + 1, maxUnlockedLevel);
    }
    public void addLevel()
    {
        currentLevelId++;
        maxUnlockedLevel = Mathf.Max(currentLevelId, maxUnlockedLevel);
    }


    public int getCollectedCount()
    {
        int i = 0;
        foreach (var s in currentLevel.collectedItem)
        {
            if (s)
            {
                i++;
            }
        }
        return i;
    }


    public int starCount()
    {
        int showStarCount = 0;
        //if (getTargetFinish())
        //{
        //    showStarCount = 3;
        //}
        //else if (getMainTargetFinish())
        //{
        //    var diff = currentLevel.targetCount - currentLevel.mainTargetCount;
        //    var expectDiff = (diff + 1) / 2;

        //    //Debug.Log($"stars diff {diff}, expect {expectDiff}, compare {currentLevel.targetCount - rescuedCount}");
        //    if (currentLevel.targetCount - rescuedCount <= expectDiff)
        //    {
        //        showStarCount = 2;

        //    }
        //    else
        //    {
        //        showStarCount = 1;
        //    }

        //}
        //if (!levelToStarCount.ContainsKey(currentLevel.sceneName))
        //{
        //    levelToStarCount[currentLevel.sceneName] = 0;
        //}
        //if (showStarCount > levelToStarCount[currentLevel.sceneName])
        //{
        //    levelToStarCount[currentLevel.sceneName] = showStarCount;
        //}
        return showStarCount;
    }

    public void selectLevel()
    {

        // LevelSelectionView view = GameObject.FindObjectOfType<LevelSelectionView>(true);
        // view.showView();
    }
    public void finishLevel()
    {
        //first disable player move and create car

        //var player = GameObject.FindObjectOfType<PlayerController>();
        //player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //Instantiate(Resources.Load<GameObject>("rangerCar"));
        //isFinished = true;
        //foreach (var camera in GameObject.FindObjectsOfType<CinemachineVirtualCamera>())
        //{
        //    camera.Follow = null;
        //}
        //when car finished moving start real level finish
    }

    public void finishLevelReal()
    {

        //Time.timeScale = 0;
        //RewardView view = GameObject.FindObjectOfType<RewardView>(true);
        //view.showView();
    }

    //public void linkAnimal(string type)
    //{
    //    if (isGameFinished)
    //    {
    //        return;
    //    }
    //    rescuedCount++;
    //    if (type == currentLevel.mainTarget || currentLevel.mainTarget == "Animal")
    //    {
    //        rescuedMainCount++;
    //    }
    //    EventPool.Trigger("linkAnimal");
    //}

    public void collectItem(int id)
    {
        if (id >= currentLevel.collectedItem.Length)
        {
            Debug.LogError(id + " id is too large " + currentLevel.collectedItem.Length);
            return;
        }
        currentLevel.collectedItem[id] = true;
    }

    public void startLevel(int id)
    {
        currentLevelId = id;
        //startNextLevel();
        //if (isInHome)
        //{

        //    finishLevel();
        //}
        //else
        //{
            startNextLevel();
        //}
    }



    public void startNextLevel()
    {
        isFinished = false;
        Time.timeScale = 1;


        //{
        //    if (currentLevel.id < 6)
        //    {

        //        MusicManager.Instance.playLevelMusic();
        //    }
        //    else
        //    {
        //        MusicManager.Instance.playLevelMusic2();
        //    }
        //}
        LevelManager.Instance.reset();
        SceneManager.LoadScene(currentLevel.sceneName);
    }

    // Start is called before the first frame update
    void Start()
    {
        levelInfoList = CsvUtil.LoadObjects<LevelInfo>("Level");
        int id = 0;
        foreach (var info in levelInfoList)
        {
            //if (info.isDeprecated == 1)
            //{
            //    continue;
            //}
            info.id = id++;
            levelInfoByName[info.displayName] = info;
        }
        startNextLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            restart();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            currentLevelId++;
            restart();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            currentLevelId--;
            restart();
        }
    }
}
