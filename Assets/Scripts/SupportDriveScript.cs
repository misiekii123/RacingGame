using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportDriveScript : MonoBehaviour
{
    [Header("0 - left wheel, 1 - right wheel")]
    public WheelCollider[] frontWheels = new WheelCollider[2];
    public WheelCollider[] rearWheels = new WheelCollider[2];
    public float antiRoll = 5000.0f;
    Rigidbody rigidbodyOfCar;

    private void Start()
    {
        rigidbodyOfCar = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HoldWheelsOnGround(frontWheels);
        HoldWheelsOnGround(rearWheels);
    }

    private void HoldWheelsOnGround(WheelCollider[] wheels)
    {
        WheelHit hit;
        float leftRiding = 1.0f;
        float rightRiding = 1.0f;

        bool groundedL = wheels[0].GetGroundHit(out hit);
        if(groundedL) leftRiding =
                (-wheels[0].transform.InverseTransformPoint(hit.point).y
                - wheels[0].radius) / wheels[0].suspensionDistance;

        bool groundedR = wheels[0].GetGroundHit(out hit);
        if (groundedR) rightRiding =
                (-wheels[1].transform.InverseTransformPoint(hit.point).y
                - wheels[1].radius) / wheels[1].suspensionDistance;

        float antiRollForce = (leftRiding - rightRiding) * antiRoll;

        if (groundedL) rigidbodyOfCar.AddForceAtPosition(wheels[0].transform.up * -antiRollForce, wheels[0].transform.position);
        if (groundedR) rigidbodyOfCar.AddForceAtPosition(wheels[1].transform.up * antiRollForce, wheels[1].transform.position);
    }
}
