using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Empty,
    Food,
    Metal
}

[SerializeField]
public struct Resource
{
    public Resource(ResourceType resourceType, int weight)
    {
        this.ResourceType = resourceType;
        this.Weight = weight;
    }
    public ResourceType ResourceType { get; }
    public int Weight { get; }
}

public class ResourcePile : MonoBehaviour
{
    private ResourceType resourceType = ResourceType.Food;
    private int amount = 20;

    public void setData(ResourceType resource, int amount)
    {
        this.resourceType = resource;
        this.amount = amount;
    }

    public ResourceType getResourceType() { return resourceType; }
    public int getResourceAmount() { return amount; }

    public ResourceType collectResource(int amount)
    {
        if (this.amount > amount)
        {
            this.amount -= amount;
            if (this.amount == 0)
                GameObject.Destroy(this);
            return this.resourceType;
        }
        return ResourceType.Empty;
    }
}
