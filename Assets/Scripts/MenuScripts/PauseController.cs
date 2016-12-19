using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PauseController : MonoBehaviour
{
    public delegate void LastCheckpoint();
    public static event LastCheckpoint OnLastCheckpoint;

    [SerializeField] private List<KeyCode> pauseKeys;
    [SerializeField] private List<KeyCode> lastCheckpointKeys;
    private bool isPaused = false;
    private AudioSource[] allAudioSources;

    private void Awake()
    {
        SetActiveAllChildren(false);
    }

    private void Update()
    {
        // check pause state
        if (isPaused)
        {
            foreach (KeyCode key in pauseKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    ResumeGame();
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
                }
            }
        }

        // check for retry
        foreach (KeyCode key in lastCheckpointKeys)
        {
            if (Input.GetKeyDown(key))
            {
                BackToLastCheckpoint();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        PauseAllAudio();
        SetActiveAllChildren(true);
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        UnPauseAllAudio();
        SetActiveAllChildren(false);
        isPaused = false;
    }

    private void OnDestroy()
    {
        ResumeGame();
    }

    // Audio -------------------------------------------------------------------
    void PauseAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource source in allAudioSources)
        {
            source.Pause();
        }
    }

    void UnPauseAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource source in allAudioSources)
        {
            source.UnPause();
        }
    }

    private void SetActiveAllChildren(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }

    // Buttons -----------------------------------------------------------------
    public void BackToLastCheckpoint()
    {
        if (OnLastCheckpoint != null)
        {
            OnLastCheckpoint();
            ResumeGame();
        }
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
