using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MEC;
using Firebase;
using Firebase.Database;
using System.IO;

public enum PColor
{
    red,
    green,
    blue,
    white,
    black
}

public class PlayerController : MonoBehaviour
{
    [Header("UI Controller:")]
    public UiController controller;
    public SkinsInfo sInfo;
    [Space]

    #region Private data:

    private bool onMainPlatform;    //Set to true when the player is on the main platform... ofc xD
    [HideInInspector] public Transform Target;
    public Transform prevTarget;    //Store the previous target transform!
    [HideInInspector] public bool canTap = true;
    [HideInInspector] public Platform platform;
    [HideInInspector] public int points;
    int levelProgression;           //This is incremented as you progress through level (starts from 0 and goes to maxLevelProgression)
    int maxLevelProgression;        //The number of platforms that will spawn before the level is cleared/complete! >:)
    int hiddenMaxLevel = 5;         //All levels after this one will repeat the maxLevelProgression count! => if too short, just increase it! >:D
    Transform startPos;
    Quaternion startRot;
    Rigidbody rb;

    #endregion


    #region General data:

    [Header("Main Data:")]
    public PColor pColor;   //Check if it matches with the selected platform!!!
    [Space]
    public Platform mainPlat;

    [Header("Canvas Info:")]
    public Text riddleTxt;  //This is the main gameplay feature! >:D

    [Header("Player Movement Speed:")]
    public float movementSpeed = 0.023f;

    [Header("Camera:")]
    public Camera cam;

    #endregion



    #region Start Level:


    private FirebaseDatabase databaseReference;
    private const string pingAddress = "8.8.8.8";
    private Ping ping;

    private void Start()
    {
        databaseReference = FirebaseDatabase.GetInstance("https://riddle-cube-climber-default-rtdb.firebaseio.com/");

        rb = GetComponent<Rigidbody>();

        prevTarget = mainPlat.transform;
        startRot = transform.rotation;

        SaveData.instance.LoadGame();


        ReadDatabaseData();
    }

    #region Read Data From Database

    public void ReadDatabaseData()
    {
        ping = new Ping(pingAddress);

        if (ping == null)
        {
            LoadLocalData();
            return;
        }

        RedData();
        GreenData();
        BlueData();
        WhiteData();
        BlackData();
    }


    public void RedData()
    {
        if (ping == null)
        {
            if (File.Exists(Application.persistentDataPath + "/riddle.txt"))
            {
                string saveString = File.ReadAllText(Application.persistentDataPath + "/riddle.txt");

                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                redRiddle = saveObject.redRiddleSave;
                redAnswer = saveObject.redAnswerSave;
            }
            return;
        }

        databaseReference.GetReference("riddles/red/").GetValueAsync().ContinueWith(task => {

            if (task.IsFaulted)
            {
                return;
            }

            if (task.IsCanceled)
            {
                return;
            }

            if (task.IsCompleted)
            {
                redRiddle.Clear();
                redAnswer.Clear();

                DataSnapshot snapshot = task.Result;

                foreach (var red in snapshot.Children)
                {
                    string t = red.GetRawJsonValue();

                    RiddleData r = JsonUtility.FromJson<RiddleData>(t);

                    if (r.isActive == "true")
                    {
                        redRiddle.Add(r.question);
                        redAnswer.Add(r.answer);
                    }
                }
            }
        });
    }
    public void GreenData()
    {
        if (ping == null)
        {
            if (File.Exists(Application.persistentDataPath + "/riddle.txt"))
            {
                string saveString = File.ReadAllText(Application.persistentDataPath + "/riddle.txt");

                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                greenRiddle = saveObject.greenRiddleSave;
                greenAnswer = saveObject.greenAnswerSave;
            }
            return;
        }

        databaseReference.GetReference("riddles/green/").GetValueAsync().ContinueWith(task => {

            if (task.IsFaulted)
            {
                return;
            }

            if (task.IsCanceled)
            {
                return;
            }

            if (task.IsCompleted)
            {
                greenRiddle.Clear();
                greenAnswer.Clear();

                DataSnapshot snapshot = task.Result;

                foreach (var green in snapshot.Children)
                {
                    string t = green.GetRawJsonValue();

                    RiddleData r = JsonUtility.FromJson<RiddleData>(t);

                    if (r.isActive == "true")
                    {
                        greenRiddle.Add(r.question);
                        greenAnswer.Add(r.answer);
                    }
                }
            }
        });
    }
    public void BlueData()
    {
        if (ping == null)
        {
            if (File.Exists(Application.persistentDataPath + "/riddle.txt"))
            {
                string saveString = File.ReadAllText(Application.persistentDataPath + "/riddle.txt");

                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                blueRiddle = saveObject.blueRiddleSave;
                blueAnswer = saveObject.blueAnswerSave;
            }
            return;
        }

        databaseReference.GetReference("riddles/blue/").GetValueAsync().ContinueWith(task => {

            if (task.IsFaulted)
            {
                return;
            }

            if (task.IsCanceled)
            {
                return;
            }

            if (task.IsCompleted)
            {
                blueRiddle.Clear();
                blueAnswer.Clear();

                DataSnapshot snapshot = task.Result;

                foreach (var blue in snapshot.Children)
                {
                    string t = blue.GetRawJsonValue();

                    RiddleData r = JsonUtility.FromJson<RiddleData>(t);

                    if (r.isActive == "true")
                    {
                        blueRiddle.Add(r.question);
                        blueAnswer.Add(r.answer);
                    }
                }
            }
        });
    }
    public void WhiteData()
    {
        if (ping == null)
        {
            if (File.Exists(Application.persistentDataPath + "/riddle.txt"))
            {
                string saveString = File.ReadAllText(Application.persistentDataPath + "/riddle.txt");

                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                whiteRiddle = saveObject.whiteRiddleSave;
                whiteAnswer = saveObject.whiteAnswerSave;
            }
            return;
        }

        databaseReference.GetReference("riddles/white/").GetValueAsync().ContinueWith(task => {

            if (task.IsFaulted)
            {
                return;
            }

            if (task.IsCanceled)
            {
                return;
            }


            if (task.IsCompleted)
            {
                whiteRiddle.Clear();
                whiteAnswer.Clear();

                DataSnapshot snapshot = task.Result;

                foreach (var white in snapshot.Children)
                {
                    string t = white.GetRawJsonValue();

                    RiddleData r = JsonUtility.FromJson<RiddleData>(t);

                    if (r.isActive == "true")
                    {
                        whiteRiddle.Add(r.question);
                        whiteAnswer.Add(r.answer);
                    }
                }
            }
        });
    }
    public void BlackData()
    {
        if (ping == null)
        {
            if (File.Exists(Application.persistentDataPath + "/riddle.txt"))
            {
                string saveString = File.ReadAllText(Application.persistentDataPath + "/riddle.txt");

                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                blackRiddle = saveObject.blackRiddleSave;
                blackAnswer = saveObject.blackAnswerSave;
            }
            return;
        }

        databaseReference.GetReference("riddles/black/").GetValueAsync().ContinueWith(task => {

            if (task.IsFaulted)
            {
                return;
            }

            if (task.IsCanceled)
            {
                return;
            }

            if (task.IsCompleted)
            {
                blackRiddle.Clear();
                blackAnswer.Clear();

                DataSnapshot snapshot = task.Result;

                foreach (var black in snapshot.Children)
                {
                    string t = black.GetRawJsonValue();

                    RiddleData r = JsonUtility.FromJson<RiddleData>(t);

                    if (r.isActive == "true")
                    {
                        blackRiddle.Add(r.question);
                        blackAnswer.Add(r.answer);
                    }
                }
            }
        });
    }

    #region Save and Load:
    public void SaveLocalData()
    {
        if(ping != null)
        {
            ReadDatabaseData();
        }

        SaveObject saveObject = new SaveObject
        {
            redRiddleSave = redRiddle,
            greenRiddleSave = greenRiddle,
            blueRiddleSave = blueRiddle,
            whiteRiddleSave = whiteRiddle,
            blackRiddleSave = blackRiddle,

            redAnswerSave = redAnswer,
            greenAnswerSave = greenAnswer,
            blueAnswerSave = blueAnswer,
            whiteAnswerSave = whiteAnswer,
            blackAnswerSave = blackAnswer
        };
        string json = JsonUtility.ToJson(saveObject);

        File.WriteAllText(Application.persistentDataPath + "/riddle.txt", json);
    }


    public void LoadLocalData()
    {
        if (File.Exists(Application.persistentDataPath + "/riddle.txt"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/riddle.txt");

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            
            redRiddle = saveObject.redRiddleSave;
            greenRiddle = saveObject.greenRiddleSave;
            blueRiddle = saveObject.blueRiddleSave;
            whiteRiddle = saveObject.whiteRiddleSave;
            blackRiddle = saveObject.blackRiddleSave;

            redAnswer = saveObject.redAnswerSave;
            greenAnswer = saveObject.greenAnswerSave;
            blueAnswer = saveObject.blueAnswerSave;
            whiteAnswer = saveObject.whiteAnswerSave;
            blackAnswer = saveObject.blackAnswerSave;
        }
    }

    #endregion

    private class SaveObject
    {
        public List<string> redRiddleSave = new List<string>();
        public List<string> greenRiddleSave = new List<string>();
        public List<string> blueRiddleSave = new List<string>();
        public List<string> whiteRiddleSave = new List<string>();
        public List<string> blackRiddleSave = new List<string>();


        public List<string> redAnswerSave = new List<string>();
        public List<string> greenAnswerSave = new List<string>();
        public List<string> blueAnswerSave = new List<string>();
        public List<string> whiteAnswerSave = new List<string>();
        public List<string> blackAnswerSave = new List<string>();
    }

    private void OnApplicationQuit()
    {
        SaveLocalData();    //Riddles are never going to repeat...
    }

    #endregion
    private bool canStart = true;
    public void StartLevel()
    {
        if (SaveData.instance.level <= hiddenMaxLevel)
            maxLevelProgression = 2 + SaveData.instance.level;    //Level 0 has 2 instances of progression!
        else
            maxLevelProgression = hiddenMaxLevel; //No one wants to play an endless level :D

        //Start the match...
        if (canStart)
        {
            canStart = false;
            riddleTxt.text = "Tap the white platform!";

            controller.lvlTxt.text = SaveData.instance.level.ToString();
            controller.nextLvlTxt.text = (SaveData.instance.level + 1).ToString();

            prevTarget.GetComponent<Platform>().NewLevel();
        }
    }

    #endregion

    #region Movement:

    private float delay;
    private void Update()
    {
        //Touch inputs... => tap the platform to move the player... seems simple enough : |
        #region Mobile:

        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began) && canTap)
        {
            Ray raycast = cam.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit rayHit;
            if (Physics.Raycast(raycast, out rayHit))
            {
                if (rayHit.transform.GetComponent<Platform>() != null)
                {
                    if (!rayHit.transform.GetComponent<Platform>().grayPlatform)
                    {
                        canTap = false;
                        delay = Time.time + .05f;
                        Target = rayHit.transform.gameObject.transform;
                        Timing.RunCoroutine(_Bezier().CancelWith(gameObject));   //This will move the character
                    }

                    //if (!rayHit.transform.GetComponent<RiddleCraft>())
                    //{
                    //    canTap = false;
                    //    //Turn on the get set colors ui... this is just for show anyway XD
                    //}
                }
            }            
        }

        #endregion

        #region pc:

        //the pc input => this should be remvoed before building the project (or commented out at least) : )
        if (Input.GetMouseButtonDown(0) && canTap)
        {
            Ray clickcast = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(clickcast, out RaycastHit hit))
            {
                if (hit.transform.GetComponent<Platform>() != null)
                {
                    if (!hit.transform.GetComponent<Platform>().grayPlatform)
                    {
                        canTap = false;
                        delay = Time.time + .05f;
                        Target = hit.transform.gameObject.transform;
                        Timing.RunCoroutine(_Bezier().CancelWith(gameObject));   //this will move the character
                    }

                    //if (!hit.transform.GetComponent<RiddleCraft>())
                    //{
                    //    canTap = false;
                    //    //Turn on the get set colors ui
                    //}
                }
            }
        }

        #endregion

        if (!canTap && delay < Time.time)
        {
            //Touch inputs... => tap the platform to move the player... seems simple enough : |
            #region Mobile:

            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Ray raycast = cam.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit rayHit;
                if (Physics.Raycast(raycast, out rayHit))
                {
                    if (rayHit.transform.GetComponent<Platform>() != null)
                    {
                        if (!rayHit.transform.GetComponent<Platform>().grayPlatform)
                        {
                            SkipAhead();
                        }
                    }
                }
            }

            #endregion

            #region pc:

            //the pc input => this should be remvoed before building the project (or commented out at least) : )
            if (Input.GetMouseButtonDown(0))
            {
                Ray clickcast = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(clickcast, out RaycastHit hit))
                {
                    if (hit.transform.GetComponent<Platform>() != null)
                    {
                        if (!hit.transform.GetComponent<Platform>().grayPlatform)
                        {
                            SkipAhead();
                        }
                    }
                }
            }

            #endregion
        }
    }

    Vector3 stepPosition;
    private bool skip;

    [Header("Skip Button:")]
    public GameObject SkipBtn;

    public void SkipAhead()
    {
        skip = true;
    }

    IEnumerator<float> _Bezier()
    {
        GetComponent<AudioSource>().Play();
        SkipBtn.SetActive(true);

        if (showSkinBtns)
            HideSkinBtns();

        startPos = transform;

        float t = 0; //Ranges from [0 to 1]
        rb.useGravity = false;

        while (t < 1)
        {
            if (skip)
                break;

            t += movementSpeed;
            if (t > 1)
                t = 1;

            stepPosition =
                Mathf.Pow(1 - t, 3) * startPos.position +
                3 * t * Mathf.Pow(1 - t, 2) * (new Vector3(startPos.position.x, startPos.position.y + 1f, startPos.position.z)) +
                3 * Mathf.Pow(t, 2) * (1 - t) * (new Vector3(Target.position.x, Target.position.y + 1f, Target.position.z)) +
                Mathf.Pow(t, 3) * Target.position;

            transform.position = stepPosition;  //Move!!! >: D
            yield return 0;
        }
        rb.useGravity = true;
        transform.position = Target.position;

        SkipBtn.SetActive(false);
        if (skip)
        {
            skip = false;
            transform.position = Target.position;
        }


        if (Target.GetComponent<Platform>().pColor == pColor)
        {

            levelProgression++;
            Target.GetComponent<Platform>().SetGrayMat();
            Target.transform.parent = transform.parent;

            prevTarget.GetComponent<Platform>().ClearPlatformsPTS();
            prevTarget = Target;

            if (!prevTarget.GetComponent<Platform>().isMainPlatform)
                controller.SetProgressImg((float)levelProgression / (float)maxLevelProgression);
                

            if (levelProgression < maxLevelProgression)
                Timing.RunCoroutine(_CallSummonPlatforms().CancelWith(gameObject));
            else
            {
                TinySauce.OnGameFinished(levelNumber: "Level_" + SaveData.instance.level, true, SaveData.instance.level);   //No score though...

                SaveData.instance.level++;
                CheckAreaId();
                levelProgression = 0;

                Timing.RunCoroutine(_CallEndLevel().CancelWith(gameObject));
                Target.GetComponent<Platform>().NewLevel();
            }
        } else {
            Respawn();  //Test... | Try to destroy all the platforms except the prevTarget and call the respawn at the end (with slight time delay)
            Fail();

            health--;
            SetHealth(health);
        }
    }

    public void CheckAreaId()
    {
        switch (SaveData.instance.level)
        {
            //Maps:
            case 1:
                sInfo.MapUnlocked();    //Summon an image of unlocked area to the screen! - Desert
                SaveData.instance.areaId++;
                break;

            case 5:
                sInfo.MapUnlocked();    //Summon an image of unlocked area to the screen! - Forest
                SaveData.instance.areaId++;
                break;

            case 9:
                sInfo.MapUnlocked();    //Summon an image of unlocked area to the screen! - Savanna
                SaveData.instance.areaId++;
                break;

                //Platforms:
            case 2:
                SaveData.instance.platformId++;
                sInfo.PlatformUnlocked();    //Summon an image of unlocked area to the screen! - Desert
                break;

            case 6:
                SaveData.instance.platformId++;
                sInfo.PlatformUnlocked();    //Summon an image of unlocked area to the screen! - Desert
                break;

            //Skins:
            case 3:
                SaveData.instance.playerSkinId++;
                sInfo.PlayerSkinUnlocked();    //Summon an image of unlocked area to the screen! - Desert
                break;

            case 7:
                SaveData.instance.playerSkinId++;
                sInfo.PlayerSkinUnlocked();    //Summon an image of unlocked area to the screen! - Desert
                break;

            case 10:
                SaveData.instance.playerSkinId++;
                sInfo.PlayerSkinUnlocked();    //Summon an image of unlocked area to the screen! - Desert
                break;

            //Crafting:
            case 4:
                sInfo.CraftingUnlocked();
                break;
        }
    }

    IEnumerator<float> _CallSummonPlatforms()
    {
        cam.GetComponent<CameraController>().target = Target;


        if (!Target.GetComponent<Platform>().isMainPlatform)
        {
            switch (riddletype)
            {
                case 0:
                    riddleTxt.text = redAnswer[riddleID];

                    redAnswer.RemoveAt(riddleID);
                    redRiddle.RemoveAt(riddleID);

                    if (redRiddle.Count <= 0)
                    {
                        RedData();
                    }
                    break;

                case 1:
                    riddleTxt.text = greenAnswer[riddleID];
                    
                    greenAnswer.RemoveAt(riddleID);
                    greenRiddle.RemoveAt(riddleID);

                    if (greenRiddle.Count <= 0)
                    {
                        GreenData();
                    }
                    break;

                case 2:
                    riddleTxt.text = blueAnswer[riddleID];

                    blueAnswer.RemoveAt(riddleID);
                    blueRiddle.RemoveAt(riddleID);

                    if (blueRiddle.Count <= 0)
                    {
                        BlueData();
                    }
                    break;

                case 3:
                    riddleTxt.text = whiteAnswer[riddleID];

                    whiteAnswer.RemoveAt(riddleID);
                    whiteRiddle.RemoveAt(riddleID);

                    if (whiteRiddle.Count <= 0)
                    {
                        WhiteData();
                    }
                    break;

                case 4:
                    riddleTxt.text = blackAnswer[riddleID];

                    blackAnswer.RemoveAt(riddleID);
                    blackRiddle.RemoveAt(riddleID);

                    if (blackRiddle.Count <= 0)
                    {
                        BlackData();
                    }
                    break;
            }
        } else {
            ClearStage();
        }


        yield return Timing.WaitForSeconds(.2f);
        cam.GetComponent<CameraController>().SetMoveCam();

        yield return Timing.WaitForSeconds(2.3f);
        prevTarget.GetComponent<Platform>().SummonPlatforms();
        canTap = true;

        GetComponent<UiController>().playerBtn.color = Color.white;
        GetComponent<UiController>().platformBtn.color = Color.white;
        GetComponent<UiController>().mapBtn.color = Color.white;
        GetComponent<UiController>().craftBtn.GetComponent<Image>().color = Color.white;
    }

    IEnumerator<float> _CallEndLevel()
    {
        cam.GetComponent<CameraController>().target = Target;

        switch (riddletype)
        {
            case 0:
                riddleTxt.text = redAnswer[riddleID];

                redAnswer.RemoveAt(riddleID);
                redRiddle.RemoveAt(riddleID);

                if (redRiddle.Count <= 0)
                {
                    RedData();
                }
                break;

            case 1:
                riddleTxt.text = greenAnswer[riddleID];

                greenAnswer.RemoveAt(riddleID);
                greenRiddle.RemoveAt(riddleID);

                if (greenRiddle.Count <= 0)
                {
                    GreenData();

                }
                break;

            case 2:
                riddleTxt.text = blueAnswer[riddleID];

                blueAnswer.RemoveAt(riddleID);
                blueRiddle.RemoveAt(riddleID);

                if (blueRiddle.Count <= 0)
                {
                    BlueData();
                }
                break;

            case 3:
                riddleTxt.text = whiteAnswer[riddleID];

                whiteAnswer.RemoveAt(riddleID);
                whiteRiddle.RemoveAt(riddleID);

                if (whiteRiddle.Count <= 0)
                {
                    WhiteData();
                }
                break;

            case 4:
                riddleTxt.text = blackAnswer[riddleID];

                blackAnswer.RemoveAt(riddleID);
                blackRiddle.RemoveAt(riddleID);

                if (blackRiddle.Count <= 0)
                {
                    BlackData();
                }
                break;
        }

        yield return Timing.WaitForSeconds(.2f);
        cam.GetComponent<CameraController>().SetMoveCam();

        yield return Timing.WaitForSeconds(2.3f);
        int lvlClearInt = Random.Range(0, 5);

        switch (lvlClearInt)
        {
            case 0:
                riddleTxt.text = "See, That wasn't so hard!";
                break;

            case 1:
                riddleTxt.text = "Wow, I was sure you were a goner!";
                break;

            case 2:
                riddleTxt.text = "I think you got lucky to get this far!";
                break;

            case 3:
                riddleTxt.text = "If you haven't already please rate the app!";
                break;

            case 4:
                riddleTxt.text = "Invite your friends to check the app!";
                break;
        }

        canTap = true;
        GetComponent<UiController>().playerBtn.color = Color.white;
        GetComponent<UiController>().platformBtn.color = Color.white;
        GetComponent<UiController>().mapBtn.color = Color.white;
        GetComponent<UiController>().craftBtn.GetComponent<Image>().color = Color.white;
    }

    public void Respawn()
    {
        canTap = false;
        rb.AddForce(transform.up * 3f, ForceMode.Impulse);
        rb.AddForce(-transform.forward * 1.5f, ForceMode.Impulse);
        rb.AddTorque(-transform.right * 3f, ForceMode.Impulse);

        GetComponent<UiController>().playerBtn.color = Color.gray;
        GetComponent<UiController>().platformBtn.color = Color.gray;
        GetComponent<UiController>().mapBtn.color = Color.gray;
        GetComponent<UiController>().craftBtn.GetComponent<Image>().color = Color.gray;

        //Respawn the character...
        prevTarget.GetComponent<Platform>().ClearPlatformsNoPTS();
        Timing.RunCoroutine(_ResTime().CancelWith(gameObject));
    }

    [Header("Poof:")]
    public GameObject poof;

    IEnumerator<float> _ResTime()
    {
        yield return Timing.WaitForSeconds(2.4f);
        Instantiate(poof, prevTarget.position + new Vector3(0, 3, 0), poof.transform.rotation); //This is the poof effect! :O

        yield return Timing.WaitForSeconds(.1f);
        transform.position = prevTarget.position + new Vector3(0, 3, 0);
        rb.angularVelocity = Vector3.zero;  //This is important (breaks the game feel if removed!!!)
        rb.velocity = Vector3.zero;         //Seems like this breaks the game as well xD
        transform.rotation = startRot;



        yield return Timing.WaitForSeconds(.5f);
        prevTarget.GetComponent<Platform>().SummonPlatforms();
        canTap = true;  //This shouldn't be true right away!!!

        GetComponent<UiController>().playerBtn.color = Color.white;
        GetComponent<UiController>().platformBtn.color = Color.white;
        GetComponent<UiController>().mapBtn.color = Color.white;
        GetComponent<UiController>().craftBtn.GetComponent<Image>().color = Color.white;
    }

    [Header("Health:")]
    public float health = 3.5f;    //The player only has 3 hp... and gets 1 bonus hp by watching an ad!!! C:<
    private float constHp = 3;  //Same as health

    void SetHealth(float hp)
    {
        if (hp > 0.5f)
            transform.localScale = new Vector3(hp / constHp, hp / constHp, hp / constHp);
        else
        {
            TinySauce.OnGameFinished(levelNumber: "Level_" + SaveData.instance.level, false, SaveData.instance.level);   //No score though...
            Timing.RunCoroutine(_DeleteMe().CancelWith(gameObject));   //We'll remove this later!
        }
    }

    #region Delete this later:

    IEnumerator<float> _DeleteMe()
    {
        yield return Timing.WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(0); //This will be moved to another function!!! Because we need reward ads... remember? >:D
    }

    #endregion

    #endregion

    #region Riddle data:

    [Header("Riddles:")]
    public List<string> redRiddle = new List<string>(); //Test... YOLO! >XD
    public List<string> greenRiddle = new List<string>();
    public List<string> blueRiddle = new List<string>();
    public List<string> whiteRiddle = new List<string>();
    public List<string> blackRiddle = new List<string>();


    [Header("Answers:")]
    public List<string> redAnswer = new List<string>();
    public List<string> greenAnswer = new List<string>();
    public List<string> blueAnswer = new List<string>();
    public List<string> whiteAnswer = new List<string>();
    public List<string> blackAnswer = new List<string>();

    [Header("Fails:")]
    public string[] fail;

    [Header("Level Clear:")]
    public string[] lvlClear;

    public int riddletype;  //0 = red, 1 = green, ...and so on
    public int riddleID;    //The selected riddle => example: redRiddle[0] => the answer of this riddle would be redAnswer[0] C:<

    public void Riddle()
    {
        switch (riddletype)
        {
            case 0:
                riddleID = Random.Range(0, redRiddle.Count);    //YOLO!!! >:O
                riddleTxt.text = redRiddle[riddleID];
                
                break;

            case 1:
                riddleID = Random.Range(0, greenRiddle.Count);
                riddleTxt.text = greenRiddle[riddleID];
                
                break;

            case 2:
                riddleID = Random.Range(0, blueRiddle.Count);
                riddleTxt.text = blueRiddle[riddleID];
                
                break;

            case 3:
                riddleID = Random.Range(0, whiteRiddle.Count);
                riddleTxt.text = whiteRiddle[riddleID];
                
                break;

            case 4:
                riddleID = Random.Range(0, blackRiddle.Count);
                riddleTxt.text = blackRiddle[riddleID];
                
                break;
        }
    }

    public void RemoveWhiteZero()
    {
        whiteRiddle.RemoveAt(0);
        whiteAnswer.RemoveAt(0);
    }

    [Header("Confetti:")]
    public GameObject confetti;

    [Header("Skin Buttons:")]
    public GameObject PlayerSkinBtn;
    public GameObject PlatfromSkinBtn;
    public GameObject mapBtn;
    private bool showSkinBtns;
    public void ClearStage()
    {
        Instantiate(confetti, transform.position + new Vector3(0, 1, 0), confetti.transform.rotation);

        int lvlRandInt = Random.Range(0, lvlClear.Length);
        riddleTxt.text = lvlClear[lvlRandInt];

        GetComponentInParent<World>()._SetWorld();

        controller.SetProgressImg(0);
        controller.lvlTxt.text = SaveData.instance.level.ToString();
        controller.nextLvlTxt.text = (SaveData.instance.level + 1).ToString();
        controller.nextLvlTxt.GetComponentInParent<Image>().color = Color.white;

        health = constHp + .5f;
        SetHealth(constHp);
        GetComponent<AudioSource>().Play();

        #region Call Skin Buttons:

        showSkinBtns = true;
        if(SaveData.instance.level >= 3)
            PlayerSkinBtn.SetActive(showSkinBtns);

        if (SaveData.instance.level >= 2)
            PlatfromSkinBtn.SetActive(showSkinBtns);

        if (SaveData.instance.level >= 1)
            mapBtn.SetActive(showSkinBtns);

        if (SaveData.instance.level >= 4)
            GetComponent<UiController>().craftBtn.SetActive(showSkinBtns);

        GetComponent<UiController>().playerBtn.color = Color.gray;
        GetComponent<UiController>().platformBtn.color = Color.gray;
        GetComponent<UiController>().mapBtn.color = Color.gray;
        GetComponent<UiController>().craftBtn.GetComponent<Image>().color = Color.gray;

        GetComponent<UiController>().playerBtn.sprite = GetComponent<SkinsInfo>().playerSkins[SaveData.instance.selectedSkin];
        GetComponent<UiController>().platformBtn.sprite = GetComponent<SkinsInfo>().platformSkins[SaveData.instance.selectedPlatform];

        #endregion

        SaveData.instance.SaveGame();   //Save when you pass the level! :\
    }

    public void HideSkinBtns()
    {
        showSkinBtns = false;
        PlayerSkinBtn.SetActive(showSkinBtns);
        PlatfromSkinBtn.SetActive(showSkinBtns);
        mapBtn.SetActive(showSkinBtns);
        GetComponent<UiController>().craftBtn.SetActive(showSkinBtns);
    }

    public void Fail()
    {
        riddleID = Random.Range(0, fail.Length);
        riddleTxt.text = fail[riddleID];
    }

    #endregion


}
