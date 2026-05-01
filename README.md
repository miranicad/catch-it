# catch-it

## Current Implementation Scope Ideas - summarized by AI, partially implemented in Level 0

For the current prototype, we should reduce the original concept to a realistic and testable MVP. The full vision of CATCH IT still includes different environments, mixed reality, different spider realism levels, movement behavior, and more advanced exposure settings. However, for the current deliverable and the remaining project time, the focus should be on demonstrating the core idea clearly rather than implementing every planned feature.

The proposed MVP is a **stationary VR experience** in which the user remains in one indoor environment and catches spiders with hand-based interaction. The user does not move through the world with controllers. Instead, spiders appear at predefined positions in the room, and the user physically looks around, turns, and reaches toward them. This keeps the interaction simple, supports the hand-tracking concept, and avoids additional complexity such as locomotion or teleportation.

### Proposed Level Concept

The prototype should contain **two core levels**, with a possible third level as a stretch goal.

#### Level 1 — Friendly Introduction

The first level introduces the interaction in a low-intensity way.

- Indoor environment
- Small cartoon spider
- Static spider
- One spider active at a time
- User catches a small number of spiders to complete the level
- Panic / emergency option available

This level is meant to help users understand the interaction and feel safe before the spider becomes more realistic.

#### Level 2 — Controlled Realism

The second level increases the exposure slightly while keeping the interaction predictable.

- Same indoor environment
- More realistic spider
- Still static or only very mildly animated
- One spider active at a time
- User catches a slightly higher number of spiders
- Panic / emergency option available

This level mainly increases the visual realism of the spider, while keeping the environment and interaction stable. This should make the progression understandable without overwhelming users.

#### Optional Level 3 — Stronger Exposure

A third level can be added if the first two levels are stable.

- Same indoor environment
- Realistic spider
- Larger spider or slightly closer spawn positions
- Optional slow, predictable movement
- Panic / emergency option available

This level should be treated as optional. It should not block the main prototype if time becomes limited.

### Interaction and Movement Concept

For the prototype, we should avoid artificial player movement. The user should not need controller locomotion or teleportation. Instead, the room and spawn points should be arranged so that the user can interact while mostly standing or sitting in one place.

The basic interaction loop is:

1. A spider appears in the room.
2. The user looks around and locates it.
3. The user reaches toward it with their tracked hand.
4. When the hand touches the spider trigger area, the spider is collected.
5. The next spider appears.
6. After enough spiders are collected, the next level starts or the prototype ends.

This keeps the XR value focused on spatial placement, hand-based interaction, and gradual exposure.

### Planned Basic Project Structure

The implementation should stay simple and prototype-friendly. The current spider spawning logic can be reused and extended.

Suggested script responsibilities:

- **LevelManager**  
  Handles the current level, number of collected spiders, level completion, and switching to the next level.

- **LevelConfig**  
  Stores simple settings per level, such as spider type, spider size, number of spiders to catch, and whether panic mode is enabled.

- **SpiderSpawner**  
  Spawns spiders at predefined spawn points and applies the current level settings.

- **SpiderCollect**  
  Detects when the user’s hand collider touches a spider and notifies the LevelManager.

- **Panic / Emergency Function**  
  Removes or hides active spiders and gives the user a safe break option.

This should be enough structure to keep the code understandable without overengineering the prototype.

### Features We Should Not Prioritize Yet

The following features are still part of the broader vision, but should probably be moved to later development or future work:

- Mixed reality / passthrough mode
- Outside environment
- Controller-based movement
- Complex teleportation
- Gesture-based panic detection
- Spider crawling on the hand
- Advanced spider AI
- Full difficulty editor
- Many different levels

### Goal for Deliverable 3

For Deliverable 3, the goal is not a final polished application. The goal is to show a working first prototype and communicate the concept clearly enough for peer feedback.

A realistic D3 target would be:

- one working indoor environment
- hand-based spider collection
- at least one working level
- ideally a basic second level or clear plan for it
- first version of level progression
- visible explanation of the intended panic/emergency option
- screencast showing the current prototype state

The final concept and exact level structure can still be adjusted after implementation and user feedback.
