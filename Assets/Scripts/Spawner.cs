using UnityEngine;

public class Spawner : MonoBehaviour { // TODO: BURN
    [SerializeField] GameObject enemyObject;
    [SerializeField] GameObject unitObject;
    [SerializeField] bool isManual;

    Vector2 _spawnPoint;
    const float SpawnRate = 4.0f;
    readonly EntityManager _entityManager = EntityManager.Instance;

    void Start() {
        var localPosition = gameObject.transform.localPosition;
        _spawnPoint = new Vector2(localPosition.x, localPosition.y);

        if (!isManual) {
            InvokeRepeating(nameof(SpawnEnemy), 1.0f, SpawnRate);
        }
    }

    void SpawnEnemy() {
        _entityManager.SpawnEntity(() => InstantiatePrefab(enemyObject));
    }

    void SpawnUnit() {
        _entityManager.SpawnEntity(() => InstantiatePrefab(unitObject));
    }

    GameObject InstantiatePrefab(GameObject prefab) {
        return Instantiate(prefab, _spawnPoint, Quaternion.identity);
    }

    public void Spawn() {
        SpawnUnit();
    }
}
