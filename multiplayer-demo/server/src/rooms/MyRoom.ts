

import { Room, Client } from "colyseus";
import { GameState } from "../schema/GameState.js";
import { Player } from "../schema/Player.js";

export class MyRoom extends Room {
  maxClients = 4;

  // Recommended v0.17 style: initialize state as class property
  state = new GameState();

  onCreate(options: any) {
    // You can add more initialization logic here if needed
    // (no need to call this.state(...) anymore)

    this.onMessage("input", (client, data) => {

      const player = this.state.players.get(client.sessionId);
      if (!player) return;

      const speed = 5;

      player.x += data.moveX * speed * 0.016;
      player.z += data.moveZ * speed * 0.016;

      player.rotY = data.rotY;

      if (data.jump) {
        player.jumping = true;
        player.anim = "Jump";
      } else {
        player.jumping = false;
        player.anim = "Idle";
      }

      if (data.sit) {
        player.anim = "Sit";
      }

      player.skin = data.skin;
    });

  }

  onJoin(client: Client, options: any) {
    console.log(client.sessionId, "joined!");
    const player = new Player();

    // Ensure your GameState schema has a MapSchema named 'players'
    this.state.players.set(client.sessionId, player);
  }

  onLeave(client: Client, code: number) {
    this.state.players.delete(client.sessionId);
    console.log(client.sessionId, "left with code:", code);
  }
}