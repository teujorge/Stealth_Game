using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{

    public GameObject gameOverUI;
    public GameObject gameWinUI;
    bool gameIsOver;

    // Start is called before the first frame update
    void Start()
    {
        Guard.OnGuardHasSpottedPlayer += ShowGameOverUI;
        FindObjectOfType<Player>().OnEndOfLevel += ShowGameWinUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void ShowGameWinUI()
    {
        OnGameStop(gameWinUI);
    }

    void ShowGameOverUI()
    {
        OnGameStop(gameOverUI);
    }

    void OnGameStop(GameObject gameUI)
    {
        gameIsOver = true;
        gameUI.SetActive(true);
        Guard.OnGuardHasSpottedPlayer -= ShowGameOverUI;
        FindObjectOfType<Player>().OnEndOfLevel -= ShowGameWinUI;
    }

}
