using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuButtonController : MonoBehaviour
{
    public Dictionary<string, GameObject> pages = new Dictionary<string, GameObject>();

    public delegate void PlayGame();
    public static event PlayGame OnPlayGame;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Page"))
            {
                pages.Add(child.name, child.gameObject);
                if (child.name == "Main")
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void PlayNewGame()
    {
        SceneManager.LoadScene("LevelZero", LoadSceneMode.Single);
        if (OnPlayGame != null)
        {
            OnPlayGame();
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void EnterLevelSelectMenu()
    {
        foreach (KeyValuePair<string, GameObject> page in pages)
        {
            page.Value.SetActive(false);
        }
        pages["Level Select"].SetActive(true);
    }

    public void EnterHelpMenu()
    {
        foreach (KeyValuePair<string, GameObject> page in pages)
        {
            page.Value.SetActive(false);
        }
        pages["Help"].SetActive(true);
    }

    public void EnterCreditsMenu()
    {
        foreach (KeyValuePair<string, GameObject> page in pages)
        {
            page.Value.SetActive(false);
        }
        pages["Credits"].SetActive(true);
    }

    public void BackToMainMenu()
    {
        foreach (KeyValuePair<string, GameObject> page in pages)
        {
            page.Value.SetActive(false);
        }
        pages["Main"].SetActive(true);
    }
}

