# 🕷️ catch-it

> A VR-based exposure therapy app to help people overcome their fear of spiders.

---

## About

catch-it is an XR application built for the **Meta Quest** using **hand tracking** (no controllers). Players must physically "touch" and collect virtual spiders. The difficulty increases gradually, guiding users from cartoonish spiders all the way to realistic close-contact interaction.

---

## Core Features

- 🖐 **Hand Tracking** — natural interaction, no controllers required
- 🎮 **2 Exposure Levels** — Cartoon → Realistic

---

## Tech Stack

| Component | Technology |
|---|---|
| Engine | Unity 2022 LTS |
| Headset | Meta Quest 3 |
| Interaction | Meta Hand Tracking SDK |
| Platform | Android (APK sideloaded via Meta Developer Hub) |

---

## Getting Started

```bash
git clone https://github.com/miranicad/catch-it.git
```

1. Open the project in **Unity Hub** (Unity 2022 LTS recommended)
2. Install the **Meta XR SDK** via Unity Package Manager
3. Enable **Developer Mode** on your Meta Quest headset
4. Build & deploy via **Meta Quest Developer Hub** or Unity's build settings

---

## Project Structure

```
ArachnoVR/
├── Assets/
│   ├── Scenes/         # Main game scenes (Menu, Level 1–3)
│   ├── Scripts/        # Game logic & hand interaction
│   ├── Prefabs/        # Spider models & environment objects
├── Packages/           # Unity package dependencies
└── README.md
```

---

## Exposure Levels

| Level | Spider Type | Interaction |
|---|---|---|
| 1 | Cartoon / stylized | Touch to collect |
| 2 | Realistic, moving | Touch to collect |

---

## Team
Saskia Bosshard: saskia.bosshard@students.fhnw.ch 

Tamira Leber: tamira.leber@students.fhnw.ch 

Nadine Zbinden: nadine.zbinden@students.fhnw.ch 

Developed as part of the **exr course at FHNW** (2026) by a team of 3 students. 
 
---
