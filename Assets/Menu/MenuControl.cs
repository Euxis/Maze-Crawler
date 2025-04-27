using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class MenuControl : MonoBehaviour
{
    [Header("Title Text")]
    [SerializeField] private GameObject volumeText;
    [SerializeField] private GameObject shaderText;
    [SerializeField] private GameObject quit;

    [Header("Option Text")]
    [SerializeField] private TMP_Text volumeOption;
    [SerializeField] private TMP_Text shaderOption;

    [Header("Selection Highlight")] 
    [SerializeField] private GameObject highlightObj;

    // Linked list to hold options
    public static LinkedList<GameObject> selection = new LinkedList<GameObject>();
    private static LinkedListNode<GameObject> currentSelection;
    
    private static int currentVolume = 5;
    private static bool enableShaders = true;
    private static bool selectVolume = false;
    
    [SerializeField] private UniversalAdditionalCameraData cameraData;

    private void Start()
    {
        selection.AddFirst(volumeText);
        currentSelection = selection.First;
        selection.AddAfter(currentSelection, shaderOption.gameObject);
        selection.AddAfter(currentSelection.Next, quit);
    }

    public void MoveMenuCursor(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>().y;
        
        if (context.performed)
        {
            // can't go to another option if currently adjusting one
            if (selectVolume) return;
            
            // Move up in menu (back)
            if (direction > 0)
            {
                if (currentSelection.Previous == null) currentSelection = currentSelection.List.First;
                else currentSelection = currentSelection.Previous;
            }

            // Move down menu
            if (direction < 0)
            {
                if (currentSelection.Next == null) currentSelection = currentSelection.List.Last;
                else currentSelection = currentSelection.Next;
            }
            
            Debug.Log(currentSelection.Value.transform.position.y);
            
            highlightObj.transform.position = currentSelection.Value.transform.position;
        }
    }

    public void ConfirmOption(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentSelection.Value == null || selectVolume) return;

            if (currentSelection.Value == volumeText)
            {
                // do volume selection
                selectVolume = true;
                highlightObj.transform.position = volumeOption.transform.position;
            }

            if (currentSelection.Value == shaderOption.gameObject)
            {
                Debug.Log("Selected shader");
                if (enableShaders)
                {
                    enableShaders = false;
                    shaderOption.text = "[ N ]";
                    SetShader(false);
                }
                else
                {
                    enableShaders = true;
                    shaderOption.text = "[ Y ]";
                    SetShader(true);
                }
            }

            if (currentSelection.Value == quit)
            {
                Application.Quit();
            }
        }
    }

    public void AdjustVolume(InputAction.CallbackContext context)
    {
        
        
        var direction = context.ReadValue<Vector2>().x;
        string output;
        
        if (context.performed && selectVolume)
        {
            // increase volume
            if (direction > 0)
            {
                if(currentVolume < 10) currentVolume++;
            }
            // decrease volume
            if (direction < 0)
            {
                if(currentVolume > 0) currentVolume--;
            }
            output = String.Format("[ {0} ]", currentVolume);
            volumeOption.text = output;
            
            MediatorScript.instance.SetMusicVolume(currentVolume);
        }
    }

    public void Back(InputAction.CallbackContext context)
    {
        if (selectVolume && context.performed)
        {
            selectVolume = false;
            highlightObj.transform.position = volumeText.transform.position;
        }
    }

    public void Resume(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // reset position of cursor
            currentSelection = selection.First;
            highlightObj.transform.position = currentSelection.Value.transform.position;
            selectVolume = false;
            MediatorScript.instance.LoadOptionsMenu(false);
        }
    }

    private void SetShader(bool b)
    {
        if (b)
        {
            cameraData.SetRenderer(0);
            MediatorScript.instance.SetShader(false);
        }
        else
        {
            cameraData.SetRenderer(1);
            MediatorScript.instance.SetShader(true);
        }
    }

}
