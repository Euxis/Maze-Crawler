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
            SceneManager.LoadScene("ManagerScene", LoadSceneMode.Single);

            SceneManager.LoadScene("Maze", LoadSceneMode.Additive);
            SceneManager.LoadScene("BulletHell", LoadSceneMode.Additive);
            SceneManager.LoadScene("Options", LoadSceneMode.Additive);
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Maze"));
            SceneManager.UnloadSceneAsync("TitleScreen");

        }
    }

    public void ExitGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Application.Quit();
        }
    }
    
}
