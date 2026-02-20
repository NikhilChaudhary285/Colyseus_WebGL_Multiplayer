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
        // ===== SPAWN / UPDATE PLAYERS =====
        state.players.ForEach((id, playerObj) =>
        {
            Player player = playerObj as Player;
            if (player == null) return;

            GameObject obj = PlayerRegistry.Get(id);

            // ===== SPAWN IF NEW =====
            if (obj == null)
            {
                Debug.Log("Spawn player: " + id);

                obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                obj.name = "Player_" + id;

                var controller = obj.AddComponent<PlayerController>();
                controller.isLocal = (id == room.SessionId);

                // Add Animator placeholder if prefab not used yet
                if (obj.GetComponent<Animator>() == null)
                    obj.AddComponent<Animator>();

                PlayerRegistry.Add(id, obj);
            }

            // ===== POSITION SMOOTH SYNC =====
            obj.transform.position = Vector3.Lerp(
                obj.transform.position,
                new Vector3(player.x, player.y, player.z),
                Time.deltaTime * 10f
            );

            // ===== ROTATION SYNC =====
            obj.transform.rotation = Quaternion.Euler(0, player.rotY, 0);

            // ===== ANIMATION SYNC =====
            var anim = obj.GetComponent<Animator>();
            if (anim != null)
            {
                // Reset bools first
                anim.SetBool("walk", false);
                anim.SetBool("sit", false);

                // WALK
                if (player.anim == "walk")
                    anim.SetBool("walk", true);

                // SIT
                else if (player.anim == "sit")
                    anim.SetBool("sit", true);

                // IDLE fallback
                else
                    anim.Play("Idle");

                // JUMP trigger
                if (player.jumping)
                    anim.SetTrigger("jump");
            }

            // ===== SKIN SYNC =====
            var skin = obj.GetComponent<PlayerSkin>();
            if (skin != null)
                skin.ApplySkin((int)player.skin);
        });

        // ===== CLEANUP REMOVED PLAYERS =====
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

    // ================= SEND INPUT TO SERVER =================

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