# Healthy: Neuron Run — Mini GDD (MVP)

## Vision
**Healthy: Neuron Run** is a calm endless runner where the player glides forward through a stylized neural pathway, avoiding stress-like hazards and collecting wellness signals to maintain momentum and mood.

## MVP Scope
Build a polished vertical slice that proves the full core loop:
- One playable endless lane/path with forward auto-movement.
- Left/right movement and jump controls.
- At least 1 collectible type and 1 hazard type.
- Score tracking with increasing run distance.
- A visible Mood Meter that rises/falls based on play.
- Game over and restart flow.

## Target Feel
- Smooth, low-stress, readable gameplay.
- Clean UI, soft feedback, no harsh punishment spikes.
- Session length target: 1–3 minutes for early MVP runs.

## Core Loop
1. Player auto-runs forward.
2. Player steers and jumps to line up with collectibles while avoiding hazards.
3. Collectibles increase score and stabilize/increase mood.
4. Hazards reduce mood and may disrupt score pacing.
5. Difficulty gradually scales by speed/spawn density.
6. Run ends when Mood Meter reaches zero (or equivalent fail condition).
7. Show results and allow immediate restart.

## Controls (PC MVP)
- **A / Left Arrow:** Move left
- **D / Right Arrow:** Move right
- **Space:** Jump
- *(Optional mirror)* **W / Up Arrow:** Jump

## Player + World Rules
- Camera follows player with stable framing.
- Forward movement is constant and readable.
- Lane-switch or smooth horizontal movement is acceptable for MVP.
- Hazards/collectibles spawn ahead of player with simple randomization and safe constraints.

## Collectible
- **Name (working):** Calm Pulse
- **Function:** Increases score and mood slightly.
- **Feedback:** Soft VFX/SFX + small UI pop.

## Hazard
- **Name (working):** Stress Spike
- **Function:** Reduces mood on contact.
- **Feedback:** Clear hit reaction + mood drop indicator.

## Scoring
- Score is driven by:
  - Distance survived (passive increase over time), and
  - Collectible pickups (bonus points).
- End-of-run screen shows total score and distance.

## Mood Meter (Primary Failure System)
- Range: 0–100 (or normalized equivalent).
- Starts at a healthy baseline.
- Increases slightly from collectibles / stable play.
- Decreases from hazard hits (and optionally over time at higher difficulty).
- **Game over** when mood reaches 0.

## UI (MVP)
- HUD elements:
  - Score
  - Distance
  - Mood Meter
- Simple start prompt and game-over panel with restart button/key.

## Out of Scope (for MVP)
- Narrative campaign
- Multiple characters
- Advanced procedural generation biomes
- Save/profile/meta progression
