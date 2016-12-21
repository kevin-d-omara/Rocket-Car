using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameOverController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Text timeText;

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

        float time = gameManager.PlayerTime;
        timeText.text = "Time!" + Environment.NewLine + toTimeFormat(time);
    }

    private string toTimeFormat(float number)
    {
        int mins = (int)(number / 60);
        int seconds = ((int)number - mins * 60);
        int milis = (int)((number - (int)number) * 100);
        string milisstring = milis < 10 ? "0" + milis : "" + milis;

        return mins + ":" + seconds + ":" + milisstring;
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
