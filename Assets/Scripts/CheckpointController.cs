using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public int lap = 0;
    public int currentCheckpoint = -1;
    int checkpointCounter;
    public int nextCheckpoint;
    public GameObject lastCheckpointObject;

    private void Start()
    {
        GameObject[] checkpoints =
            GameObject.FindGameObjectsWithTag("Checkpoint");
        checkpointCounter = checkpoints.Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Checkpoint")
        {
            int thisCheckpoint = int.Parse(other.gameObject.name);
            if(thisCheckpoint == nextCheckpoint)
            {
                lastCheckpointObject = other.gameObject;
                currentCheckpoint = nextCheckpoint;
                if(currentCheckpoint == 0)
                {
                    lap++;
                    Debug.Log("Lap: " + lap);
                }
                nextCheckpoint++;
                nextCheckpoint = nextCheckpoint % checkpointCounter;
            }
        }
    }
}
