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

    [SerializeField] string localhostServerURL = "ws://localhost:8080";
    [SerializeField] string cloudServerURL = "wss://colyseus-server-6cav.onrender.com";
    [SerializeField] bool useCloud = false;

    public Action<string> OnRoomJoined;
    public Action<List<Player>> OnPlayerListUpdated;
    public Action<int> OnCountdown;
    public Action OnMatchStarted;

    bool matchStartedTriggered = false;
    int lastCountdown = -1;

    public bool IsHost =>
        room != null &&
        room.State != null &&
        room.SessionId == room.State.hostSessionId;

    void Awake()
    {
        Instance = this;
        string url = useCloud ? cloudServerURL : localhostServerURL;
        client = new Client(url);
        Debug.Log("Connecting to: " + url);
    }

    public async Task CreateRoom(string playerName)
    {
        room = await client.Create<GameState>("my_room");
        CurrentRoomCode = room.RoomId;
        room.OnStateChange += OnStateChange;
        SendName(playerName);
        OnRoomJoined?.Invoke(CurrentRoomCode);
    }

    public async Task JoinRoomByCode(string code, string playerName)
    {
        room = await client.JoinById<GameState>(code);
        CurrentRoomCode = code;
        room.OnStateChange += OnStateChange;
        SendName(playerName);
        OnRoomJoined?.Invoke(code);
    }

    void OnStateChange(GameState state, bool first)
    {
        List<Player> playerList = new List<Player>();
        HashSet<string> currentIds = new HashSet<string>();

        state.players.ForEach((id, obj) =>
        {
            Player player = obj as Player;
            if (player == null) return;

            currentIds.Add(id);
            playerList.Add(player);

            GameObject go = PlayerRegistry.Get(id);

            // ========= SPAWN PLAYER =========
            if (go == null)
            {
                go = Instantiate(playerPrefab);
                go.name = "Player_" + id;

                go.transform.position = new Vector3(player.x, player.y, player.z);
                go.transform.rotation = Quaternion.Euler(0, player.rotY, 0);

                // attach controller
                var controller = go.AddComponent<PlayerController>();
                controller.isLocal = (id == room.SessionId);

                // attach remote sync component ONLY for remote players
                if (id != room.SessionId)
                    go.AddComponent<RemotePlayer>();

                PlayerRegistry.Add(id, go);
            }

            // ========= SKIN =========
            var skin = go.GetComponent<PlayerSkin>();
            if (skin != null)
                skin.ApplySkin((int)player.skin);

            // ========= REMOTE SYNC =========
            if (id != room.SessionId)
            {
                var remote = go.GetComponent<RemotePlayer>();
                if (remote != null)
                    remote.ApplyServerState(player);
            }

            Debug.Log($"[STATE] Player {id} pos:{player.x},{player.y},{player.z} anim:{player.anim} jump:{player.jumping}");
        });

        // ========= REMOVE DISCONNECTED PLAYERS =========
        var registryIds = new List<string>(PlayerRegistry.AllIds());
        foreach (var id in registryIds)
        {
            if (!currentIds.Contains(id))
            {
                GameObject go = PlayerRegistry.Get(id);
                if (go != null)
                {
                    Debug.Log($"[DESTROY] Removing disconnected player: {id} (GameObject: {go.name})");

                    // Optional cleanup
                    var controller = go.GetComponent<PlayerController>();
                    if (controller != null) controller.enabled = false;

                    var remote = go.GetComponent<RemotePlayer>();
                    if (remote != null) remote.enabled = false;

                    Destroy(go);  // ← actual destruction
                }

                PlayerRegistry.Remove(id);  // clean registry
            }
        }

        OnPlayerListUpdated?.Invoke(playerList);

        // ========= COUNTDOWN =========
        if (lastCountdown != (int)state.countdown)
        {
            lastCountdown = (int)state.countdown;
            OnCountdown?.Invoke(lastCountdown);
        }

        // ========= MATCH START =========
        if (state.matchStarted && !matchStartedTriggered)
        {
            matchStartedTriggered = true;
            OnMatchStarted?.Invoke();
        }
    }

    public void RequestStartGame() => room?.Send("startGame");

    // Update all room.Send calls to use this wrapper, e.g., in SendMove:
    public void SendMove(Vector3 pos, float rot, bool walking)
    {
        string animStr = walking ? "walk" : "idle";

        var msg = new MoveMessage
        {
            x = pos.x,
            y = pos.y,
            z = pos.z,
            rotY = rot,
            anim = animStr
        };

        string jsonDebug = JsonUtility.ToJson(msg);
        Debug.Log($"[SEND MOVE FULL] {pos:F2} rot:{rot:F2} anim:{animStr} → JSON: {jsonDebug}");

        if (room != null)
        {
            room?.Send("move", msg);
        }
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

[System.Serializable]
public class MoveMessage
{
    public float x;
    public float y;
    public float z;
    public float rotY;
    public string anim;
}