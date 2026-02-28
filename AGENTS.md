# AGENTS.md — Healthy: Neuron Run (Persistent Repo Context)

## Project identity
- Project: **Healthy: Neuron Run**
- Genre: **Calm endless runner**
- Current MVP focus: deliver a clean, replayable run loop with readable controls, mood-based fail state, and clear score feedback.

## Unity version
- Detected from `ProjectSettings/ProjectVersion.txt`:
  - **Unity 6, 6000.3.10f1**
- When adding packages/features, prefer compatibility with this editor version.

## MVP gameplay contract (for future tasks)
- Player auto-runs forward continuously.
- Player can move left/right and jump.
- Include at least one collectible and one hazard.
- Track score (distance + pickups).
- Mood Meter is a core mechanic and primary fail condition.
- Game-over + restart flow should remain quick and readable.

## Source-of-truth content docs
- Mini-GDD lives at: `Docs/MiniGDD_Healthy.md`
- Keep future gameplay tasks aligned with this doc unless explicitly instructed otherwise.

## Folder structure conventions
Primary project folders for new gameplay content:
- `Assets/_Project/Scenes` — project scenes (MVP scene variants, test scenes)
- `Assets/_Project/Scripts` — gameplay/runtime/editor C# scripts
- `Assets/_Project/Prefabs` — reusable prefabs
- `Assets/_Project/UI` — UI prefabs, HUD assets, menu assets
- `Assets/_Project/Art` — sprites, materials, VFX, and other art content

Legacy/default Unity folders may still exist; new feature work should prefer `Assets/_Project/*`.

## C# and naming conventions
- Use **PascalCase** for C# filenames and class names.
- Keep **one class per file** for gameplay/runtime code unless there is a strong Unity-specific reason not to.
- Keep names intention-revealing (e.g., `MoodMeter`, `RunnerScoreController`, `StressSpike`).

## Play-mode expectations
- New gameplay code should be safe to run in Play Mode without requiring manual scene surgery each run.
- Avoid hidden dependencies; expose required references in inspector or initialize clearly in code.
- Keep logs meaningful and avoid noisy per-frame debug logging in committed code.

## Git / repo hygiene
- Maintain a standard Unity `.gitignore`.
- Never commit generated folders like `Library/`, `Temp/`, `Obj/`, `Logs/`, or `UserSettings/`.
