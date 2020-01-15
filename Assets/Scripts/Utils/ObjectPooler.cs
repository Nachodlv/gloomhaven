using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

public class ObjectPooler
{
    private List<Pooleable> _objects;
    private int initialQuantity;
    private GameObject parent;
    private Pooleable objectToPool;

    public void InstantiateObjects(int quantity, Pooleable pooleable, string parentName)
    {
        initialQuantity = quantity;
        parent = new GameObject
        {
            name = parentName
        };
        this.objectToPool = pooleable;
        _objects = new List<Pooleable>();
        Grow();
    }

    public Pooleable GetNextObject()
    {
        if (!_objects.Any()) Grow();
    
        var first = _objects.First();
        _objects.RemoveAt(0);
        first.Activate(this);
        return first;
    }

    private void Grow()
    {
        for (var i = 0; i < initialQuantity; i++)
        {
            var newObject = Object.Instantiate(objectToPool, parent.transform, true);
            _objects.Add(newObject);
        }
    }

    public void PooleableDeactivated(Pooleable pooleable)
    {
        _objects.Add(pooleable);
    }
}