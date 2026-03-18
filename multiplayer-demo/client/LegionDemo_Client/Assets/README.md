# \# 🎮 Unity WebGL Multiplayer Prototype (Colyseus)

# 

# A \*\*multiplayer prototype built with Unity WebGL and Colyseus\*\*, focused on exploring \*\*server-authoritative architecture and real-time synchronization\*\* in browser-based games.

# 

# > ⚠️ This is a \*\*technical prototype\*\*, not a full production game.  

# > The goal of this project is to experiment with multiplayer networking concepts and architecture using Colyseus as i already worked with Photon And Socket.IO multiplayer networking systems and architecture just for comparison.

# 

# \---

# 

# \## 🚀 Live Demo

# 

# 👉 https://colyseus-multiplayer.netlify.app/

# 

# > Note: The backend runs on a free-tier server and may take a few seconds to wake up and if you want to play that game please mail me at: (nikhilchaudhary285@gmail.com). So, I can restart or run the server so anyone can enjoy the game to play.

# 

# \---

# 

# \## 🧠 Project Goal

# 

# This project was built to:

# 

# \- Explore \*\*server-authoritative multiplayer architecture\*\*

# \- Understand \*\*state synchronization across clients\*\*

# \- Experiment with \*\*WebGL multiplayer in browser\*\*

# \- Learn and compare \*\*Colyseus vs Photon and Socket.IO networking approaches\*\*

# 

# \---

# 

# \## 🎮 Features

# 

# \- ✅ Create / Join Room (unique room codes)

# \- ✅ Real-time multiplayer in browser (WebGL)

# \- ✅ Server-authoritative movement \& gameplay logic

# \- ✅ Player synchronization across multiple clients

# \- ✅ Animation sync (walk, jump, sit)

# \- ✅ Skin switching synced across players

# \- ✅ Host-controlled match start system

# \- ✅ Player join / leave / despawn handling

# 

# \---

# 

# \## ⚙️ Tech Stack

# 

# \### Client

# \- Unity (WebGL)

# \- C#

# 

# \### Server

# \- Colyseus (Node.js)

# \- WebSockets

# 

# \### Deployment

# \- Frontend: Netlify

# \- Backend: Render

# 

# \---

# 

# \## 🏗 Architecture Overview

# 

# This project follows a \*\*server-authoritative multiplayer model\*\*:

# 

# \- The \*\*server controls game state\*\*

# \- Clients send \*\*input/events\*\*

# \- Server validates and updates state

# \- State is \*\*broadcast to all connected clients\*\*

# 

# \### Flow:

# 

# 1\. Player connects to server

# 2\. Joins or creates a room

# 3\. Sends input (movement, actions)

# 4\. Server processes and updates state

# 5\. All clients receive synchronized updates

# 

# \---

# 

# \## 🔧 Key Technical Learnings

# 

# \- Handling \*\*real-time state synchronization\*\*

# \- Managing \*\*client-server communication using WebSockets\*\*

# \- Solving \*\*animation sync issues across network\*\*

# \- Preventing \*\*state drift and inconsistent updates\*\*

# \- Managing \*\*player lifecycle (join/leave/disconnect)\*\*

# 

# \---

# 

# \## 🧪 Challenges Faced

# 

# \- Animation desync between client \& server

# \- Movement inconsistency across clients

# \- Server patch rate tuning

# \- Handling idle server delays (free-tier hosting)

# 

# \---

# 

# \## 🎮 Controls

# 

# | Action        | Key        |

# |--------------|-----------|

# | Move         | WASD      |

# | Jump         | Space     |

# | Sit          | C         |

# | Change Skin  | 1 – 4     |

# 

# \---

# 

# \## 📹 Demo Video

# 

# 👉 (Google Drive Link): https://drive.google.com/file/d/1DuBpsr\_5JBLUTJuovOrSIuQBQQidUlVe/view?usp=sharing

# 

# \---

# 

# \## 📂 Repositories

# 

# \### Unity Client

# 👉 https://github.com/NikhilChaudhary285/Colyseus\_WebGL\_Multiplayer

# 

# \### Colyseus Server

# 👉 https://github.com/NikhilChaudhary285/Colyseus\_WebGL\_Server

# 

# \---

# 

# \## 🔄 Comparison with Photon

# 

# Previously, I have worked with Photon and Socket.IO for multiplayer systems.

# 

# This project helped me understand:

# 

# \- Differences between \*\*client-authoritative vs server-authoritative models\*\*

# \- Flexibility of \*\*custom backend with Colyseus\*\*

# \- Deeper control over \*\*game state and networking flow\*\*

# 

# \---

# 

# \---

# 

# \## 🤝 Contribution

# 

# This is a personal learning project, but feedback and suggestions are always welcome!

# 

# \---

# 

# \## 👨‍💻 Author

# 

# \*\*Nikhil Chaudhary\*\*  

# Unity Multiplayer Developer  

# 

# \---

# 

# \## ⭐ If you found this useful

# 

# Feel free to star the repo ⭐ and connect with me!

