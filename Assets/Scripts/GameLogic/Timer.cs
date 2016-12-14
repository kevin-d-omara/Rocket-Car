using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Timer : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    bool running = false;
    float startTime = 0;
    public void StartTimer()
    {
        startTime = Time.time;
        running = true;
    }
   

	// Update is called once per frame
	void Update () {
        float t = Time.time- startTime;
        int mins =(int)(t / 60);
        int seconds = ((int)t  -mins*60);
        int milis =(int)((t - (int)t) * 100);
        string milisstring = milis < 10 ? "0" + milis : ""+milis;
       GetComponent<Text>().text =mins+"."+ seconds + "." + milisstring;
	}
}
