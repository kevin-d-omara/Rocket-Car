using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameTypes { Laps, RaceToFinish }

public class GameManager : MonoBehaviour
{
    public float PlayerTime { get; private set; }
    public int CurrentLap { get; private set; }
    public int NumberOfRetries { get; private set; }

    public delegate void RaceVictory();
    public static event RaceVictory OnRaceVictory;

    public delegate void LapCompleted(int currentLap, int totalLap);
    public static event LapCompleted OnLapCompleted;

    [SerializeField] private GameObject mainCamera; // Main Camera instance
    [SerializeField] private GameObject spawnPoint; // Checkpoint instance
    [SerializeField] private GameObject endPoint;   // Checkpoint instance
    [SerializeField] private GameObject rocketCarPrefab;
    [SerializeField] private GameTypes gameType;
    [SerializeField] private int laps = 1; // ignore if GameType is RaceToFinish

    private GameObject lastCheckpoint;
    private GameObject rocketCar;
    private List<GameObject> checkpoints = new List<GameObject>();

    private void Awake()
    {
        lastCheckpoint = spawnPoint;
        PlayerTime = 0f;
        CurrentLap = 1;
        NumberOfRetries = 0;
        SpawnCar();

        if (gameType == GameTypes.RaceToFinish)
        {
            laps = 1;
        }
    }

    private void Start()
    {
        // pre-race checkpoint initialization
        GameObject[] checkpointArray = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (GameObject checkpoint in checkpointArray)
        {
            checkpoints.Add(checkpoint);
        }
        spawnPoint.GetComponent<CheckpointController>().hasBeenReached = true;

        // remove End Point from checkpoints if Laps GameType
        if (gameType == GameTypes.Laps)
        {
            endPoint.GetComponent<CheckpointController>().hasBeenReached = true;
            checkpoints.Remove(endPoint);
        }
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

                        if (OnLapCompleted != null)
                        {
                            OnLapCompleted(CurrentLap, laps);
                        }
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

        return numReached == checkpoints.Count;
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
        rocketCar.GetComponentInChildren<ThrusterController>().ResetFuel();

        mainCamera.GetComponent<CameraController>().target = rocketCar.transform;
    }

    private void OnKillFloorHit()
    {
        RevertToLastCheckpoint();
    }

    private void RevertToLastCheckpoint()
    {
        NumberOfRetries++;
        Destroy(rocketCar);
        SpawnCar();
    }

    private void Victory()
    {
        if (OnRaceVictory != null)
        {
            OnRaceVictory();
        }
    }

    public GameObject GetSpawnPoint()
    {
        return spawnPoint;
    }

    public int GetLapsToWin()
    {
        return laps;
    }
}
