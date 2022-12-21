using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

public class RaceController : MonoBehaviourPunCallbacks
{
    public static bool racing = false;
    public static int totalLaps = 1;
    public int timer = 3;
    public CheckpointController[] checkpointControllers;
    public Text startText;
    AudioSource audioSource;
    public AudioClip count;
    public AudioClip start;
    public GameObject endPanel;

    public GameObject carPrefab;
    public Transform[] spawnPositions;
    public int playerCount;

    public GameObject startRace;
    public GameObject waitingText;

    public RawImage mirror;

    private void Start()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
       
        endPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        startText.gameObject.SetActive(true);
        startRace.SetActive(false);
        waitingText.SetActive(false);

        SpawnCar();

        
    }

    [PunRPC]
    public void StartGame()
    {
        InvokeRepeating("CountDown", 3, 1);
        
        waitingText.SetActive(false);

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        checkpointControllers = new CheckpointController[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            checkpointControllers[i] = cars[i].GetComponent<CheckpointController>();
        }
    }

    public void BeginGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartGame", RpcTarget.All, null);
        }
        
        startRace.SetActive(false);
    }


    private void SpawnCar()
    {
        
        GameObject playerCar = null;
        

        if (PhotonNetwork.IsConnected)
        {
            
            Vector3 startPos = spawnPositions[playerCount - 1].position;
            Quaternion startRot = Quaternion.LookRotation(spawnPositions[playerCount - 1].right);

            object[] instanceData = new object[4];
            instanceData[0] = PlayerPrefs.GetString("PlayerName");
            instanceData[1] = PlayerPrefs.GetInt("Red");
            instanceData[2] = PlayerPrefs.GetInt("Green");
            instanceData[3] = PlayerPrefs.GetInt("Blue");

            if (OnlinePlayer.LocalPlayerInstance == null)
            {
                playerCar = PhotonNetwork.Instantiate(carPrefab.name, startPos, startRot, 0, instanceData);
                playerCar.GetComponent<CarAppearance>().SetLocalPlayer();
            }

            if (PhotonNetwork.IsMasterClient) startRace.SetActive(true);
            else waitingText.SetActive(true);

        }

        playerCar.GetComponent<PlayerController>().enabled = true;
    }

    private void LateUpdate()
    {
        int finishedLap = 0;
        foreach(CheckpointController cc in checkpointControllers)
        {
            if (cc.lap == totalLaps + 1) finishedLap++;
            if(finishedLap == checkpointControllers.Length && racing)
            {
                endPanel.SetActive(true);
                racing = false;
            }
        }
    }

    void CountDown()
    {
        if(timer != 0)
        {
            startText.text = timer.ToString();
            audioSource.PlayOneShot(count);
            timer--;
        }
        else
        {
            startText.text = "START";
            audioSource.PlayOneShot(start);
            racing = true;
            CancelInvoke("CountDown");
            Invoke("HideStartText", 1);
        }
    }

    void HideStartText()
    {
        startText.gameObject.SetActive(false);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SetMirror(Camera rearCamera)
    {
        mirror.texture = rearCamera.targetTexture;
    }
}
