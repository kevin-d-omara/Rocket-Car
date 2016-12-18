using UnityEngine;
using System.Collections;

public class KillFloorController : MonoBehaviour
{
    public delegate void KillFloorHit();
    public static event KillFloorHit OnKillFloorHit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            // play sound

            if (OnKillFloorHit != null)
            {
                OnKillFloorHit(); // broadcast event
            }
        }
        else if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject); // prevent Obstacles from falling for infinity
        }
    }
}
