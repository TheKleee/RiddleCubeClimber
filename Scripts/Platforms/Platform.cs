using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Platform : MonoBehaviour
{
    #region Private data:
    
    private PlayerController Player;
    private PColor questionColor;
    int randQ;
    int prevRandMat;    //Store the value of the previous rand mat!
    int randPoints;
    #endregion

    [Header("Main Info:")]
    public bool grayPlatform;

    [Header("Platform type:")]
    public bool isMainPlatform;
    [Space]
    public PColor pColor;

    [Header("References:")]
    public Material[] platformMaterials;    //List of platform materials => these are based on the platform type!
    public GameObject mainPlatform;           //This one is called when we've cleared the level
    public GameObject[] platform;               //Regular platform...
    [Space]
    public Transform[] points;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerController>();

        if (!isMainPlatform)
            grayPlatform = false;
    }

    private void Start()
    {

        if (SaveData.instance.level != 0)
            grayPlatform = false;

        //if (grayPlatform)
        //{
        //    if (isMainPlatform)
        //    {
        //        //Show the menu UI elements + hide gameplay elements (except text, lvl and points) >:D
        //        if (SaveData.instance.level == 0)
        //        {
        //            GetComponentInChildren<MeshRenderer>().material = platformMaterials[0]; //This should be the gray material!!! >:|
        //            //LevelZero();
        //        }
        //    }
        //}
    }

    public void LevelZero()
    {
        Player.StartLevel();
        //Summon a white platform
        Player.riddletype = 3;
        GameObject plat = Instantiate(platform[SaveData.instance.selectedPlatform], points[0].position, points[0].rotation);
        plat.GetComponent<Platform>().pColor = PColor.white;
        plat.GetComponent<Platform>().SetPlatformMaterial();
        Player.pColor = PColor.white;
        Player.riddleTxt.text = "Tap the white platform!";

        Player.RemoveWhiteZero();
    }

    public void NewLevel()
    {
        TinySauce.OnGameStarted(levelNumber: "Level_" + SaveData.instance.level);

        if (SaveData.instance.level > 0)
        {
            Player.StartLevel();
            //Let's do it here instead... :/
            Player.pColor = PColor.white;
            Timing.RunCoroutine(_SummonMainPlatform().CancelWith(gameObject));
        }
        else
            LevelZero();
    }

    public void SetGrayMat()
    {
        grayPlatform = true;
        GetComponentInChildren<MeshRenderer>().material = platformMaterials[0]; //This should be the gray material!!! >:|
    }

    #region Summons:

    public void SetPlatformMaterial()
    {
        switch (pColor)
        {
            case PColor.red:
                GetComponentInChildren<MeshRenderer>().material = platformMaterials[1];
                break;

            case PColor.green:
                GetComponentInChildren<MeshRenderer>().material = platformMaterials[2];
                break;

            case PColor.blue:
                GetComponentInChildren<MeshRenderer>().material = platformMaterials[3];
                break;

            case PColor.white:
                GetComponentInChildren<MeshRenderer>().material = platformMaterials[4];
                break;

            case PColor.black:
                GetComponentInChildren<MeshRenderer>().material = platformMaterials[5];
                break;
        }
    }


    public void SummonPlatforms()   //Set this to a delay!!!
    {
        if (grayPlatform)
        {
            ShufflePlatforms();

            if (isMainPlatform)
                Player.StartLevel();

            randPoints = Random.Range(1, points.Length);

            if (randPoints == 1)
                randPoints += 2;

            for (int i = 0; i < randPoints; i++)
            {
                GameObject posObj = Instantiate(platform[SaveData.instance.selectedPlatform], points[i].position, points[i].rotation);
                posObj.transform.parent = points[i];    //Set posObj as a child of points transform!
                int randMat = Random.Range(1, platformMaterials.Length);
                if (randMat == randQ)
                {
                    if (randMat != platformMaterials.Length)
                        randMat++;
                    else
                        randMat--;
                }

                if (randMat == prevRandMat)
                {
                    if (randMat != 2)
                        randMat -= 2;
                    else if (randMat != platformMaterials.Length)
                        randMat += 2;
                    else
                        randMat -= 5;
                }

                switch (randMat)
                {
                    case 1:
                        posObj.GetComponentInChildren<Platform>().pColor = PColor.red;
                        break;

                    case 2:
                        posObj.GetComponentInChildren<Platform>().pColor = PColor.green;
                        break;

                    case 3:
                        posObj.GetComponentInChildren<Platform>().pColor = PColor.blue;
                        break;

                    case 4:
                        posObj.GetComponentInChildren<Platform>().pColor = PColor.white;
                        break;

                    case 5:
                        posObj.GetComponentInChildren<Platform>().pColor = PColor.black;
                        break;
                }

                prevRandMat = randMat;
                posObj.GetComponent<Platform>().SetPlatformMaterial();
            }

            points[0].GetComponentInChildren<Platform>().pColor = questionColor;  //We need a question color!
            points[0].GetComponentInChildren<MeshRenderer>().material = platformMaterials[randQ];

            //Summon from 1 to 3 platforms and set them to corresponding platform positions.
            //Randomly set their pColor in a loop and after the loop set the pColor based on the question on the first one!
            //Shuffle the platforms.

            Player.riddletype = randQ - 1;
            Player.Riddle();
        }
    }

    public void ShufflePlatforms()
    {
        //use this to shuffle all the points!
        for (int i = 0; i < points.Length; i++)
        {
            int randomIndex = Random.Range(i, points.Length);
            Transform temp = points[i];
            points[i] = points[randomIndex];
            points[randomIndex] = temp;
        }
        SetQuestion();
    }

    public void SetQuestion()
    {
        randQ = Random.Range(1, 6);

        switch (randQ)
        {
            case 1:
                questionColor = PColor.red;
                Player.pColor = questionColor;
                break;

            case 2:
                questionColor = PColor.green;
                Player.pColor = questionColor;
                break;

            case 3:
                questionColor = PColor.blue;
                Player.pColor = questionColor;
                break;

            case 4:
                questionColor = PColor.white;
                Player.pColor = questionColor;
                break;

            case 5:
                questionColor = PColor.black;
                Player.pColor = questionColor;
                break;
        }
    }

    public IEnumerator<float> _SummonMainPlatform()
    {
        yield return Timing.WaitForSeconds(2.5f);
        GameObject mainPlat = Instantiate(mainPlatform, transform.GetChild(1).position, transform.GetChild(1).rotation);
        mainPlat.GetComponent<Platform>().pColor = PColor.white;
        mainPlat.GetComponent<Platform>().SetPlatformMaterial();
    }

    

    #endregion

    public void SetPlayerType()
    {
        int randQuestion = Random.Range(0, 5);

        switch (randQuestion)
        {
            case 0:
                PColor question0 = PColor.red;
                Player.pColor = question0;
                break;

            case 1:
                PColor question1 = PColor.green;
                Player.pColor = question1;
                break;

            case 2:
                PColor question2 = PColor.blue;
                Player.pColor = question2;
                break;

            case 3:
                PColor question3 = PColor.white;
                Player.pColor = question3;
                break;

            case 4:
                PColor question4 = PColor.black;
                Player.pColor = question4;
                break;
        }
    }

    [Header("Vfx:")]
    public GameObject vfx;

    public void ClearPlatformsNoPTS()
    {
        Platform[] pl = FindObjectsOfType<Platform>();
        foreach (Platform plat in pl)
        {
            if (!plat.grayPlatform)
            {
                GameObject fx = Instantiate(vfx, plat.transform.position, Quaternion.identity);
                fx.GetComponent<debris>().SetColor(plat.pColor);
            }
        }

        //Get no points
        for (int i = 0; i < randPoints; i++)
        {
            Destroy(points[i].GetChild(0).gameObject);
            //Instantiate no points effect!
        }
    }

    public void ClearPlatformsPTS()
    {
        Platform[] pl = FindObjectsOfType<Platform>();
        foreach (Platform plat in pl)
        {
            if (!plat.grayPlatform)
            {
                GameObject fx = Instantiate(vfx, plat.transform.position, Quaternion.identity);
                fx.GetComponent<debris>().SetColor(plat.pColor);
            }
        }

        //Get points
        for (int i = 0; i < randPoints; i++)
        {
            Destroy(points[i].gameObject);
            //Instantiate get points effect at points[i] position!
        }

        //Instantiate get points effect at current position!
        Destroy(gameObject);    //Destroy self
    }
}
