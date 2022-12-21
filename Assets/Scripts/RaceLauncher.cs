using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class RaceLauncher : MonoBehaviourPunCallbacks
{
    public InputField playerName;
    byte maxPlayerPerRoom = 3;
    bool isConnecting;
    public Text networkText;
    string gameVersion = "1";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerName.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }

    public void StartRace()
    {
        SceneManager.LoadScene(1);
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            networkText.text += "OnConnectToMaster...\n";
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        networkText.text += "Failed to join random room.";
        PhotonNetwork.CreateRoom(null, new RoomOptions
        {
            MaxPlayers = this.maxPlayerPerRoom
        });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        networkText.text += "Disconnected because " + cause + "\n";
        isConnecting = false;
    }

    public override void OnJoinedRoom()
    {
        networkText.text += "Joined Room with" +
            PhotonNetwork.CurrentRoom.PlayerCount + "players.\n";
        PhotonNetwork.LoadLevel("SampleScene");
    }

    public void ConnectNetwork()
    {
        networkText.text = "";
        isConnecting = true;
        PhotonNetwork.NickName = playerName.text;
        if (PhotonNetwork.IsConnected)
        {
            networkText.text += "Joining Room...\n";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            networkText.text += "Connecting...\n";
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}
