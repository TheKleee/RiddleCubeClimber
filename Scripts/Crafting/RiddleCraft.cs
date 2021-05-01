using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using UnityEngine.Events;
using Firebase.Extensions;

public class RiddleCraft : MonoBehaviour
{
    private string riddleColor;

    [Header("Player Ref:")]
    public PlayerController player; //Set this up in PlayerController.cs (on instance)

    [Header("Fields:")]
    public InputField riddleInput;
    public Text riddleText;
    public InputField answerInput;
    public Text answerText;

    [Header("Color Img:")]
    public Image colorImg;

    [Header("Start Crafting Button Ref:")]
    public GameObject CraftBtn;

    private int colorIndex; //If 0 => white | respectively: red, green, blue, black  

    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    private Ping ping;
    private const string pingAddress = "8.8.8.8";

    

    private void Start()
    {
        riddleColor = "white";
        databaseReference = FirebaseDatabase.GetInstance(DATA_URL);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                Debug.LogError($"Failed to initialize Firebase with {task.Exception}");
                return;
            }

            OnFirebaseInitialized.Invoke();
        });
    }

    private void OnEnable()
    {
        CallCraft();
    }

    public void RegularExit()
    {
        ExitCrafting(true);
        player.Respawn();
        player.riddleTxt.text = "Feel free to try again!";
    }

    public void ExitCrafting(bool exitTap)
    {
        player.canTap = exitTap;
        //Go back to the gameplay...
        gameObject.SetActive(false);

        CraftBtn.SetActive(exitTap);


        //Restart all the data => dump data...
        colorIndex = 0;
        colorImg.color = Color.white;
        riddleColor = "white";
        riddleInput.text = "";
        answerInput.text = "";
    }

    #region Colors:

    public void ChangeColor(bool increment)
    {
        #region Arrow Mechanics:

        if (increment)
            colorIndex++;
        else
            colorIndex--;

        if (colorIndex > 4)
            colorIndex = 0;

        if (colorIndex < 0)
            colorIndex = 4;

        #endregion

        switch (colorIndex)
        {
            case 0:
                colorImg.color = Color.white;
                riddleColor = "white";
                break;

            case 1:
                colorImg.color = Color.red;
                riddleColor = "red";
                break;

            case 2:
                colorImg.color = Color.green;
                riddleColor = "green";
                break;

            case 3:
                colorImg.color = Color.blue;
                riddleColor = "blue";
                break;

            case 4:
                colorImg.color = Color.black;
                riddleColor = "black";
                break;
        }
    }

    public void ChooseColor()
    {
        //Set the color, change riddle text (to hint to something else
    }

    #endregion

    #region Remove with later update/new design

    public void CallCraft()
    {
        ping = new Ping(pingAddress);
        player.riddleTxt.text = "Design your riddle!\nClick done when you are ready!";
    }

    #endregion

    #region Sending Data:

    [HideInInspector]public string DATA_URL = "https://riddle-cube-climber-default-rtdb.firebaseio.com/";
    private FirebaseDatabase databaseReference;

    /// <summary>
    /// Send data to the database!
    /// </summary>
    public void SubmitRiddle()
    {
        if (ping != null)
        {
            /*
             * Save color based on colorIndex
             * Save riddle based on the riddleText.text
             * Save answer based on the answerText.text
            */

            //Sending data:
            if (riddleInput.text.Equals("") || answerInput.text.Equals(""))
            {
                player.riddleTxt.text = "Please fill in the riddle and the answer before submiting!";
                return;
            }

            //Test... :S
            RiddleData data = new RiddleData(riddleInput.text, answerInput.text, "false");
            string jsonData = JsonUtility.ToJson(data);
            databaseReference.RootReference.Child($"riddles/{riddleColor}/").Push().SetRawJsonValueAsync(jsonData);

            player.Respawn();
            player.riddleTxt.text = "Thank you for helping out the community!";
            ExitCrafting(false);
        }
        else
            ExitCrafting(true);
    }

    #endregion
}

[Serializable]
public class RiddleData
{
    public string question;
    public string answer;
    public string isActive;

    public RiddleData(string rid, string ans, string active)
    {
        question = rid;
        answer = ans;
        isActive = active;
    }
}
