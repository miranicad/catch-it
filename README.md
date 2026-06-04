# 🕷️ catch-it

> A VR-based exposure therapy app to help people overcome their fear of spiders.

---

## About

catch-it is an XR application built for the **Meta Quest** using **hand tracking** (no controllers). Players must physically "touch" and collect virtual spiders. The difficulty increases gradually, guiding users from cartoonish spiders all the way to realistic close-contact interaction.

---

## Core Features

- 🖐 **Hand Tracking** — natural interaction, no controllers required
- 🎮 **Multiple Exposure Levels** — From Cartoon to Realistic to Scary

---

## Tech Stack

| Component | Technology |
|---|---|
| Engine | Unity 6 LTS |
| Headset | Meta Quest 3 |
| Interaction | Meta Hand Tracking SDK |
| Platform | Android (APK sideloaded via Meta Developer Hub) |

---

## Getting Started

```bash
git clone https://github.com/miranicad/catch-it.git
```

1. Open the project in **Unity Hub** (Unity 6 (specifically 6000.3.13f1))
2. Install the **Meta XR SDK** via Unity Package Manager
3. Enable **Developer Mode** on your Meta Quest headset
4. Build & deploy via **Meta Quest Developer Hub** or Unity's build settings

---

## Project Structure

```
catch-it/
├── Assets/
│   ├── Scenes/               # game scene (Catch It in Daylight)
│      └── Testing Levels/    # old game scenes from development & testing (Menu, Level 0–2)
│   ├── Scripts/              # Game logic & hand interaction
│      └── Testing Scripts/   # old scripts from development & testing; evolutions of these in parent folder
│   ├── Prefabs/              # Spider models; new prefabs made from imported assets with animation controllers where relevant
└── README.md
```

---

## Exposure Levels

| Level | Spider Type | Interaction |
|---|---|---|
| 1 | Fantasy | Touch to collect |
| 2 | Cartoon | Touch to collect |
| 3 | Realistic | Touch to collect |
| 4 | Scary | Touch to collect |

---

## Team

Saskia Bosshard: <saskia.bosshard@students.fhnw.ch>

Tamira Leber: <tamira.leber@students.fhnw.ch>

Nadine Zbinden: <nadine.zbinden@students.fhnw.ch>

Developed as part of the **exr course at FHNW** (2026) by a team of 3 students.

---
