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
    [SerializeField] private List<GameObject> buttons;
    private bool isPaused = false;
    private AudioSource[] allAudioSources;

    private void Awake()
    {
        SetActiveAllButtons(false);
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
        SetActiveAllButtons(true);
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        UnPauseAllAudio();
        SetActiveAllButtons(false);
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

    // Buttons -----------------------------------------------------------------
    private void SetActiveAllButtons(bool value)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(value);
        }
    }

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
