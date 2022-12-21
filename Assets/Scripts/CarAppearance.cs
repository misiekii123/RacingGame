using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarAppearance : MonoBehaviour
{
    public string playerName;
    public Color carColor;
    public Text nameText;
    public Renderer carRenderer;
    public int playerNumber;
    public Camera rearCamera;
    int carRego;
    bool regoSet = false;
    public CheckpointController checkpoints;

    public void SetNameAndColor(string name, Color color)
    {
        playerName = name;
        nameText.text = name;
        nameText.color = color;
        carRenderer.material.color = color;
    }

    public void SetLocalPlayer()
    {
        playerName = PlayerPrefs.GetString("PlayerName");
        int r = PlayerPrefs.GetInt("Red"),
            g = PlayerPrefs.GetInt("Green"),
            b = PlayerPrefs.GetInt("Blue");
        carColor = ColorCar.IntToColor(r, g, b);
        nameText.text = playerName;
        nameText.color = carColor;
        carRenderer.material.color = carColor;

        FindObjectOfType<CameraController>().SetCameraProperties(this.gameObject);

        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        rearCamera.targetTexture = renderTexture;
        FindObjectOfType<RaceController>().SetMirror(rearCamera);
    }

    private void LateUpdate()
    {
        if (!regoSet)
        {
            carRego = Leaderboard.RegisterCar(playerName);
            regoSet = true;
            return;
        }

        Leaderboard.SetPosition(carRego, checkpoints.lap, checkpoints.currentCheckpoint);
    }
}
