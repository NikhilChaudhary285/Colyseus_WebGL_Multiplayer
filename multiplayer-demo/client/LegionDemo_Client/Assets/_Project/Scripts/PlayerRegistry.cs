using System.Collections.Generic;
using UnityEngine;

public static class PlayerRegistry
{
    static Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    public static void Add(string id, GameObject go)
    {
        players[id] = go;
    }

    public static GameObject Get(string id)
    {
        players.TryGetValue(id, out var go);
        return go;
    }

    public static void Remove(string id)
    {
        if (players.TryGetValue(id, out var go))
        {
            players.Remove(id);
        }
    }

    public static IEnumerable<string> AllIds()
    {
        return players.Keys;
    }
}
