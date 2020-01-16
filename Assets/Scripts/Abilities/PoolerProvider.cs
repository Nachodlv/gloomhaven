using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;

[Serializable]
public class PoolerProvided
{
    [Tooltip("The ability type of the prefab")]
    public AbilityType abilityType;

    [Tooltip("The prefab that will be used to instantiates the game objects")]
    public Pooleable prefab;

    [Tooltip("Quantity of prefabs that will be instantiated")]
    public int quantity;

    [NonSerialized] public ObjectPooler objectPooler = new ObjectPooler();
}

public class PoolerProvider : MonoBehaviour
{
    [SerializeField] [Tooltip("Object poolers provided by the pooler provided")]
    private List<PoolerProvided> poolers;

    private void Awake()
    {
        InstantiatePoolers();
    }

    /// <summary>
    /// Give the <paramref name="abilityType"/> returns the ObjectPooler that corresponds.
    /// If no ObjectPooler is found with that <paramref name="abilityType"/> then it will return null.
    /// </summary>
    /// <param name="abilityType">The abilityType that the ObjectPooler needs to have to be returned</param>
    /// <returns>The ObjectPooler with the give <paramref name="abilityType"/></returns>
    public ObjectPooler GetPooler(AbilityType abilityType)
    {
        foreach (var pooler in poolers.Where(pooler => pooler.abilityType == abilityType))
        {
            return pooler.objectPooler;
        }
        Debug.LogError("No pooler found for the give ability type");
        return null;
    }

    /// <summary>
    /// Instantiates the prefabs
    /// </summary>
    private void InstantiatePoolers()
    {
        poolers.ForEach(pooler =>
        {
            pooler.objectPooler.InstantiateObjects(pooler.quantity, pooler.prefab, pooler.prefab.name);
        });
    }
}