using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] GameObject castle;
    GameObject target;
    bool hasTarget = false;

    // Если цель НЕ юнит, то найти ближайшего юнита и проверить, что он в зоне видимости
    // Если в зоне видимости, то задать цель этим юнитом -> SUCCESS
    // Если цель юнит, то проверить не вышел ли он из зоны видимости
    // Если не вышел, то -> SUCCESS
    // Иначе FAILURE
    NodeStates UnitVisible() {
        if (target.tag != "Unit") {
            target = GetNearestTarget(this.gameObject);
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 3) {
            Debug.Log(target.name);
            return NodeStates.SUCCESS;
        }

        target = castle;
        return NodeStates.FAILURE;
    }

    // можно ударить
    NodeStates TargetInAttackRange() {
        return NodeStates.FAILURE;
    }

    // ударить
    NodeStates Attack() {
        return NodeStates.FAILURE;
    }

    // Стоять на месте
    NodeStates Wait() {
        return NodeStates.FAILURE;
    }

    // Движение к игровому объекту (Замок, Юнит)
    NodeStates MoveToTarget() {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.005f);
        return NodeStates.SUCCESS;
    }

    [SerializeField] List<GameObject> units = new List<GameObject>();
    [SerializeField] List<GameObject> enemies = new List<GameObject>();

    public GameObject GetNearestTarget(GameObject gameObject) {
        if (gameObject.tag == "Enemy") {
            return GetNearestTarget(gameObject, units);
        }
        else if (gameObject.tag == "Unit") {
            return GetNearestTarget(gameObject, enemies);
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

    Node root;

    void Start()
    {
        target = castle;

        ActionNode inRange = new ActionNode(TargetInAttackRange);
        ActionNode moveTo = new ActionNode(MoveToTarget);
        ActionNode visible = new ActionNode(UnitVisible);
        ActionNode attack = new ActionNode(Attack);
        ActionNode wait = new ActionNode(Wait);


        Selector attackOrWait = new Selector(new List<Node>{attack, wait});
        Sequence attackInRange = new Sequence(new List<Node>{inRange, attackOrWait});
        Selector fight = new Selector(new List<Node>{attackInRange, moveTo});

        Sequence moveToVisible = new Sequence(new List<Node>{visible, moveTo});

        Selector targetUnit = new Selector(new List<Node>{fight, moveToVisible});
        Selector targetCastle = new Selector(new List<Node>{fight, moveTo});

        Selector unitOrCastle = new Selector(new List<Node>{targetUnit, targetCastle});

        root = unitOrCastle;
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
    }
}
