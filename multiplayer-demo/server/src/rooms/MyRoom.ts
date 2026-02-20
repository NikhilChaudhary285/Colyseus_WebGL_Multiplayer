

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

    this.onMessage("move", (client, data) => {
      let p = this.state.players.get(client.sessionId);
      if (!p) return;

      p.x = data.x;
      p.y = data.y;
      p.z = data.z;
      p.rotY = data.rotY;
      p.anim = data.anim;
    });

    this.onMessage("jump", (client) => {
      let p = this.state.players.get(client.sessionId);
      if (p) p.jumping = true;
    });

    this.onMessage("sit", (client, sit) => {
      let p = this.state.players.get(client.sessionId);
      if (p) p.sitting = sit;
    });

    this.onMessage("skin", (client, id) => {
      let p = this.state.players.get(client.sessionId);
      if (p) p.skin = id;
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