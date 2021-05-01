using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class SkinsInfo : MonoBehaviour
{

    /*
        Make Sure That Skins Do NOT Spawn At The Same Levels As Maps...
        and that player skins don't spawn at same lvls as platform ones >:D
    */



    [Header("Main Data:")]
    public Image mapImg;       //The image used when new map is available!!!
    public Image skinImg;       //The image used when new skins are available!!! Player or Platform... C:<
    private bool skipInfo;
    private bool isMap;
    [Space]

    [Header("Maps:")]
    public Sprite[] mapList;   //Map skins...

    [Header("Player Skins:")]
    public Sprite[] playerSkins;

    [Header("Platform Skins:")]
    public Sprite[] platformSkins;

    [Header("Crafting Info:")]
    public Sprite craftingInfo; //I need to create this one asap >xD
    public Text nameTxt;

    
    #region Functions To Call From Another Script:
    /// <summary>
    /// This is called whenever a new map is unlocked... :/
    /// </summary>
    public void MapUnlocked()
    {
        isMap = true;
        skipInfo = false;
        mapImg.gameObject.SetActive(true);
        mapImg.sprite = mapList[SaveData.instance.areaId];

        Timing.RunCoroutine(HideImg().CancelWith(gameObject));
    }

    public void PlayerSkinUnlocked()
    {
        isMap = false;
        skipInfo = false;
        skinImg.gameObject.SetActive(true);
        skinImg.sprite = playerSkins[SaveData.instance.playerSkinId];

        Timing.RunCoroutine(HideImg().CancelWith(gameObject));

    }

    public void PlatformUnlocked()
    {
        isMap = false;
        skipInfo = false;
        skinImg.gameObject.SetActive(true);
        skinImg.sprite = platformSkins[SaveData.instance.platformId];

        Timing.RunCoroutine(HideImg().CancelWith(gameObject));
    }

    public void CraftingUnlocked()
    {
        isCrafting = true;
        isMap = false;
        nameTxt.text = "Riddle Crafting Unlocked";
        skipInfo = false;
        mapImg.gameObject.SetActive(true);
        mapImg.sprite = craftingInfo;

        //Timing.RunCoroutine(HideImg().CancelWith(gameObject));
    }

    #endregion

    IEnumerator<float> HideImg()
    {
        float waitDur = 2.6f;   //Hopefully this will last for 1.2 sec xD
        do
        {
            if (skipInfo || waitDur <= 0)
                break;

            waitDur -= .2f;
            yield return Timing.WaitForSeconds(.2f);
        } while (waitDur > 0);


        if (isMap)
            mapImg.gameObject.SetActive(false);
        else
            skinImg.gameObject.SetActive(false);

    }

    private bool isCrafting;

    #region Skip BUTTON!!! WOA!! >:O
    /// <summary>
    /// This is a button used to skip ahead!!! >: D
    /// It's really not that big of a deal though -.-
    /// </summary>
    /// 
    public void SkipBtn()
    {
        skipInfo = true;

        if (isCrafting)
        {
            isCrafting = false;
            nameTxt.text = "New Map Unlocked";
            mapImg.gameObject.SetActive(false);
        }
    }

    #endregion
}
