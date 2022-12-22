using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrivingScript : MonoBehaviour
{
    public WheelScript[] wheels;
    public float torque = 200,
        maxSteerAngle = 30,
        maxBrakeTorque = 500,
        maxSpeed = 200,
        previousSpeed = 0f,
        currentSpeed;
    public Rigidbody rigidbodyOfCar;
    public AudioSource engineSound;
    public bool isCarReversing = false;
    public GameObject backLights;
    public GameObject reverseLights;
    public GameObject cameraTarget;
    public int nitroFuel = 3;
    public int nitroPower = 930000;
    public Text nitroCounter;

    public int currentGear = 1;
    const float GEAR_FACTOR = 0.05f;

    [Header("Nitro Bar")]
    public GameObject segment1;
    public GameObject segment2;
    public GameObject segment3;

    private void Start()
    {
        nitroCounter = GameObject.FindGameObjectWithTag("Fuel").GetComponent<Text>();
        nitroCounter.text = nitroFuel.ToString();

        segment1 = GameObject.FindGameObjectWithTag("Seg1");
        segment2 = GameObject.FindGameObjectWithTag("Seg2");
        segment3 = GameObject.FindGameObjectWithTag("Seg3");
    }

    public void Drive(float acceleration, float brake, float steerAngle)
    {
        acceleration = Mathf.Clamp(acceleration, -1, 1);
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        steerAngle = Mathf.Clamp(steerAngle, -1, 1) * maxSteerAngle;

        currentSpeed = rigidbodyOfCar.velocity.magnitude * 3;
        float thrustTorque = 0;

        if(currentSpeed < maxSpeed) 
            thrustTorque = acceleration * torque;

        foreach(WheelScript wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = thrustTorque;
            
            if(wheel.isFrontWheel)
                wheel.wheelCollider.steerAngle = steerAngle;
            else
                wheel.wheelCollider.brakeTorque = brake;

            Quaternion rotation;
            Vector3 position;
            wheel.wheelCollider.GetWorldPose(out position, out rotation);
            wheel.wheelModel.transform.position = position;
            wheel.wheelModel.transform.rotation = rotation;
        }
    }


    public void EngineSound()
    {
        if (engineSound.pitch == 1f && previousSpeed < currentSpeed && !isCarReversing)
            currentGear++;

        else if (engineSound.pitch < 0.8f && currentGear > 1 && previousSpeed > currentSpeed)
            currentGear--;

        currentSpeed = rigidbodyOfCar.velocity.magnitude * 3;
        float speedPerc = Mathf.InverseLerp(0f, maxSpeed * currentGear * GEAR_FACTOR, currentSpeed);
        float pitch = Mathf.Lerp(0.3f, 1f, speedPerc);

        engineSound.pitch = pitch;

        previousSpeed = currentSpeed;
    }

    public void ActivateReverseLights()
    {
        isCarReversing = Vector3.Dot(rigidbodyOfCar.velocity.normalized, rigidbodyOfCar.transform.forward.normalized) < 0 && currentSpeed >= 0.1f;
        reverseLights.SetActive(isCarReversing);
    }

    public void ActivateBrakeLights(float brake)
    {
        if (brake != 0)
        {
            backLights.SetActive(true);
        }
        else
        {
            backLights.SetActive(false);
        }
    }

    void Boost(float boostPower)
    {
        rigidbodyOfCar.AddForce(rigidbodyOfCar.gameObject.transform.forward * boostPower);
    }

    public void ChangeFuelText()
    {
        nitroCounter.text = "Nitro: " + nitroFuel.ToString();
    }

    public void Nitro(bool isOn)
    {
        if(nitroFuel > 0 && isOn)
        {
            Boost(nitroPower);
            nitroFuel -= 1;
            ChangeFuelText();
        }
    }

    public void NitroBar()
    {
        if (nitroFuel >= 3)
        {
            segment1.SetActive(true);
            segment2.SetActive(true);
            segment3.SetActive(true);
        }

        else if (nitroFuel == 2)
        {
            segment1.SetActive(true);
            segment2.SetActive(true);
            segment3.SetActive(false);
        }

        else if (nitroFuel == 1)
        {
            segment1.SetActive(true);
            segment2.SetActive(false);
            segment3.SetActive(false);
        }

        else if (nitroFuel == 0)
        {
            segment1.SetActive(false);
            segment2.SetActive(false);
            segment3.SetActive(false);
        }
    }
}
