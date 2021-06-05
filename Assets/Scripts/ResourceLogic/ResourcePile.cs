using ResourceLogic;
using UnityEngine;

public class ResourcePile : MonoBehaviour {
    public void SetData(ResourceType resource, int amount) {
        ResourceType = resource;
        ResourceAmount = amount;
    }

    public ResourceType ResourceType { get; private set; } = ResourceType.Food;

    public int ResourceAmount { get; private set; } = 20;

    public ResourceType CollectResource(int amount) {
        if (ResourceAmount <= amount) {
            return ResourceType.Empty;
        }

        ResourceAmount -= amount;
        if (ResourceAmount == 0) {
            Destroy(this);
        }

        return ResourceType;
    }
}
