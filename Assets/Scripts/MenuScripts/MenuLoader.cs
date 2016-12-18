using UnityEngine;
using System.Collections;

public class MenuLoader : MonoBehaviour
{
    public AudioManager audioManager;   // prefab to instantiate

    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }
    }
}
