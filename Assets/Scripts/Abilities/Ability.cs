using System;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

// This class in MonoBehaviour because of unity lack of serialization when using polymorphism.
public abstract class Ability: MonoBehaviour
{
    [SerializeField] [Tooltip("Prefab that will be instantiate when the ability is fire")]
    protected Pooleable prefab;
    
    [SerializeField] [Tooltip("Quantity of turns that the character will need to wait to use the ability again")]
    private int cooldown;

    [SerializeField] [Tooltip("Range of the ability in squares")]
    private int range;

    [SerializeField] [Tooltip("The stats that will affect if the ability hits a player")]
    private StatsModifier statsModifier;

    [SerializeField] [Tooltip("The status effects that will be set on the squares that ability hits")]
    private List<StatusEffect> statusEffects;

    protected List<Vector2Int> areaOfEffect;
    private int currentCooldown;

    public abstract void MakeAbility(Vector3 origin, Vector3 destination);
}