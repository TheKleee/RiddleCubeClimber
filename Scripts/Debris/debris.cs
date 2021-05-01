using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class debris : MonoBehaviour
{
    [Header("Platform Debris:")]
    public GameObject[] platformDebris; //This will change when we add platform skins!!! >:|

    public void SetColor(PColor pColor)
    {
        switch (pColor)
        {
            case PColor.red:
                GameObject plat0 = Instantiate(platformDebris[0], transform.position, platformDebris[0].transform.rotation);
                plat0.transform.parent = transform;
                break;

            case PColor.green:
                GameObject plat1 = Instantiate(platformDebris[1], transform.position, platformDebris[1].transform.rotation);
                plat1.transform.parent = transform;
                break;

            case PColor.blue:
                GameObject plat2 = Instantiate(platformDebris[2], transform.position, platformDebris[2].transform.rotation);
                plat2.transform.parent = transform;
                break;

            case PColor.white:
                GameObject plat3 = Instantiate(platformDebris[3], transform.position, platformDebris[3].transform.rotation);
                plat3.transform.parent = transform;
                break;

            case PColor.black:
                GameObject plat4 = Instantiate(platformDebris[4], transform.position, platformDebris[4].transform.rotation);
                plat4.transform.parent = transform;
                break;
        }

        SaveData.instance.Sfx();
        Timing.RunCoroutine(_Expire().CancelWith(gameObject));
    }

    IEnumerator<float> _Expire()
    {
        yield return Timing.WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
