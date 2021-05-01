using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkinManager : MonoBehaviour
{

    public GameObject[] skinList;   //The list of all the skins... ofc xD

    /// <summary>
    /// Use this whenver you are summoning a skin!!! >: \
    /// </summary>
    public void SetSkinType()
    {
        if(transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);  //Let's destroy it first >: D

        GameObject playerType = Instantiate(skinList[SaveData.instance.selectedSkin],
            new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z),
            transform.rotation
            );
        playerType.transform.parent = transform;
    }
}
