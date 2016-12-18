using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null; // singleton AudioManager

    // music clips (looping background music)
    [SerializeField] private AudioClip menu;
    [SerializeField] private AudioClip[] inGame;
    private Dictionary<string, AudioSource> music = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        // enforce singleton pattern for AudioManager
        if (instance == null)
        {
            instance = this;            // first AudioManager instance becomes the singleton
        }
        else if (instance != this)
        {
            Destroy(gameObject);        // all other instances get destroyed
        }
        DontDestroyOnLoad(gameObject);  // preserve parent GameObject to preserve the singleton

        // populate music dictionary
        music.Add("Menu", AddAudioSourceComponent(menu, true, false, 1f));
        foreach (AudioClip song in inGame)
        {
            music.Add(song.ToString(), AddAudioSourceComponent(song, true, false, 1f));
        }
        music["Menu"].Play();
    }

    private AudioSource AddAudioSourceComponent(AudioClip clip, bool loop, bool playAwake, float vol)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;
        return newAudio;
    }

    private void OnEnable()
    {
        // music subscriptions
        MenuButtonController.OnPlayGame += PlayInGame;
        //UIController.OnBackToMenu += PlayMenu;
    }

    private void OnDisable()
    {
        // music subscriptions
        MenuButtonController.OnPlayGame -= PlayInGame;
        //UIController.OnBackToMenu += PlayMenu;
    }

    private void PlayMenu()                                                     // TODO: use StartCoroutine() w/ yield return WaitForFixedUpdate + Time.deltaTime + lerp to fade out
    {
        PlaySong("Menu");
    }

    private void PlayInGame()
    {
        List<string> songs = new List<string>(music.Keys);
        string randomSong = songs[Random.Range(0, songs.Count)];

        PlaySong(randomSong);
    }

    private void PlaySong(string songName)
    {
        foreach (KeyValuePair<string, AudioSource> song in music)
        {
            if (song.Key == songName)
            {
                song.Value.Play();
            }
            else
            {
                song.Value.Stop();
            }
        }
    }
}
