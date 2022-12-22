
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    DrivingScript drivingScript;
    CheckpointController checkpointController;
    GameObject carBody;
    public Rigidbody rigidbodyOfCar;
    float lastTimeMoving = 0f;
    
    private void Start()
    {
        drivingScript = GetComponent<DrivingScript>();
        carBody = drivingScript.rigidbodyOfCar.gameObject;
        checkpointController = carBody.GetComponent<CheckpointController>();
    }

    private void Update()
    {
        float acceleration = Input.GetAxis("Vertical");
        float steerAngle = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");
        bool nitro = Input.GetKeyDown(KeyCode.LeftShift);

        CheckIfCarResetIsNeeded();

        if (!RaceController.racing || checkpointController.lap == RaceController.totalLaps + 1)
        {
            brake = 1f;
            acceleration = 0f;
            steerAngle = 0f;
        }

        drivingScript.ActivateReverseLights();
        drivingScript.ActivateBrakeLights(brake);
        drivingScript.Nitro(nitro);
        drivingScript.NitroBar();
        drivingScript.Drive(acceleration, brake, steerAngle);
        drivingScript.EngineSound();
    }

    private void ResetLayer()
    {
        carBody.layer = 0;
    }

    private void CheckIfCarResetIsNeeded()
    {
        if(drivingScript.rigidbodyOfCar.velocity.magnitude > 1 || !RaceController.racing)
        {
            lastTimeMoving = Time.time;
        }

        if (Time.time > lastTimeMoving + 5 || carBody.transform.position.y < -5)
        {
            MoveCarToLastCheckpoint();
        }
    }

    private void MoveCarToLastCheckpoint()
    {
        if (checkpointController.lastCheckpointObject == null)
        {
            return;
        }

        carBody.transform.position = checkpointController.lastCheckpointObject.transform.position + Vector3.up*2;
        carBody.transform.rotation = Quaternion.LookRotation(checkpointController.lastCheckpointObject.transform.right);

        carBody.layer = 6;

        Invoke("ResetLayer", 3);
    }
}
