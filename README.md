# 🎮 Unity WebGL Multiplayer Prototype (Colyseus)

A multiplayer prototype built with Unity WebGL and Colyseus, focused on exploring server-authoritative architecture and real-time synchronization in browser-based games.

> ⚠️ **This is a technical prototype, not a full production game.**
> The goal of this project is to experiment with multiplayer networking concepts and compare Colyseus with systems like Photon and Socket.IO, which I have previously worked with in production.

---

## 🚀 Live Demo

👉 **[colyseus-multiplayer.netlify.app](https://colyseus-multiplayer.netlify.app/)**

⚠️ The backend runs on a free-tier server and may take **30–50 seconds** to wake up on first load. If the game isn't running, email me at nikhilchaudhary285@gmail.com and I'll restart the server.

---

## 🧠 Project Goal

This project was built to:
- Explore server-authoritative multiplayer architecture
- Understand real-time state synchronization across clients
- Experiment with WebGL multiplayer in the browser
- Compare Colyseus vs Photon vs Socket.IO networking approaches

---

## 🎮 Features

- ✅ Create / Join rooms using unique room codes
- ✅ Real-time multiplayer gameplay in browser (WebGL)
- ✅ Server-authoritative movement and gameplay logic
- ✅ Player state synchronization across clients
- ✅ Animation sync (walk, jump, sit)
- ✅ Skin switching synchronized across players
- ✅ Host-controlled match start system
- ✅ Player join, leave, and disconnect handling

---

## ⚙️ Tech Stack

**Client**
- Unity (WebGL)
- C#

**Server**
- Colyseus (Node.js)
- WebSockets

**Deployment**
- Frontend: Netlify
- Backend: Render

---

## 🏗 Architecture Overview

This project follows a server-authoritative multiplayer model:
- The server controls the game state
- Clients send input/events
- The server validates and processes inputs
- Updated state is broadcast to all connected clients

**Flow:**
1. Player connects to the server
2. Player joins or creates a room
3. Client sends input (movement, actions)
4. Server processes input and updates state
5. All clients receive synchronized updates

---

## 🔧 Key Technical Learnings

- Handling real-time state synchronization
- Managing client-server communication via WebSockets
- Solving animation synchronization issues
- Preventing state drift and inconsistencies
- Managing player lifecycle (join/leave/disconnect)

---

## 🧪 Challenges Faced

- Animation desynchronization between client and server
- Movement inconsistencies across clients
- Server patch rate tuning
- Handling delays due to free-tier backend hosting

---

## 🎮 Controls

| Action | Key |
|---|---|
| Move | WASD |
| Jump | Space |
| Sit | C |
| Change Skin | 1–4 |

---

## 📹 Demo Video

👉 [Watch on Google Drive](https://drive.google.com/file/d/1DuBpsr_5JBLUTJuovOrSIuQBQQidUlVe/view?usp=sharing)

---

## 📂 Repositories

- **Unity Client:** [Colyseus_WebGL_Multiplayer](https://github.com/NikhilChaudhary285/Colyseus_WebGL_Multiplayer)
- **Colyseus Server:** [Colyseus_WebGL_Server](https://github.com/NikhilChaudhary285/Colyseus_WebGL_Server)

---

## 🔄 Comparison with Photon and Socket.IO

I have previously worked with Photon and Socket.IO for multiplayer systems in production. This project helped me understand:
- Differences between client-authoritative vs server-authoritative models
- Flexibility of building a custom backend using Colyseus
- Greater control over game state and networking flow

---

## 🤝 Contribution

This is a personal learning project, but feedback and suggestions are always welcome!

---

## 👨‍💻 Author

**Nikhil Chaudhary**
Unity Multiplayer Developer

---

## ⭐ If you found this useful

Feel free to star the repository and connect with me!
