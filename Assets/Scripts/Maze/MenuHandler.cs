using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    // Handles menu input
    public void OpenMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MediatorScript.instance.LoadOptionsMenu(true);
            //if(!SceneManager.GetSceneByBuildIndex(5).IsValid()) SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);
        }
    }
}
