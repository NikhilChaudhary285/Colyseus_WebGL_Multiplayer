import { Schema, type } from "@colyseus/schema";

export class Player extends Schema {

  @type("number") x = 0;
  @type("number") y = 0;
  @type("number") z = 0;

  @type("number") rotY = 0;

  @type("boolean") jumping = false;
  @type("string") anim = "Idle";

  @type("number") skin = 0;
}
