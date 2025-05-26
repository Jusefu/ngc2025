using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeReference]
    public List<IStatusEffect> statusEffects = new List<IStatusEffect>();
}
