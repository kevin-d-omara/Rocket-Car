using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera; // Main Camera instance
    [SerializeField] private GameObject spawnPoint; // Checkpoint instance
    [SerializeField] private GameObject rocketCarPrefab;

    private GameObject lastCheckpoint;
    private GameObject rocketCar;

    public float playerTime { get; private set; }

    private void Awake()
    {
        lastCheckpoint = spawnPoint;
        playerTime = 0f;
        SpawnCar();
    }

    private void OnEnable()
    {
        CheckpointController.OnCheckpointReached += OnCheckpointReached;
        KillFloorController.OnKillFloorHit += OnKillFloorHit;
        PauseController.OnLastCheckpoint += RevertToLastCheckpoint;
    }

    private void OnDisable()
    {
        CheckpointController.OnCheckpointReached -= OnCheckpointReached;
        KillFloorController.OnKillFloorHit -= OnKillFloorHit;
        PauseController.OnLastCheckpoint -= RevertToLastCheckpoint;
    }

    private void Update()
    {
        playerTime += Time.deltaTime;
    }

    private void OnCheckpointReached(GameObject checkpoint)
    {
        lastCheckpoint = checkpoint;
        rocketCar.GetComponentInChildren<ThrusterController>().ResetFuel();
    }

    private void SpawnCar()
    {
        Vector3 position = lastCheckpoint.transform.position;
        Quaternion rotation = lastCheckpoint.transform.rotation;

        rocketCar = Instantiate(rocketCarPrefab, position, rotation) as GameObject;


        mainCamera.GetComponent<CameraController>().target = rocketCar.transform;
    }

    private void OnKillFloorHit()
    {
        RevertToLastCheckpoint();
    }

    private void RevertToLastCheckpoint()
    {
        Destroy(rocketCar);
        SpawnCar();
    }
}
