using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectController : MonoBehaviour
{
    public delegate void PlayLevel();
    public static event PlayLevel OnPlayLevel;

    [Serializable]
    private class Level
    {
        [SerializeField] private string displayName;
        [SerializeField] private string sceneName;
        [SerializeField] private Sprite picture;

        public Level(string name, Sprite picture)
        {
            this.displayName = name;
            this.picture = picture;
        }

        public string getDisplayName() { return displayName; }
        public string getSceneName() { return sceneName; }
        public Sprite getPicture() { return picture; }
    }
    [SerializeField] private List<Level> levelList;
    private CircularList<Level> levels;

    [SerializeField] private Image current;
    [SerializeField] private Image next;
    [SerializeField] private Image previous;
    [SerializeField] private Text levelName;

    private void Start()
    {
        levels = new CircularList<Level>(levelList);
        UpdateLevelPictures();
    }

    private void UpdateLevelPictures()
    {
        current.sprite = levels.peekCurrent().getPicture();
        next.sprite = levels.peekNext().getPicture();
        previous.sprite = levels.peekPrevious().getPicture();

        levelName.text = levels.peekCurrent().getDisplayName();
    }

    public void SelectNextLevel()
    {
        levels.rotateNext();
        UpdateLevelPictures();
    }

    public void SelectPreviousLevel()
    {
        levels.rotatePrevious();
        UpdateLevelPictures();
    }

    public void PlayCurrentLevel()
    {
        string sceneName = levels.peekCurrent().getSceneName();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        if (OnPlayLevel != null)
        {
            OnPlayLevel();
        }
    }

    private class CircularList<T>
    {
        private int index = 0;
        private List<T> list;

        public CircularList (List<T> list)
        {
            this.list = list;
        }

        public T peekCurrent()
        {
            return list[index];
        }

        public T peekNext()
        {
            return list[(index + 1) % list.Count];
        }

        public T peekPrevious()
        {
            return list[(index + list.Count - 1) % list.Count];
        }

        public T rotateNext()
        {
            index = (index + 1) % list.Count;
            return list[index];
        }

        public T rotatePrevious()
        {
            index = (index + list.Count - 1) % list.Count;
            return list[index];
        }
    }
}
