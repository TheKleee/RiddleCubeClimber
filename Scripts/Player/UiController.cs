using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class UiController : MonoBehaviour
{
    [Header("UI Elements:")]
    public Image progressImg;
    public Text lvlTxt;
    public Text nextLvlTxt;
    public Color col;

    [Header("Skins:")]
    public Skins[] skins;   //List of UI skins :|
    public GameObject skinBackground;
    private SkinType sType;
    private bool viewSkinList;

    [Header("Riddle Craft:")]
    public GameObject rCraft;
    public GameObject craftBtn;

    public void SetProgressImg(float progress)
    {
        if (progress > 0)
            Timing.RunCoroutine(_ProgressFill(progress));
        else
            progressImg.fillAmount = 0;
    }

    IEnumerator<float> _ProgressFill(float progress)
    {
        float prog = progress;
        while (prog > 0)
        {
            if (progressImg.fillAmount >= progress)
                break;

            prog -= .015f;
            progressImg.fillAmount += .015f;
            yield return Timing.WaitForSeconds(.015f);
        }

        if (progressImg.fillAmount == 1)
            nextLvlTxt.GetComponentInParent<Image>().color = col;
    }

    /// <summary>
    /// Call this to view the list of player skins... : /
    /// </summary>
    public void PlayerSkinList()
    {
        sType = SkinType.character;
        CallList(GetComponent<PlayerController>().canTap);
    }

    /// <summary>
    /// Same as above but for platforms instead >: D
    /// </summary>
    public void PlatformSkinList()
    {
        sType = SkinType.platform;
        CallList(GetComponent<PlayerController>().canTap);
    }
    /// <summary>
    /// This is for maps : )
    /// </summary>
    public void mapList()
    {
        sType = SkinType.map;
        CallList(GetComponent<PlayerController>().canTap);
    }

    public void CraftRiddle()
    {
        if (GetComponent<PlayerController>().canTap)
        {
            rCraft.SetActive(true);
            craftBtn.SetActive(false);
            GetComponent<PlayerController>().canTap = false;
        }
    }

    #region I'm not trolling xD

    [Header("Btn Imgs:")]
    public Image playerBtn;
    public Image platformBtn;
    public Image mapBtn;


    [HideInInspector] public Sprite playerSprite;
    [HideInInspector] public Sprite platformSprite;
    public void CallList(bool canCall)
    {
        if (canCall)
        {
            viewSkinList = !viewSkinList;
            skinBackground.SetActive(viewSkinList);

            foreach (Skins s in skins)
            {
                if (s.sType == sType)
                {
                    s.gameObject.SetActive(true);
                    switch (sType)
                    {
                        case SkinType.character:
                            if (s.skinID > SaveData.instance.playerSkinId)
                            {
                                s.gameObject.SetActive(false);
                            }
                            break;

                        case SkinType.platform:
                            if (s.skinID > SaveData.instance.platformId)
                            {
                                s.gameObject.SetActive(false);
                            }
                            break;

                        case SkinType.map:
                            if (s.skinID > SaveData.instance.areaId)
                            {
                                s.gameObject.SetActive(false);
                            }
                            break;
                    }
                }
                else
                    s.gameObject.SetActive(false);
            }

            playerSprite = GetComponent<SkinsInfo>().playerSkins[SaveData.instance.selectedSkin];
            platformSprite = GetComponent<SkinsInfo>().platformSkins[SaveData.instance.selectedPlatform];

            playerBtn.sprite = playerSprite;
            platformBtn.sprite = platformSprite;
        }
        
        else
            skinBackground.SetActive(false);
    }

    #endregion
}
