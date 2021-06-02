using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public Text foodCount;
    float x;
    float y;

    int[] resources;
    List<Unit> units;
    int hunger = 3;

    // Start is called before the first frame update
    void Start()
    {
        x = this.transform.localPosition.x;
        y = this.transform.localPosition.y;
        resources = new int[2];
        setResourceCount(Resource.Food,1000);
        setResourceCount(Resource.Metal, 100);

        InvokeRepeating("SpamUnits", 1.0f, 5f);
        InvokeRepeating("SpamUnits", 1.0f, 4f);
    }

    void changeResourceCount(Resource resource, int change)
    {
        resources[((int)resource) - 1] += change;
        foodCount.text = resources[((int)Resource.Food) - 1].ToString();
    }
    void setResourceCount(Resource resource, int newCount)
    {
        resources[((int)resource) - 1] = newCount;
        foodCount.text = resources[((int)Resource.Food) - 1].ToString();
    }

    int getResourceCount(Resource resource)
    {
        return resources[((int)resource) - 1];
    }

    void SpamUnits()
    {
        Eat(-5);
    }

    void ConsumeFood()
    {
        Eat(-hunger);
    }

    bool Eat(int food)
    {
        if (resources[((int)Resource.Food)] >= food)
        {
            changeResourceCount(Resource.Food, food);
            return true;
        }
        return false;
    }

    public void Spawn(GameObject unitPrefab)
    {
        Vector2 spawnPoint = new Vector2(x, y);
        Instantiate(unitPrefab, spawnPoint, Quaternion.identity);
    }
}
