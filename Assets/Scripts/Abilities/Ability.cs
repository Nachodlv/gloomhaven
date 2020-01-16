using System;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

// This class in MonoBehaviour because of unity lack of serialization when using polymorphism.
public abstract class Ability : MonoBehaviour
{
    [Tooltip("Cost in mana that the ability costs")]
    public int cost;

    [Tooltip("Range of the ability in squares")]
    public int range;

    [SerializeField] [Tooltip("Quantity of turns that the character will need to wait to use the ability again")]
    private int cooldown;

    [SerializeField] [Tooltip("The stats that will affect if the ability hits a player")]
    private StatsModifier statsModifier;

    [SerializeField] [Tooltip("The status effects that will be set on the squares that ability hits")]
    private List<StatusEffect> statusEffects;

    [SerializeField] private AbilityType abilityType;

    [NonSerialized] public List<Vector2Int> AreaOfEffect;

    private int currentCooldown;
    protected ObjectPooler objectPooler;

    /// <summary>
    ///     <para>Cast the ability in the given origin directed to its given destination</para>
    /// </summary>
    /// <param name="origin">The origin of the ability</param>
    /// <param name="destination">The destination of the ability</param>
    public abstract void CastAbility(Vector3 origin, Vector3 destination);

    protected virtual void Awake()
    {
        var poolerProvider = FindObjectOfType<PoolerProvider>();
        if(poolerProvider == null) Debug.LogError("No pooler provider found");
        else objectPooler = poolerProvider.GetPooler(abilityType);
    }
}