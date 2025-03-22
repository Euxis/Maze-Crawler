using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Points : MonoBehaviour
{
    // Get reference to points UI text
    [SerializeField] private TMP_Text points;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text gameOver;
    [SerializeField] private TMP_Text gameComplete;
    
    [SerializeField] private TMP_Text bitReward;
    [SerializeField] private GameObject UI_Parent;

    [SerializeField] private TMP_Text bitRewardUI;
    
    private int pointCount;
    private int HPCount;

    [SerializeField] private GameObject objPlayer;
    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        gameComplete.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        HPCount = 3;
        pointCount = 0;
    }

    // Checks state of game. If the player has 0 lives left, game over.
    // If they have sufficient number of points, they win!
    private void StateCheck()
    {
        if (pointCount == 10)
        {
            // Start game complete
            if(SceneManager.GetSceneByName("BulletHell").IsValid()) SceneManager.UnloadSceneAsync(3);
            gameObject.SetActive(true);

            StartCoroutine(DoGameFinish());
        }
        if(HPCount == 3) MediatorScript.instance.setShaderVars.ResetChromatic();;

        if (HPCount <= 0)
        {
            // Send game over event
            SceneManager.UnloadSceneAsync(3);
            gameObject.SetActive(true);

            StartCoroutine(DoGameOver());
        }

        if (HPCount == 2)
        {
            MediatorScript.instance.setShaderVars.SetChromatic(0.007f);
            HP.color = Color.yellow;
            objPlayer.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (HPCount == 1)
        {
            MediatorScript.instance.setShaderVars.SetChromatic(0.01f);

            HP.color = Color.red;
            objPlayer.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void AddPoints(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AddPoints(1);
        }
    }

    public void AddPoints(int p)
    {
        pointCount += p;
        points.text = pointCount.ToString() + "/10";
        
        // Spawn point text
        Instantiate(bitReward, objPlayer.transform.position, Quaternion.identity, UI_Parent.transform);
        Instantiate(bitRewardUI, points.transform.position, Quaternion.identity, points.transform);
        
        //prefab.gameObject.GetComponent<Animation>().Play("upwards");
        if (pointCount > 10)
        {
            pointCount = 10;
            return;
        }
        StateCheck();
    }

    private IEnumerator DoGameOver()
    {
        foreach(var obj in PrefabMinigame.Nodes)
        {
            obj.gameObject.SetActive(false);
        }
        gameOver.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    private IEnumerator DoGameFinish()
    {
        MediatorScript.instance.setShaderVars.ResetChromatic();

        foreach(var obj in PrefabMinigame.Nodes)
        {
            Destroy(obj);
        }

        gameComplete.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    public void RemoveLife()
    {
        HPCount--;
        StateCheck();
        playerMovement.ReturnLastPosition();
        HP.text = HPCount.ToString();
    }

}
