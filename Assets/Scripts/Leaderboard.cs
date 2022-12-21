using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Leaderboard
{
    static Dictionary<int, Car> board = new Dictionary<int, Car>();
    static int carsRegistered = -1;

    public static void Reset()
    {
        board.Clear();
        carsRegistered = -1;
    }

    public static int RegisterCar(string name)
    {
        carsRegistered++;
        board.Add(carsRegistered, new Car(name, 0));
        return carsRegistered;
    }

    public static void SetPosition(int rego, int lap, int checkpoint)
    {
        int position = lap * 1000 + checkpoint;
        board[rego] = new Car(board[rego].name, position);
    }

    public static List<string> GetPlaces()
    {
        List<string> places = new List<string>();
        foreach (var pos in board.OrderByDescending(key => key.Value.position))
        {
            places.Add(pos.Value.name);
        }
        return places;
    }
}