using UnityEngine;
using Colyseus;
using Colyseus.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    public GameObject playerPrefab;

    Client client;
    public Room<GameState> room;
    public string CurrentRoomCode;

    [Header("Localhost Server URL")]
    [SerializeField] string localhostServerURL = "ws://localhost:2567";

    [Header("Cloud Server URL")]
    [SerializeField] string cloudServerURL =
        "wss://colyseuswebglmultiplayerserver-production.up.railway.app";

    [Header("Use Cloud?")]
    [SerializeField] bool useCloud = false;

    // ===== EVENTS FOR UI =====
    public Action<string> OnRoomJoined;
    public Action<List<Player>> OnPlayerListUpdated;

    void Awake()
    {
        Instance = this;

        string url = useCloud ? cloudServerURL : localhostServerURL;
        client = new Client(url);

        Debug.Log("Connecting to: " + url);
    }

    // =============================
    // CREATE ROOM
    // =============================
    public async Task CreateRoom(string playerName)
    {
        try
        {
            room = await client.Create<GameState>("my_room");

            CurrentRoomCode = room.RoomId;
            Debug.Log("Created Room: " + CurrentRoomCode);
            Debug.Log("Session ID: " + room.SessionId);

            room.OnStateChange += OnStateChange;

            SendName(playerName);

            OnRoomJoined?.Invoke(CurrentRoomCode);
        }
        catch (Exception e)
        {
            Debug.LogError("Create Room Failed: " + e);
        }
    }

    // =============================
    // JOIN ROOM
    // =============================
    public async Task JoinRoomByCode(string code, string playerName)
    {
        try
        {
            room = await client.JoinById<GameState>(code);

            CurrentRoomCode = code;
            Debug.Log("Joined Room: " + code);
            Debug.Log("Session ID: " + room.SessionId);

            room.OnStateChange += OnStateChange;

            SendName(playerName);

            OnRoomJoined?.Invoke(code);
        }
        catch (Exception e)
        {
            Debug.LogError("Join Failed: " + e);
        }
    }

    // =============================
    // STATE SYNC
    // =============================
    void OnStateChange(GameState state, bool first)
    {
        Debug.Log("STATE UPDATE RECEIVED");

        List<Player> playerList = new List<Player>();

        state.players.ForEach((id, obj) =>
        {
            Player player = obj as Player;
            if (player == null) return;

            playerList.Add(player);

            GameObject go = PlayerRegistry.Get(id);

            if (go == null)
            {
                go = Instantiate(playerPrefab);
                go.name = "Player_" + id;

                var controller = go.AddComponent<PlayerController>();
                controller.isLocal = (id == room.SessionId);

                PlayerRegistry.Add(id, go);
            }

            if (id != room.SessionId)
            {
                go.transform.position = Vector3.Lerp(
                    go.transform.position,
                    new Vector3(player.x, player.y, player.z),
                    Time.deltaTime * 12f
                );
            }

            go.transform.rotation = Quaternion.Euler(0, player.rotY, 0);

            var skin = go.GetComponent<PlayerSkin>();
            if (skin != null)
                skin.ApplySkin((int)player.skin);
        });

        OnPlayerListUpdated?.Invoke(playerList);
        if (AreAllPlayersReady())
        {
            Debug.Log("ALL PLAYERS READY → START GAME");

            GameStartController.Instance?.StartGame();
        }
    }

    public bool AreAllPlayersReady()
    {
        if (room == null || room.State == null) return false;

        int count = 0;
        int ready = 0;

        room.State.players.ForEach((id, obj) =>
        {
            Player p = obj as Player;
            if (p == null) return;

            count++;
            if (p.ready) ready++;
        });

        if (count == 0) return false;

        return count == ready;
    }

    // =============================
    // SEND INPUT
    // =============================

    public void SendMove(Vector3 pos, float rot, bool walking)
    {
        room?.Send("move", new {
            x = pos.x,
            y = pos.y,
            z = pos.z,
            rotY = rot,
            anim = walking ? "walk" : "idle"
        });
    }

    public void SendJump() => room?.Send("jump");
    public void SendSit(bool sit) => room?.Send("sit", sit);
    public void SendSkin(int id) => room?.Send("skin", id);
    public void SendReady(bool ready) => room?.Send("ready", ready);
    public void SendName(string name) => room?.Send("setName", name);

    void OnDestroy()
    {
        if (room != null)
            room.OnStateChange -= OnStateChange;
    }
}