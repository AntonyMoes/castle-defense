using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public Text foodCount;
    [SerializeField]
    Dictionary<ResourceType, Text> textFields;

    Dictionary<ResourceType, int> resources;
    int hunger = 0;
    EntityManager unitManager;

    // Start is called before the first frame update
    void Start()
    {
        unitManager = new EntityManager();
        resources = new Dictionary<ResourceType, int>();

        InvokeRepeating("ConsumeFood", 1.0f, 4f);

        //stuff for testing
        SetResourceCount(ResourceType.Food, 1000);
        SetResourceCount(ResourceType.Metal, 100);
    }

    void ChangeResourceCount(ResourceType resource, int change)
    {
        resources[resource] += change;
        //textFields[resource].text = resources[resource].ToString();
    }
    void SetResourceCount(ResourceType resource, int newCount)
    {
        resources[resource] = newCount;
        //textFields[resource].text = resources[resource].ToString();
    }

    int GetResourceCount(ResourceType resource)
    {
        return resources[resource];
    }

    public void AddResources(Dictionary<ResourceType, int> resources)
    {
    }

    void ConsumeFood()
    {
        Eat(hunger);
    }

    bool Eat(int food)
    {
        if (resources[(ResourceType.Food)] >= food)
        {
            ChangeResourceCount(ResourceType.Food, -food);
            return true;
        }
        return false;
    }
}
