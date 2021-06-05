using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Entity
{
    Dictionary<ResourceType, int> resources;
    int hunger = 1;
    int cost = 3;
    int resourceCapacity = 3;

    int resourceRange;

    public int getResourceRange() { return resourceRange; }

    private void Start()
    {
        resources = new Dictionary<ResourceType, int>();
    }

    public int getHunger() { return hunger; }

    public int getCost() { return cost; }

    public void collectResource(ResourcePile pile)
    {
        int amount = Mathf.Min(pile.getResourceAmount(), getSpace());
        ResourceType resource = pile.collectResource(amount);
        resources.Add(resource, amount);
    }

    int getSpace() { return resourceCapacity - resources.Count; }

    public void dropResource(Castle castle)
    {
        castle.AddResources(this.resources);
        this.resources.Clear();
    }

    //public ResourceType getResource() { return resource; }

    public bool hasResource() { return false; }
}
