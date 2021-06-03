using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    GameObject target;
    bool canMove = false;

    public void setTarget(GameObject target) {
        this.target = target;
        canMove = true;
    }
    public void beIdle() {
        canMove = false;
    }

    public void move() {
        if (canMove) {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.005f);
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, transform.position, 0.005f);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
}
