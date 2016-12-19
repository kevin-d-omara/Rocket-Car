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

    private void OnEnable()
    {
        PauseController.OnLastCheckpoint += UpdateRetryText;
        GameManager.OnLapCompleted += UpdateLapText;
        ThrusterController.OnFuelUpdate += UpdateBoostMeter;
    }

    private void OnDisable()
    {
        PauseController.OnLastCheckpoint -= UpdateRetryText;
        GameManager.OnLapCompleted -= UpdateLapText;
        ThrusterController.OnFuelUpdate -= UpdateBoostMeter;
    }

    private void SetActiveAllChildren(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }

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

    private void UpdateRetryText()
    {
        StartCoroutine(UpdateRetryTextAfterShortPause());
    }

    IEnumerator UpdateRetryTextAfterShortPause()
    {
        yield return new WaitForSeconds(.05f);
        retryText.text = "Retries: " + gameManager.NumberOfRetries;
    }

    private void UpdateLapText(int currentLap, int totalLap)
    {
        lapText.text = "Lap " + currentLap + "/" + totalLap;
    }

    private void UpdateBoostMeter(float percentRemaining)
    {
        boostMeter.value = percentRemaining;
    }
}
