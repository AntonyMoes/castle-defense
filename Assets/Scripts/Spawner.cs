using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyObj;
    [SerializeField] GameObject unitObj;
    [SerializeField] bool isManual = false;

    Vector2 spawnPoint;
    float spawnRate = 4.0f;
    float spawnCD = 0.0f;
    EntityManager em = new EntityManager();

    private void Start()
    {
        float x = gameObject.transform.localPosition.x;
        float y = gameObject.transform.localPosition.y;
        spawnPoint = new Vector2(x, y);
        if(!isManual)
            InvokeRepeating("SpawnEnemy", 1.0f, 4f);
    }
    void SpawnEnemy()
    {
        GameObject enemyObj = Instantiate(this.enemyObj, spawnPoint, Quaternion.identity);
        em.SpawnEntity(enemyObj, spawnPoint);
    }
    void SpawnUnit()
    {
        GameObject unitObj = Instantiate(this.unitObj, spawnPoint, Quaternion.identity);
        em.SpawnEntity(unitObj, spawnPoint);
    }

    public void Spawn()
    {
        SpawnUnit();
    }
}
