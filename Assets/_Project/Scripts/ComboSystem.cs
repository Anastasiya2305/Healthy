using UnityEngine;

/// <summary>
/// Tracks combo streak count and computes a clamped score multiplier.
/// </summary>
public class ComboSystem
{
    private readonly float comboStep;
    private readonly float maxMultiplier;

    public int Combo { get; private set; }

    public float Multiplier => Mathf.Clamp(1f + (Combo * comboStep), 1f, maxMultiplier);

    public ComboSystem(float comboStep = 0.1f, float maxMultiplier = 3f)
    {
        this.comboStep = Mathf.Max(0f, comboStep);
        this.maxMultiplier = Mathf.Max(1f, maxMultiplier);
    }

    public void IncrementCombo()
    {
        Combo++;
    }

    public void ResetCombo()
    {
        Combo = 0;
    }
}
