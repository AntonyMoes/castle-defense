using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resource
{
    Empty,
    Food,
    Metal
}

public class ResourcePile : MonoBehaviour
{
    private Resource resourceType = Resource.Food;
    private int capacity = 20;

    public void setData(Resource resource, int capacity)
    {
        this.resourceType = resource;
        this.capacity = capacity;
    }

    private void displayResourceInfo()
    {
        
    }

    public Resource getResource()
    {
        capacity--;
        return this.resourceType;
    }
}
