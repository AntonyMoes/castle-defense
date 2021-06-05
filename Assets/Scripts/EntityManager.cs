using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    List<GameObject> units = new List<GameObject>();
    List<GameObject> enemies = new List<GameObject>();

    public Unit AddUnit(Vector2 spawnPoint)
    {
        return new Unit();
    }

    public GameObject GetNearestTarget(GameObject gameObject)
    {
        if (gameObject.tag == "Enemy")
        {
            return GetNearestTarget(gameObject, units);
        }
        else if (gameObject.tag == "Unit")
        {
            return GetNearestTarget(gameObject, enemies);
        }
        return null;
    }

    public GameObject GetNearestTarget(GameObject gameObject, List<GameObject> possibleTargets)
    {
        float dist = Vector3.Distance(gameObject.transform.position, possibleTargets[0].transform.position);
        int ind = 0;

        for (int i = 1; i < possibleTargets.Count; ++i)
        {
            float newDist = Vector3.Distance(gameObject.transform.position, possibleTargets[i].transform.position);
            if (newDist < dist)
            {
                dist = newDist;
                ind = i;
            }
        }

        return possibleTargets[ind];
    }

    public void SpawnEntity(GameObject gameObject, Vector2 spawnPoint)
    {
        if (gameObject.tag == "Enemy")
        {
            enemies.Add(gameObject);
            Enemy enemy = gameObject.GetComponent<Enemy>();
            enemy.onDestroy += x => enemies.Remove(x.gameObject);
        }
        else if (gameObject.tag == "Unit")
        {
            units.Add(gameObject);
            Unit unit = gameObject.GetComponent<Unit>();
            unit.onDestroy += x => units.Remove(x.gameObject);
        }
    }
}
