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

    [SerializeField] [Tooltip("Prefab that will be instantiate when the ability is fire")]
    protected Pooleable prefab;

    [SerializeField] [Tooltip("Quantity of turns that the character will need to wait to use the ability again")]
    private int cooldown;

    [SerializeField] [Tooltip("The stats that will affect if the ability hits a player")]
    private StatsModifier statsModifier;

    [SerializeField] [Tooltip("The status effects that will be set on the squares that ability hits")]
    private List<StatusEffect> statusEffects;

    [NonSerialized]
    public List<Vector2Int> AreaOfEffect;
    private int currentCooldown;

    public abstract void CastAbility(Vector3 origin, Vector3 destination);
}