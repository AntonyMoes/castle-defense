using UnityEngine;

public class Pathfinder : MonoBehaviour {
    Rigidbody2D _rb;
    public float speed;
    public float delta;
    Transform _destination;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    bool SetDestination(Transform target) {
        _destination = target;

        return true;
    }

    void Stop() {
        _rb.velocity = Vector2.zero;
        _destination = null;
    }

    void FixedUpdate() {
        if (!_destination) {
            return;
        }
        
        var vector = _destination.position - _rb.transform.position;
        if (vector.magnitude <= delta) {
            Stop();
            return;
        }

        _rb.velocity = vector.normalized * speed;
    }
}
