using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public enum SkinType
{
    character,
    platform,
    map
}
public class Skins : MonoBehaviour
{
    public SkinType sType;

    [Header("Player ref:")]
    public SkinManager player;   //This might be important xD

    [Header("World ref:")]
    public World world; //The whole wide world >:D
    public int worldID;

    [Header("Skin ID:")]
    public int skinID;  //Please don't ask me why we need this! >:\
    
    /// <summary>
    /// Call this to summon the selected skin!!! >:D
    /// </summary>
    public void SetSelectedSkin()
    {
        if (player.GetComponent<PlayerController>().canTap)
        {
            switch (sType)
            {
                case SkinType.character:
                    SaveData.instance.selectedSkin = skinID;
                    player.SetSkinType();
                    break;

                case SkinType.platform:
                    if (skinID != SaveData.instance.selectedPlatform)
                    {
                        SaveData.instance.selectedPlatform = skinID;
                        player.GetComponent<PlayerController>().Respawn();
                    }
                    break;

                case SkinType.map:
                    world.CreateWorld(worldID, true);
                    break;
            }
            SaveData.instance.SaveGame();
        }

        player.GetComponent<UiController>().CallList(player.GetComponent<PlayerController>().canTap);
    }
}
