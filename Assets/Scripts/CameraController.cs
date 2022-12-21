using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    Damping damping;
    public Vector3[] positions;
    public CinemachineVirtualCamera vcam;
    CinemachineTransposer transposer;
    int activePosition = 0;

    private void Start()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        if (positions.Length == 0) return;

        damping = new Damping(transposer);

        SetSpecificView(activePosition);
    }

    private void Update()
    {        
        if (positions.Length == 0) return;
        if (Input.GetKeyDown(KeyCode.V))
            ChangeView();
    }

    private void ChangeView()
    {
        activePosition++;
        activePosition = activePosition % positions.Length;
        SetSpecificView(activePosition);
    }

    private void SetSpecificView(int viewIndex)
    {
        switch(viewIndex)
        {
            case (int)CarViews.BEHIND_CAR_CLOSER:
            case (int)CarViews.BEHIND_CAR_FURTHER:
                damping.Restore(transposer);
                break;
            case (int)CarViews.ON_MASK:
                TurnOffDampings();
                break;
            default:
                Debug.Log("Unhandled view");
                break;
        }

        transposer.m_FollowOffset = positions[viewIndex];
    }

    private void TurnOffDampings()
    {
        transposer.m_XDamping = 0f;
        transposer.m_YDamping = 0f;
        transposer.m_ZDamping = 0f;
    }

    public void SetCameraProperties(GameObject car)
    {
        vcam.Follow = car.GetComponent<DrivingScript>().rigidbodyOfCar.transform;
        vcam.LookAt = car.GetComponent<DrivingScript>().cameraTarget.transform;
    }
}
