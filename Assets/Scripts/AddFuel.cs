using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFuel : MonoBehaviour
{
    public PlayerController playerController;
    public DrivingScript drivingScript;

    public bool Add()
    {
        if (playerController.enabled)
        {
            drivingScript.nitroFuel += 1;
            drivingScript.nitroFuel = Mathf.Clamp(drivingScript.nitroFuel, 0, 5);
            drivingScript.ChangeFuelText();
            return true;
        }
        else
            return false;
    }
}
