using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameOverController : MonoBehaviour
{
    private void Start()
    {
        SetActiveAllChildren(false);
    }

    private void OnEnable()
    {
        GameManager.OnRaceVictory += OnGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnRaceVictory -= OnGameOver;
    }

    private void SetActiveAllChildren(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }

    private void OnGameOver()
    {
        SetActiveAllChildren(true);
    }

    // Buttons -----------------------------------------------------------------
    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
