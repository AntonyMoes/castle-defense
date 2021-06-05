using System.Collections.Generic;
using System.Linq;
using ResourceLogic;
using UnityEngine;

public class Unit : Entity {
    Dictionary<ResourceType, int> _resources;
    const int ResourceCapacity = 3;

    public float GetInteractionRange { get; } = .5f;
    public int GetHunger { get; } = 1;
    public int GetCost { get; } = 3;

    void Start() {
        _resources = new Dictionary<ResourceType, int>();
    }

    public void CollectResource(ResourcePile pile) {
        var amount = Mathf.Min(pile.ResourceAmount, GetSpace());
        var collectedResourceType = pile.CollectResource(amount);
        _resources.Add(collectedResourceType, amount);
    }

    int GetSpace() => ResourceCapacity - _resources.Values.Sum();

    public void DropResource(Castle castle) {
        castle.AddResources(_resources);
        _resources.Clear();
    }

    //public ResourceType getResource() { return resource; }

    public bool IsFull() {
        return GetSpace() == 0;
    }

    public bool IsEmpty() {
        return GetSpace() == ResourceCapacity;
    }
}
