using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class World : MonoBehaviour
{
    public PlayerController player;
    [Header("World Background:")]
    public GameObject Background;

    [Header("Maps:")]
    public GameObject[] Maps;

    [Header("Helper:")]
    public Helper helper;

    private void Awake()
    {
        int randMap = Random.Range(0, SaveData.instance.areaId + 1);
        if(SaveData.instance.areaId > Maps.Length)
        {
            randMap--;  //This is like a special fix... just in case, but it should never be called... right? >xD
        }

        CreateWorld(randMap, false);
    }

    public void CreateWorld(int randID, bool destPrevMap)
    {
        if(destPrevMap)
        {
            Destroy(Background.transform.GetChild(0).gameObject);   //Test... Like the rest xD
        }

        GameObject map = Instantiate(Maps[randID]);  //Set to an index from savedata.cs
        map.transform.parent = Background.transform;
    }

    private void Start()
    {
        Timing.RunCoroutine(_SetWorld().CancelWith(gameObject));
    }

    public IEnumerator<float> _SetWorld()
    {
        if (SaveData.instance.level == 0)
        {
            //Start Helper.cs
            helper.gameObject.SetActive(true);
            helper.HelpStart();
        }

        yield return 0;
        player.GetComponent<SkinManager>().SetSkinType();
        player.StartLevel();
    }
}
