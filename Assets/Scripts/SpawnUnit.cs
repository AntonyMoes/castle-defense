using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : MonoBehaviour
{
    int x;
    int y;
    public void Spawn(GameObject unitPrefab)
    {
        Vector2 spawnPoint = new Vector2(x, y);
        Instantiate(unitPrefab, spawnPoint, Quaternion.identity);
    }
}
