using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public void StartGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // switch scenes
            SceneManager.LoadScene(1);
        }
    }
}
