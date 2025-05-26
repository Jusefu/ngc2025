using UnityEngine;

public interface IStatusEffect
{
    string GetEffectName();
    void ApplyEffect(GameObject target);
}

