using UnityEngine;
using Colyseus;
using Colyseus.Schema;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    public GameObject playerPrefab;

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

    void OnStateChange(GameState state, bool first)
    {
        state.players.ForEach((id, obj) =>
        {
            Player player = obj as Player;
            if (player == null) return;

            GameObject go = PlayerRegistry.Get(id);

            // ===== SPAWN =====
            if (go == null)
            {
                go = Instantiate(playerPrefab);
                go.name = "Player_" + id;

                var controller = go.AddComponent<PlayerController>();
                controller.isLocal = (id == room.SessionId);

                PlayerRegistry.Add(id, go);
            }

            // ===== POSITION SYNC =====
            // Only sync REMOTE players (not the local one)
            if (id != room.SessionId)
            {
                go.transform.position = Vector3.Lerp(
                    go.transform.position,
                    new Vector3(player.x, player.y, player.z),
                    Time.deltaTime * 12f
                );
            }

            go.transform.rotation = Quaternion.Euler(0, player.rotY, 0);

            // ===== ANIMATIONS =====
            Animator anim = go.GetComponent<Animator>();
            if (anim != null)
            {
                bool isLocalPlayer = (id == room.SessionId);

                // Only sync animation for OTHER players
                if (!isLocalPlayer)
                {
                    anim.SetBool("walk", player.anim == "walk");
                    anim.SetBool("sit", player.anim == "sit");

                    if (player.jumping)
                        anim.SetTrigger("jump");
                }
            }

            // ===== SKIN SYNC =====
            var skin = go.GetComponent<PlayerSkin>();
            if (skin != null)
                skin.ApplySkin((int)player.skin);
        });

        // ===== CLEANUP =====
        List<string> remove = new List<string>();

        foreach (var id in PlayerRegistry.AllIds())
            if (!state.players.ContainsKey(id))
                remove.Add(id);

        foreach (var id in remove)
            PlayerRegistry.Remove(id);
    }

    // ===== SEND INPUT =====

    public void SendMove(Vector3 pos, float rot, bool walking)
    {
        if (room == null) return;

        room.Send("move", new
        {
            x = pos.x,
            y = pos.y,
            z = pos.z,
            rotY = rot,
            anim = walking ? "walk" : "idle"
        });
    }

    public void SendJump()
    {
        if (room == null) return;
        room.Send("jump");
    }

    public void SendSit(bool sit)
    {
        if (room == null) return;
        room.Send("sit", sit);
    }

    public void SendSkin(int id)
    {
        if (room == null) return;
        room.Send("skin", id);
    }
}