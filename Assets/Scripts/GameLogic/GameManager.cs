using UnityEngine;
using System.Collections;

public enum GameTypes { Laps, RaceToFinish }

public class GameManager : MonoBehaviour
{
    public float PlayerTime { get; private set; }
    public int CurrentLap { get; private set; }
    public int NumberOfRetries { get; private set; }

    public delegate void RaceVictory();
    public static event RaceVictory OnRaceVictory;

    [SerializeField] private GameObject mainCamera; // Main Camera instance
    [SerializeField] private GameObject spawnPoint; // Checkpoint instance
    [SerializeField] private GameObject endPoint;   // Checkpoint instance
    [SerializeField] private GameObject rocketCarPrefab;
    [SerializeField] private GameTypes gameType;
    [SerializeField] private int laps = 1; // ignore if GameType is RaceToFinish

    private GameObject lastCheckpoint;
    private GameObject rocketCar;
    private GameObject[] checkpoints;

    private void Awake()
    {
        lastCheckpoint = spawnPoint;
        PlayerTime = 0f;
        CurrentLap = 1;
        NumberOfRetries = 0;
        SpawnCar();
    }

    private void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        spawnPoint.GetComponent<CheckpointController>().hasBeenReached = true;
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
        PlayerTime += Time.deltaTime;
    }

    private void OnCheckpointReached(GameObject checkpoint)
    {
        lastCheckpoint = checkpoint;
        rocketCar.GetComponentInChildren<ThrusterController>().ResetFuel();

        if (gameType == GameTypes.RaceToFinish && checkpoint == endPoint)
        {
            Victory();
        }
        else if (gameType == GameTypes.Laps)
        {
            if (AllCheckpointsReached())
            {
                if (checkpoint == spawnPoint)
                {
                    if (CurrentLap == laps) // Victory!
                    {
                        Victory();
                    }
                    else
                    {
                        CurrentLap++;
                        ResetAllCheckpoints();
                    }
                }
                else
                {
                    spawnPoint.GetComponent<CheckpointController>().hasBeenReached = false;
                }
            }
        }
    }

    private bool AllCheckpointsReached()
    {
        int numReached = 0;
        foreach (GameObject checkpoint in checkpoints)
        {
            if (checkpoint.GetComponent<CheckpointController>().hasBeenReached)
            {
                numReached++;
            }
        }

        return numReached == checkpoints.Length;
    }

    private void ResetAllCheckpoints()
    {
        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.GetComponent<CheckpointController>().hasBeenReached = false;
        }
        spawnPoint.GetComponent<CheckpointController>().hasBeenReached = true;
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

    private void Victory()
    {
        Debug.Log("Victory!");
        if (OnRaceVictory != null)
        {
            OnRaceVictory();
        }
    }
}
