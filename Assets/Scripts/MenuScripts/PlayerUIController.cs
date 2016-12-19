using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private Slider boostMeter;
    [SerializeField] private Text timeText;
    [SerializeField] private Text retryText;
    [SerializeField] private Text lapText;

    [SerializeField] private Text checkpointText;
    [SerializeField] private float checkpointFadeInTime = 0.25f;
    [SerializeField] private float checkpointFadeOutTime = 0.25f;
    [SerializeField] private float checkpointPauseTime = 0.5f;
    [SerializeField] private Text newLapText;

    private void Start()
    {
        checkpointText.gameObject.SetActive(false);
        newLapText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        PauseController.OnLastCheckpoint += UpdateRetryText;
        GameManager.OnLapCompleted += UpdateLapText;
        ThrusterController.OnFuelUpdate += UpdateBoostMeter;
        CheckpointController.OnCheckpointReached += PresentCheckpointText;
    }

    private void OnDisable()
    {
        PauseController.OnLastCheckpoint -= UpdateRetryText;
        GameManager.OnLapCompleted -= UpdateLapText;
        ThrusterController.OnFuelUpdate -= UpdateBoostMeter;
        CheckpointController.OnCheckpointReached -= PresentCheckpointText;
    }

    private void SetActiveAllChildren(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }

    // Timer -------------------------------------------------------------------
    private void Update()
    {
        float time = gameManager.PlayerTime;
        timeText.text = "Time: " + toTimeFormat(time);  
    }

    private string toTimeFormat(float number)
    {
        int mins = (int)(number / 60);
        int seconds = ((int)number - mins * 60);
        int milis = (int)((number - (int)number) * 100);
        string milisstring = milis < 10 ? "0" + milis : "" + milis;

        return mins + ":" + seconds + ":" + milisstring;
    }

    // Retry -------------------------------------------------------------------
    private void UpdateRetryText()
    {
        StartCoroutine(UpdateRetryTextAfterShortPause());
    }

    private IEnumerator UpdateRetryTextAfterShortPause()
    {
        yield return new WaitForSeconds(.05f);
        retryText.text = "Retries: " + gameManager.NumberOfRetries;
    }

    // Lap ---------------------------------------------------------------------
    private void UpdateLapText(int currentLap, int totalLap)
    {
        lapText.text = "Lap " + currentLap + "/" + totalLap;

        // display "New Lap"
        newLapText.gameObject.SetActive(true);
        StartCoroutine(FadeText(newLapText, 0f, 1f, checkpointFadeInTime, checkpointPauseTime, true));
    }

    // Boost -------------------------------------------------------------------
    private void UpdateBoostMeter(float percentRemaining)
    {
        boostMeter.value = percentRemaining;
    }

    // Checkpoint --------------------------------------------------------------
    private void PresentCheckpointText(GameObject checkpoint)
    {
        if (checkpoint != gameManager.GetSpawnPoint())
        {
            checkpointText.gameObject.SetActive(true);
            StartCoroutine(FadeText(checkpointText, 0f, 1f, checkpointFadeInTime, checkpointPauseTime, true));
        }
    }

    private IEnumerator FadeText(Text text, float startAlpha, float endAlpha, float fadeTime, float pauseTime, bool fadingIn)
    {
        Color color = text.color;
        color.a = startAlpha;

        float timer = 0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, timer / fadeTime);
            text.color = color;
            yield return null;
        }

        if (fadingIn)
        {
            yield return new WaitForSeconds(pauseTime);
            StartCoroutine(FadeText(text, 1f, 0f, checkpointFadeOutTime, checkpointPauseTime, false));
        }
    }
}
