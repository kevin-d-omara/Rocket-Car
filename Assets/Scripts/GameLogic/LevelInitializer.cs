using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LevelInitializer : MonoBehaviour {
    float countDown;
    Text countDownText;
    public void Start()
    {
        countDownText = GetComponent<Text>();
        countDownText.text = "3";
        StartCoroutine(tickCountDown());
        GameObject.Find("Main Camera").GetComponent<DOF>().enabled = true;
    }
    IEnumerator tickCountDown()
    {
        yield return new WaitForSeconds(1);
        switch (countDownText.text) {
            case "3":
                countDownText.text = "2"; StartCoroutine(tickCountDown()); break;
            case "2":
                countDownText.text = "1"; StartCoroutine(tickCountDown()); break;
            case "1":
                countDownText.text = "GO!"; StartCoroutine(tickCountDown()); break;
            case "GO!":
                countDownText.text = ""; StartGame();

                break;
        }
    }

    public void StartGame()
    {
        GameObject.Find("Main Camera").GetComponent<DOF>().enabled = false;
    }
}
