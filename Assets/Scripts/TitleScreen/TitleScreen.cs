using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // switch scenes
            SceneManager.LoadScene(1);
        }
    }
}
