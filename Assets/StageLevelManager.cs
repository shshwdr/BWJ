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
    public bool unlockedHint = false;
    public bool hasBeenFinished = false;
}
public class StageLevelManager : Singleton<StageLevelManager>
{

    public bool playHintNext = false;
    public bool showDialogue = true;
    public int currentLevelId;
    public int maxUnlockedLevel = 0;
    public int currentCollected = 0;


    bool isFinished;
    public bool isLevelFinishedValidation;
    public bool hasEverCollected = false;
    public int starCountInTotal;

    public int totalCollected = 0;
    public int totalCanCollect { get { return 25; } }


    public void Save(SerializedGame save)
    {
        save.maxTutorial = TutorialManager.Instance.unlockedTutorialList;
        List<int> levelStar = new List<int>();
        save.hasEverCollected = hasEverCollected;
        save.maxUnlockedLevel = maxUnlockedLevel;
        save.totalCollected = totalCollected;
        for (int i = 0;i< levelInfoList.Count; i++)
        {
            levelStar.Add(levelInfoList[i].collectedCount);
        }
        save.levelStars = levelStar;

        List<bool> unlockedHint = new List<bool>(); 
        for (int i = 0; i < levelInfoList.Count; i++)
        {
            unlockedHint.Add(levelInfoList[i].unlockedHint);
        }
        save.unlockedHint = unlockedHint;
        save.languageIndex = Translator.Instance.GetUserDisplayLanguageIndex();
        //base.Save(save);
        //save.progress = progress;
        //save.plantUnlocked = plantUnlocked;
        //save.plantHinted = plantHinted;
    }
    public void Load(SerializedGame save)
    {
        if (save.maxTutorial == null || save.maxTutorial.Count == 0)
        {
            Debug.LogError("tutorial not existed");
            return;
        }
        if (save.levelStars.Count< save.maxUnlockedLevel)
        {
            Debug.LogError("level stars missing");
            return;
        }
        TutorialManager.Instance.unlockedTutorialList = save.maxTutorial;
        
        for (int i = 0; i < save.levelStars.Count; i++)
        {
            levelInfoList[i].collectedCount =save.levelStars[i];
            levelInfoList[i].unlockedHint = save.unlockedHint[i];
        }
        hasEverCollected = save.hasEverCollected;
        maxUnlockedLevel = Mathf.Min(levelInfoList.Count -1, save.maxUnlockedLevel);

        totalCollected = save.totalCollected;
        Translator.Instance.SetDisplayLanguage(save.languageIndex);
        //base.Load(save);
        //progress = save.progress;

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
        return currentCollected >= currentLevel.itemCount;
    }
    public bool collectedAll()
    {
        return totalCollected >= totalCanCollect;
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


    public LevelInfo currentLevel { get { if (currentLevelId >= 0 && currentLevelId < levelInfoList.Count) return levelInfoList[currentLevelId]; return null; } }
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
    public void clear()
    {

        TutorialManager.Instance.unlockedTutorialList = new List<string>();

        for (int i = 0; i < levelInfoList.Count; i++)
        {
            levelInfoList[i].collectedCount = 0;
        }
        hasEverCollected = false;
        maxUnlockedLevel = Mathf.Min(levelInfoList.Count - 1, 0);

        totalCollected = 0;
    }
    public void restart()
    {
        startNextLevel();
    }
    public void unlockNextLevel()
    {

        maxUnlockedLevel = Mathf.Min(levelInfoList.Count-1, Mathf.Max(currentLevelId + 1, maxUnlockedLevel));
    }
    public void addLevel()
    {
        currentLevelId++;
        maxUnlockedLevel = Mathf.Min(levelInfoList.Count - 1, Mathf.Max(currentLevelId, maxUnlockedLevel));
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

    public void setCollectable(int value) {

        currentCollected = value;
    }

    public void addCollectable()
    {
        currentCollected++;
        if (GameObject.FindObjectOfType<PlayerCubeGridMove>().startedMoving)
        {
            EventPool.Trigger("updateCollected");
            //if (!hasEverCollected)
            {
                hasEverCollected = true;
                EventPool.Trigger("firstCollect");
            }

            if (currentCollected == currentLevel.collectedCount)
            {

                FMODUnity.RuntimeManager.PlayOneShot("event:/collect all maybe");
            }
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
        isLevelFinishedValidation = true;
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
            //EventPool.Trigger("updateTotalCollected");
        }

        StartCoroutine(finishLevelReal());

        SaveLoadManager.saveGame();
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
        }
        else
        {
            if(currentLevel.itemCount != GameObject.FindGameObjectsWithTag("human").Length)
            {
                Debug.LogError("collectable item count wrong");
            }

        }

        if (GameObject.FindGameObjectsWithTag("target").Length != 1)
        {
            Debug.LogError("target item count wrong");

        }

        if (!currentLevel.hasBeenFinished)
        {
            currentLevel.hasBeenFinished = true;
            //totalCanCollect += currentLevel.itemCount;
        }

        if (showDialogue)
        {
            DialogueManager.StartConversation($"{currentLevel.id}_start");
        }
    }
    public void setToLatestLevel()
    {
        currentLevelId = maxUnlockedLevel;
    }
    public void startNextLevel(bool reload = true)
    {
        isFinished = false;
        currentCollected = 0;
        Time.timeScale = 1;

        //if (!GetComponent<AudioSource>().isPlaying)
        //{
        //    GetComponent<AudioSource>().Play();
        //}

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
        if (reload)
        {
            SceneManager.LoadScene(currentLevel.sceneName);
        }


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
        //startNextLevel();

    }

    public void unlockHint()
    {
        
        levelInfoList[currentLevelId].unlockedHint = true;
        playHint();
    }

    public void playHint()
    {

        //if already started
        if (GameObject.FindObjectOfType<PlayerCubeGridMove>().startedMoving)
        {
            playHintNext = true;
            restart();
        }
        else
        {

            GameObject.FindObjectOfType<PlayerCubeGridMove>().isAuto = true;
        }
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
