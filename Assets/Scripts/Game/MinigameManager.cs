using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    // Create a singleton minigame manager
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // unload all minigames
    private void Start()
    {
        SceneManager.UnloadSceneAsync(2);
    }

    // Load shooter minigame
    public void LoadSideScrollingShooter()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        //SceneManager.UnloadSceneAsync(1);
    }
}
