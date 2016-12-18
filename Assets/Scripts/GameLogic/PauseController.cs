using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseController : MonoBehaviour
{
    public List<KeyCode> pauseKeys;

    private bool isPaused = false;

    private GameObject menuButton;

    private void Awake()
    {
        menuButton = transform.Find("Menu").gameObject;
        menuButton.SetActive(false);
    }

    private void OnEnable()
    {
        //GameManager.OnGameOver += PauseGame;
    }

    private void OnDisable()
    {
        //GameManager.OnGameOver -= PauseGame;
    }

    private void Update()
    {
        if (isPaused)
        {
            foreach (KeyCode key in pauseKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    ResumeGame();
                    menuButton.SetActive(false);
                }
            }
        }
        else
        {
            foreach (KeyCode key in pauseKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    PauseGame();
                    menuButton.SetActive(true);
                }
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void OnDestroy()
    {
        ResumeGame();
    }
}
