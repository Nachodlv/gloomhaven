using System;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

// This class in MonoBehaviour because of unity lack of serialization when using polymorphism.
[RequireComponent(typeof(AbilityUI))]
public abstract class Ability : MonoBehaviour
{
    [SerializeField] [Tooltip("Cost in mana that the ability costs")]
    private int cost;

    public int Cost => cost;

    [SerializeField] [Tooltip("Range of the ability in squares")]
    private int range;

    public int Range => range;

    [SerializeField] [Tooltip("The stats that will affect if the ability hits a player")]
    private Stats statsModifier;

    public Stats StatsModifier => statsModifier;

    [SerializeField] [Tooltip("Quantity of turns that the character will need to wait to use the ability again")]
    private int cooldown;


    [Tooltip("The status effects that will be set on the squares that ability hits")]
    public StatusEffect[] statusEffects;

    [SerializeField] [Tooltip("It is used for getting the corresponding prefab")]
    private AbilityType abilityType;

    [NonSerialized] public List<Vector2Int> AreaOfEffect;
    [NonSerialized] public AbilityUI AbilityUi;
    [NonSerialized] public int CurrentCooldown;
    protected ObjectPooler ObjectPooler;

    /// <summary>
    ///     <para>Cast the ability in the given origin directed to its given destination</para>
    /// </summary>
    /// <param name="origin">The origin of the ability</param>
    /// <param name="destination">The destination of the ability</param>
    public virtual void CastAbility(Vector3 origin, Vector3 destination, Action finishCasting)
    {
        CurrentCooldown = cooldown;
    }

    protected virtual void Awake()
    {
        CurrentCooldown = 0;
        AbilityUi = GetComponent<AbilityUI>();

        GetProjectile();
    }

    protected virtual void GetProjectile()
    {
        var poolerProvider = FindObjectOfType<PoolerProvider>();
        if (poolerProvider == null) Debug.LogError("No pooler provider found");
        else ObjectPooler = poolerProvider.GetPooler(abilityType);
    }
}