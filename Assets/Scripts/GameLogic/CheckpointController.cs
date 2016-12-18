using UnityEngine;
using System.Collections;

// NOTE: when placing a Checkpoint prefab in the Scene, point the z-axis (blue
//       arrow in the direction of travel. This is the direction the car will
//       face when spawning.
public class CheckpointController : MonoBehaviour
{
    public delegate void CheckpointReached(GameObject checkpoint);
    public static event CheckpointReached OnCheckpointReached;

    private bool hasBeenReached = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenReached && other.transform.root.CompareTag("Player"))
        {
            hasBeenReached = true;
            // play sound

            if (OnCheckpointReached != null)
            {
                OnCheckpointReached(gameObject); // broadcast event and pass
                                                 // pointer to this Checkpoint
            }
        }
    }
}