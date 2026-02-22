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
    [SerializeField] string cloudServerURL = "wss://colyseuswebglmultiplayerserver-production.up.railway.app";

    [Header("Use Cloud?")]
    [SerializeField] bool useCloud = false;

    // EVENTS
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

        state.players.ForEach((id, obj) =>
        {
            Player player = obj as Player;
            if (player == null) return;

            playerList.Add(player);

            GameObject go = PlayerRegistry.Get(id);

            // SPAWN PLAYER IF NOT EXISTS
            if (go == null)
            {
                go = Instantiate(playerPrefab);

                // APPLY SERVER SPAWN POSITION
                go.transform.position = new Vector3(player.x, player.y, player.z);
                go.transform.rotation = Quaternion.Euler(0, player.rotY, 0);

                go.name = "Player_" + id;

                var controller = go.AddComponent<PlayerController>();
                controller.isLocal = (id == room.SessionId);

                PlayerRegistry.Add(id, go);
            }

            // ===== APPLY SKIN (THIS WAS MISSING) =====
            var skin = go.GetComponent<PlayerSkin>();
            if (skin != null)
            {
                skin.ApplySkin((int)player.skin);
            }

            // APPLY MOVEMENT ONLY AFTER MATCH START
            if (state.matchStarted && id != room.SessionId)
            {
                go.transform.position = Vector3.Lerp(
                    go.transform.position,
                    new Vector3(player.x, player.y, player.z),
                    Time.deltaTime * 12f
                );
            }

            go.transform.rotation = Quaternion.Euler(0, player.rotY, 0);

            // ===== ANIMATION SYNC =====
            if (id != room.SessionId) // only remote players
            {
                var animator = go.GetComponent<Animator>();
                if (animator != null)
                {
                    // WALK
                    animator.SetBool("walk", player.anim == "walk");

                    // SIT
                    animator.SetBool("sit", player.sitting);

                    // JUMP (trigger)
                    if (player.jumping)
                    {
                        animator.SetTrigger("jump");
                    }
                }
            }
        });

        OnPlayerListUpdated?.Invoke(playerList);

        // COUNTDOWN EVENT
        if (lastCountdown != (int)state.countdown)
        {
            lastCountdown = (int)state.countdown;
            OnCountdown?.Invoke(lastCountdown);
        }

        // MATCH START EVENT
        if (state.matchStarted && !matchStartedTriggered)
        {
            matchStartedTriggered = true;
            OnMatchStarted?.Invoke();
        }
    }

    public void RequestStartGame() => room?.Send("startGame");

    // =============================
    // INPUT MESSAGES (GAMEPLAY)
    // =============================
    public void SendMove(Vector3 pos, float rot, bool walking)
    {
        room?.Send("move", new
        {
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