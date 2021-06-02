using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    Resource resource = Resource.Empty;
    Vector2 destination;
    int hunger = 1;
    int cost = 3;

    public int getHunger()
    {
        return hunger;
    }

    public int getCost()
    {
        return cost;
    }

    public void setDestination(Vector2 destination)
    {
        this.destination = destination;
    }

    public void collectResource(ResourcePile pile)
    {
        resource = pile.getResource();
    }

    public void dropResource(Castle castle)
    {
        castle.addResource(this.resource);
        this.resource = Resource.Empty;
    }
}
