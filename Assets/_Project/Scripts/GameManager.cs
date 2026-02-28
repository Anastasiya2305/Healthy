using System;
using UnityEngine;

/// <summary>
/// Minimal game state coordinator for the runner MVP.
/// Setup:
/// 1) Add this component to an empty GameObject in your scene (for example: "GameManager").
/// 2) Optional: wire UI/audio listeners to OnStateChanged.
/// 3) RunnerController can optionally call SetDead() when fail conditions are added.
/// </summary>
public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Boot,
        Running,
        Dead,
        Paused
    }

    public static GameManager Instance { get; private set; }

    [SerializeField] private bool autoStartRunOnAwake = true;

    public GameState CurrentState { get; private set; } = GameState.Boot;
    public ScoreSystem ScoreSystem { get; private set; }
    public ComboSystem ComboSystem { get; private set; }

    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ComboSystem = new ComboSystem();
        ScoreSystem = new ScoreSystem(() => ComboSystem.Multiplier);

        SetState(GameState.Boot);

        if (autoStartRunOnAwake)
        {
            StartRun();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void StartRun()
    {
        ScoreSystem.ResetScore();
        ComboSystem.ResetCombo();

        SetState(GameState.Running);
        Time.timeScale = 1f;
    }

    public void SetDead()
    {
        ResetCombo();
        SetState(GameState.Dead);
    }

    public int AddScore(int basePoints)
    {
        return ScoreSystem.AddPoints(basePoints);
    }

    public void IncrementCombo()
    {
        ComboSystem.IncrementCombo();
    }

    public void ResetCombo()
    {
        ComboSystem.ResetCombo();
    }

    public void Pause()
    {
        SetState(GameState.Paused);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        SetState(GameState.Running);
    }

    public bool IsGameplayActive()
    {
        return CurrentState == GameState.Running;
    }

    private void SetState(GameState next)
    {
        if (CurrentState == next)
        {
            return;
        }

        CurrentState = next;
        OnStateChanged?.Invoke(CurrentState);
    }
}
