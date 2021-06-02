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
    List<Unit> units = new List<Unit>();
    int hunger = 0;

    // Start is called before the first frame update
    void Start()
    {
        x = this.transform.localPosition.x;
        y = this.transform.localPosition.y;
        resources = new int[2];

        InvokeRepeating("ConsumeFood", 1.0f, 4f);

        //stuff for testing
        setResourceCount(Resource.Food, 1000);
        setResourceCount(Resource.Metal, 100);
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

    public void addResource(Resource resource)
    {
        changeResourceCount(resource, 1);
    }

    void ConsumeFood()
    {
        Eat(hunger);
    }

    bool Eat(int food)
    {
        if (resources[((int)Resource.Food)] >= food)
        {
            changeResourceCount(Resource.Food, -food);
            return true;
        }
        return false;
    }

    public void HireUnit(GameObject unitObj)
    {
        Vector2 spawnPoint = new Vector2(x, y);
        GameObject newObj = Instantiate(unitObj, spawnPoint, Quaternion.identity);
        Unit unit = newObj.GetComponent<Unit>();

        if (Eat(unit.getHunger() * 3))
        {
            this.hunger += unit.getHunger();
            units.Add(unit);
        }
        else
        {
            GameObject.Destroy(newObj);
        }
    }
}
