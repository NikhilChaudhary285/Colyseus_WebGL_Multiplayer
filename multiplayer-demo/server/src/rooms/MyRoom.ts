import { Room, Client } from "colyseus";
import { GameState } from "../schema/GameState.js";
import { Player } from "../schema/Player.js";

export class MyRoom extends Room {
  maxClients = 4;
  state = new GameState();

  onCreate(options: any) {

    // ===== MOVEMENT + WALK/IDLE =====
    this.onMessage("move", (client, data) => {
      const p = this.state.players.get(client.sessionId);
      if (!p) return;

      p.x = data.x;
      p.y = data.y;
      p.z = data.z;
      p.rotY = data.rotY;

      // animation string from client
      p.anim = data.anim ?? "idle";
    });

    // ===== JUMP =====
    this.onMessage("jump", (client) => {
      const p = this.state.players.get(client.sessionId);
      if (!p) return;

      p.jumping = true;
      p.anim = "jump";

      // auto reset so trigger fires once
      setTimeout(() => {
        if (p) p.jumping = false;
      }, 100);
    });

    // ===== SIT =====
    this.onMessage("sit", (client, sit) => {
      const p = this.state.players.get(client.sessionId);
      if (!p) return;

      p.anim = sit ? "sit" : "idle";
    });

    // ===== SKIN =====
    this.onMessage("skin", (client, id) => {
      const p = this.state.players.get(client.sessionId);
      if (!p) return;

      p.skin = id;
    });
  }

  // ===== PLAYER JOIN =====
  onJoin(client: Client) {
    console.log(client.sessionId, "joined");

    const player = new Player();
    player.anim = "idle";
    player.skin = 0;

    this.state.players.set(client.sessionId, player);
  }

  // ===== PLAYER LEAVE CLEANUP =====
  onLeave(client: Client, code: number) {
    console.log(client.sessionId, "left", code);
    this.state.players.delete(client.sessionId);
  }
}