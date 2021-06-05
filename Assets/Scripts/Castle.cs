using System.Collections.Generic;
using ResourceLogic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour {
    public Text foodCount;
    Dictionary<ResourceType, Text> _textFields;

    readonly Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();
    const int Hunger = 0;
    EntityManager _entityManager;

    // Start is called before the first frame update
    void Start() {
        _entityManager = new EntityManager();

        InvokeRepeating(nameof(ConsumeFood), 1.0f, 4f);

        //stuff for testing
        SetResourceCount(ResourceType.Food, 1000);
        SetResourceCount(ResourceType.Metal, 100);
    }

    void ChangeResourceCount(ResourceType resource, int change) {
        _resources[resource] += change;
        //textFields[resource].text = resources[resource].ToString();
    }

    void SetResourceCount(ResourceType resource, int newCount) {
        _resources[resource] = newCount;
        //textFields[resource].text = resources[resource].ToString();
    }

    int GetResourceCount(ResourceType resource) {
        return _resources[resource];
    }

    public void AddResources(Dictionary<ResourceType, int> resources) {
        foreach (var pair in resources) {
            _resources[pair.Key] += pair.Value;
        }
    }

    void ConsumeFood() {
        Eat(Hunger);
    }

    bool Eat(int food) {
        if (_resources[(ResourceType.Food)] < food) {
            return false;
        }

        ChangeResourceCount(ResourceType.Food, -food);
        return true;
    }
}
