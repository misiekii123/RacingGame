using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPanel : MonoBehaviour
{
    public List<Text> places;

    void Start()
    {
        Leaderboard.Reset();
    }

    void LateUpdate()
    {
        List<string> players = Leaderboard.GetPlaces();

        for(int i = 0; i < places.Count; i++)
        {
            if (i < players.Count)
                places[i].text = players[i];
            else
                places[i].text = "";
        }
    }
}
