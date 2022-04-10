using Cinemachine;
using PixelCrushers.DialogueSystem;
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
    public int collectedCount = 0;
    public bool hasBeenFinished = false;
}
public class StageLevelManager : Singleton<StageLevelManager>
{

    public bool showDialogue = true;
    public int currentLevelId;
    public int maxUnlockedLevel = 0;
    public int currentCollected = 0;

    bool isFinished;
    public bool hasEverCollected = false;
    public int starCountInTotal;

    public int totalCollected = 0;
    public int totalCanCollect = 0;


    public void Save(SerializedGame save)
    {
        save.maxTutorial = TutorialManager.Instance.unlockedTutorialList;
        List<int> levelStar = new List<int>();
        for(int i = 0;i< save.maxUnlockedLevel; i++)
        {
            levelStar.Add(levelInfoList[i].collectedCount);
        }
        save.levelStars = levelStar;
        save.hasEverCollected = hasEverCollected;
        save.maxUnlockedLevel = maxUnlockedLevel;
        //base.Save(save);
        //save.progress = progress;
        //save.plantUnlocked = plantUnlocked;
        //save.plantHinted = plantHinted;
    }
    public void Load(SerializedGame save)
    {
        TutorialManager.Instance.unlockedTutorialList = save.maxTutorial;
        for (int i = 0; i < save.maxUnlockedLevel; i++)
        {
            levelInfoList[i].collectedCount =save.levelStars[i];
        }
        hasEverCollected = save.hasEverCollected;
        maxUnlockedLevel = save.maxUnlockedLevel;

        //base.Load(save);
        //progress = save.progress;

        //progressText.text = progress + " %";
        //plantUnlocked = save.plantUnlocked;
        //plantHinted = save.plantHinted;
    }

    // bool isHome;


    AudioSource audioSource;
    public bool isGameFinished { get { return isFinished; } }
    // public bool isInHome { get { return isHome; } }

    Dictionary<string, int> levelToStarCount = new Dictionary<string, int>();

    public bool collectedAllInLevel()
    {
        return currentCollected == currentLevel.itemCount;
    }
    public bool collectedAll()
    {
        return currentCollected == currentLevel.itemCount;
    }
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


    //public int getCollectedCount()
    //{
    //    //int i = 0;
    //    //foreach (var s in currentLevel.collectedItem)
    //    //{
    //    //    if (s)
    //    //    {
    //    //        i++;
    //    //    }
    //    //}
    //    return co;
    //}
    public void addCollectable()
    {
        currentCollected++;
        EventPool.Trigger("updateCollected");
        if (!hasEverCollected)
        {
            hasEverCollected = true;
            EventPool.Trigger("firstCollect");
        }
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
        if (currentCollected == currentLevel.itemCount)
        {
            showStarCount = 3;
        }
        else
        {
            var diff = currentLevel.itemCount;
            var expectDiff = (diff + 1) / 2;

            //Debug.Log($"stars diff {diff}, expect {expectDiff}, compare {currentLevel.targetCount - rescuedCount}");
            if (currentLevel.itemCount - currentCollected <= expectDiff)
            {
                showStarCount = 2;

            }
            else
            {
                showStarCount = 1;
            }
        }

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

         LevelSelectionView view = GameObject.FindObjectOfType<LevelSelectionView>(true);
         view.showView();
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
        unlockNextLevel();

        if (currentCollected > currentLevel.collectedCount)
        {
            starCountInTotal += (currentCollected - currentLevel.collectedCount);
            totalCollected += (currentCollected - currentLevel.collectedCount);
            currentLevel.collectedCount = currentCollected;
            EventPool.Trigger("updateTotalCollected");
        }

        StartCoroutine(finishLevelReal());
    }

    public IEnumerator finishLevelReal()
    {
        yield return new WaitForSeconds(2);
        //Time.timeScale = 0;
        RewardView view = GameObject.FindObjectOfType<RewardView>(true);
        view.showView();
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
        //if (id >= currentLevel.collectedItem.Length)
        //{
        //    Debug.LogError(id + " id is too large " + currentLevel.collectedItem.Length);
        //    return;
        //}
        //currentLevel.collectedItem[id] = true;
        currentCollected++;
        EventPool.Trigger("updateCollected");
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

    public void startLevel()
    {

        if (currentLevel.itemCount == 0)
        {
            currentLevel.itemCount = GameObject.FindGameObjectsWithTag("human").Length;
        }
        if (!currentLevel.hasBeenFinished)
        {
            currentLevel.hasBeenFinished = true;
            totalCanCollect += currentLevel.itemCount;
        }

        if (showDialogue)
        {
            DialogueManager.StartConversation($"{currentLevel.id}_start");
        }
    }

    public void startNextLevel()
    {
        isFinished = false;
        currentCollected = 0;
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


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLevelId++;
            startNextLevel();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLevelId--;
            startNextLevel();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            restart();
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale *= 2;
            if (Time.timeScale > 4)
            {
                Time.timeScale = 1;
            }
        }

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    currentLevelId++;
        //    restart();
        //}

        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    currentLevelId--;
        //    restart();
        //}
    }
}
