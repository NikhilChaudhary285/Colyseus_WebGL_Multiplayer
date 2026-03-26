🎮 Unity WebGL Multiplayer Prototype (Colyseus)

A multiplayer prototype built with Unity WebGL and Colyseus, focused on exploring server-authoritative architecture and real-time synchronization in browser-based games.<br><br>
⚠️ This is a technical prototype, not a full production game.
The goal of this project is to experiment with multiplayer networking concepts and compare Colyseus with systems like Photon and Socket.IO, which I have previously worked with.

----------------------

🚀 Live Demo

👉 https://colyseus-multiplayer.netlify.app/ <br><br>

⚠️ Note: The backend runs on a free-tier server and may take a few seconds to wake up.
If the game is not running, feel free to contact me at nikhilchaudhary285@gmail.com, and I can restart the server.

----------------------

🧠 Project Goal

This project was built to:<br>
• Explore server-authoritative multiplayer architecture<br>
• Understand real-time state synchronization across clients<br>
• Experiment with WebGL multiplayer in the browser<br>
• Compare Colyseus vs Photon vs Socket.IO networking approaches<br>

----------------------

🎮 Features<br>

• ✅ Create / Join rooms using unique room codes<br>
• ✅ Real-time multiplayer gameplay in browser (WebGL)<br>
• ✅ Server-authoritative movement and gameplay logic<br>
• ✅ Player state synchronization across clients<br>
• ✅ Animation sync (walk, jump, sit)<br>
• ✅ Skin switching synchronized across players<br>
• ✅ Host-controlled match start system<br>
• ✅ Player join, leave, and disconnect handling<br>

----------------------

⚙️ Tech Stack

Client<br>
• Unity (WebGL)<br>
• C#<br>

Server<br>
• Colyseus (Node.js)<br>
• WebSockets<br>

Deployment<br>
• Frontend: Netlify<br>
• Backend: Render<br>

----------------------

🏗 Architecture Overview

This project follows a server-authoritative multiplayer model:<br>
• The server controls the game state<br>
• Clients send input/events<br>
• The server validates and processes inputs<br>
• Updated state is broadcast to all connected clients<br>

Flow:

1. Player connects to the server
2. Player joins or creates a room
3. Client sends input (movement, actions)
4. Server processes input and updates state
5. All clients receive synchronized updates

----------------------

🔧 Key Technical Learnings<br><br>
• Handling real-time state synchronization<br>
• Managing client-server communication via WebSockets<br>
• Solving animation synchronization issues<br>
• Preventing state drift and inconsistencies<br>
• Managing player lifecycle (join/leave/disconnect)<br>

----------------------

🧪 Challenges Faced<br><br>
• Animation desynchronization between client and server<br>
• Movement inconsistencies across clients<br>
• Server patch rate tuning<br>
• Handling delays due to free-tier backend hosting<br>

----------------------

🎮 Controls<br><br>
Action	      || Key<br>
• Move	      || WASD<br>
• Jump	      || Space<br>
• Sit	        || C<br>
• Change Skin	|| 1 – 4<br>

----------------------

📹 Demo Video

👉 https://drive.google.com/file/d/1DuBpsr_5JBLUTJuovOrSIuQBQQidUlVe/view?usp=sharing

----------------------

📂 Repositories

• Unity Client
👉 https://github.com/NikhilChaudhary285/Colyseus_WebGL_Multiplayer

• Colyseus Server
👉 https://github.com/NikhilChaudhary285/Colyseus_WebGL_Server

----------------------

🔄 Comparison with Photon and Socket.IO

I have previously worked with Photon and Socket.IO for multiplayer systems.

This project helped me understand:<br>
• Differences between client-authoritative vs server-authoritative models<br>
• Flexibility of building a custom backend using Colyseus<br>
• Greater control over game state and networking flow<br>

----------------------

🤝 Contribution<br> This is a personal learning project, but feedback and suggestions are always welcome!

----------------------

👨‍💻 Author<br>
Nikhil Chaudhary<br>
Unity Multiplayer Developer

----------------------

⭐ If you found this useful

Feel free to ⭐ star the repository and connect with me!

----------------------
