using UnityEngine;


[System.Serializable]
public class PoisonEffect : IStatusEffect
{
    public float damagePerSecond = 5f;
    public float duration = 3f;

    public string GetEffectName() => "Poison";

    public void ApplyEffect(GameObject target)
    {
        // Implementation
        Debug.Log($"Applying poison: {damagePerSecond} damage for {duration} seconds");
    }
}