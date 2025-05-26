using UnityEngine;

[System.Serializable]
public class SlowEffect : IStatusEffect
{
    public float slowPercent = 30f;
    public float duration = 2f;

    public string GetEffectName() => "Slow";
    public void ApplyEffect(GameObject target)
    {
        Debug.Log($"Applying Slow: Slowing {slowPercent}% for {duration} seconds");
    }

}
