using UnityEngine;
using Colyseus;
using Colyseus.Schema;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    Client client;
    public Room<GameState> room;

    async void Awake()
    {
        Instance = this;
        client = new Client("ws://localhost:2567");
    }

    async void Start()
    {
        await CreateRoom();
    }

    public async System.Threading.Tasks.Task CreateRoom()
    {
        room = await client.JoinOrCreate<GameState>("my_room");
        Debug.Log("Joined room: " + room.RoomId);

        room.OnStateChange += OnStateChange;
    }

    void OnStateChange(GameState state, bool isFirstState)
    {
        // LOOP THROUGH SERVER PLAYERS
        state.players.ForEach((id, playerObj) =>
        {
            Player player = playerObj as Player;
            if (player == null) return;

            GameObject obj = PlayerRegistry.Get(id);

            // SPAWN if not exists
            if (obj == null)
            {
                Debug.Log("Spawn player: " + id);

                obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                obj.name = "Player_" + id;

                var controller = obj.AddComponent<PlayerController>();
                controller.isLocal = (id == room.SessionId);

                PlayerRegistry.Add(id, obj);
            }

            // ALWAYS UPDATE POSITION FROM SERVER
            obj.transform.position = new Vector3(player.x, player.y, player.z);
            obj.transform.rotation = Quaternion.Euler(0, player.rotY, 0);
        });

        // CLEANUP players removed from server
        List<string> toRemove = new List<string>();

        foreach (var id in PlayerRegistry.AllIds())
        {
            if (!state.players.ContainsKey(id))
                toRemove.Add(id);
        }

        foreach (var id in toRemove)
        {
            Debug.Log("Remove player: " + id);
            PlayerRegistry.Remove(id);
        }
    }
}
