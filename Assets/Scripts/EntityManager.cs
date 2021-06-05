using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager> { // TODO: un-simpleton this class
    readonly List<GameObject> _units = new List<GameObject>();
    readonly List<GameObject> _enemies = new List<GameObject>();

    public GameObject GetNearestTarget(GameObject gameObject) {
        if (gameObject.CompareTag("Enemy")) {
            return GetNearestTarget(gameObject, _units);
        }

        if (gameObject.CompareTag("Unit")) {
            return GetNearestTarget(gameObject, _enemies);
        }

        return null;
    }

    public GameObject GetNearestTarget(GameObject gameObject, List<GameObject> possibleTargets) {
        float dist = Vector3.Distance(gameObject.transform.position, possibleTargets[0].transform.position);
        int ind = 0;

        for (int i = 1; i < possibleTargets.Count; ++i) {
            float newDist = Vector3.Distance(gameObject.transform.position, possibleTargets[i].transform.position);
            if (newDist < dist) {
                dist = newDist;
                ind = i;
            }
        }

        return possibleTargets[ind];
    }

    public void SpawnEntity(Func<GameObject> objectInstantiator) {
        var gameObject = objectInstantiator();
        if (gameObject.CompareTag("Enemy")) {
            _enemies.Add(gameObject);
            Enemy enemy = gameObject.GetComponent<Enemy>();
            enemy.OnDestroy += x => _enemies.Remove(x.gameObject);
        } else if (gameObject.CompareTag("Unit")) {
            _units.Add(gameObject);
            Unit unit = gameObject.GetComponent<Unit>();
            unit.OnDestroy += x => _units.Remove(x.gameObject);
        }
    }
}
