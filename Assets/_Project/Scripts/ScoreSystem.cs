using System;
using UnityEngine;

/// <summary>
/// Applies score points with support for an external multiplier source.
/// </summary>
public class ScoreSystem
{
    private readonly Func<float> multiplierProvider;

    public int Score { get; private set; }

    public ScoreSystem(Func<float> multiplierProvider = null)
    {
        this.multiplierProvider = multiplierProvider;
    }

    public int AddPoints(int basePoints)
    {
        if (basePoints <= 0)
        {
            return Score;
        }

        float multiplier = Mathf.Max(1f, multiplierProvider?.Invoke() ?? 1f);
        int adjustedPoints = Mathf.Max(1, Mathf.RoundToInt(basePoints * multiplier));

        Score += adjustedPoints;
        return Score;
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
