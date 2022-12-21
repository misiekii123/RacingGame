using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlinePlayer : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
        else
        {
            string playerName = null;
            Color playerColor = Color.white;

            if (photonView.InstantiationData != null)
            {
                playerName = (string)photonView.InstantiationData[0];

                int r = (int)photonView.InstantiationData[1],
                    g = (int)photonView.InstantiationData[2],
                    b = (int)photonView.InstantiationData[3];
                playerColor = ColorCar.IntToColor(r, g, b);

                GetComponent<CarAppearance>().SetNameAndColor(playerName, playerColor);
            }
        }
    }
}
