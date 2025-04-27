using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckScenes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {

        if(!SceneManager.GetSceneByBuildIndex(1).IsValid()) SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        if(!SceneManager.GetSceneByBuildIndex(3).IsValid()) SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        if(!SceneManager.GetSceneByBuildIndex(4).IsValid()) SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);
        if(!SceneManager.GetSceneByBuildIndex(5).IsValid()) SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);
    }
}
