using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MEC;

public class LoadSC : MonoBehaviour
{
    void Awake()
    {
        Timing.RunCoroutine(FindGame().CancelWith(gameObject));
    }

    IEnumerator<float> FindGame()
    {
        yield return Timing.WaitForSeconds(.5f);
        SceneManager.LoadSceneAsync("MainScene");
    }

}
