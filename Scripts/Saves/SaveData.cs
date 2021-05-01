using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    #region Singleton

    public static SaveData instance;

    private void Awake()
    {
        //Set screen size for Standalone
        #if UNITY_STANDALONE
            Screen.SetResolution(720, 900, false);
            Screen.fullScreen = false;
        #endif

        if (instance != null)
            Destroy(gameObject);        //Make sure that there aren't multiple instances of this or we're screwed >xD
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    #endregion

    #region Main Save Information:

    [Header("Main Save Data:")]
    public int level;
    public int areaId;          //Background zones...
    public int playerSkinId;    //Player skins...
    public int platformId;      //Platforms...
    public int selectedPlatform;
    public int selectedSkin;
    #endregion

    #region Save and Load:

    public void SaveGame()              //This should be called whenever the game needs to be saved...
    {
        Save.SaveUser(this);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void LoadGame()
    {
        //Load user data...
        if (File.Exists(Application.persistentDataPath + "/user.dat"))
        {
            UserValuesData player = Save.LoadUser();

            level = player.level;
            areaId = player.areaId;
            playerSkinId = player.playerSkinId;
            platformId = player.platformId;

            selectedPlatform = player.selectedPlatform;
            selectedSkin = player.selectedSkin;
        }
    }

    #endregion


    #region Sound Control:


    public AudioSource aSource;
    public void Sfx()
    {
        aSource.Play();
    }

    #endregion
}
