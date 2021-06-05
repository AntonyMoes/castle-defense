using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] GameObject enemyObject;
    [SerializeField] GameObject unitObject;
    [SerializeField] bool isManual;

    Vector2 _spawnPoint;
    const float SpawnRate = 4.0f;
    readonly EntityManager _entityManager = new EntityManager();

    void Start() {
        var localPosition = gameObject.transform.localPosition;
        _spawnPoint = new Vector2(localPosition.x, localPosition.y);

        if (!isManual) {
            InvokeRepeating(nameof(SpawnEnemy), 1.0f, SpawnRate);
        }
    }

    void SpawnEnemy() {
        var enemy = Instantiate(enemyObject, _spawnPoint, Quaternion.identity);
        _entityManager.SpawnEntity(enemy, _spawnPoint);
    }

    void SpawnUnit() {
        var unit = Instantiate(unitObject, _spawnPoint, Quaternion.identity);
        _entityManager.SpawnEntity(unit, _spawnPoint);
    }

    public void Spawn() {
        SpawnUnit();
    }
}
