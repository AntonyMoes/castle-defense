using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleAI : MonoBehaviour
{
    NodeStates moveToPoint(GameObject target) {
        gameObject.GetComponent<EnemyMovement>().setTarget(target);
        return NodeStates.RUNNING;
    }

    NodeStates pointNearBy(GameObject target1) {
        if (Vector3.Distance(transform.position, target1.transform.position) < 2) {
            Debug.Log("+");
            return NodeStates.SUCCESS;
        }
        return NodeStates.FAILURE;
    }

    Selector root;
    Sequence root2;

    [SerializeField] GameObject castle; 
    [SerializeField] GameObject unit;

    void Start()
    {
        ActionNodeParameter<GameObject> moveToCastle = new ActionNodeParameter<GameObject>(moveToPoint, castle);
        ActionNodeParameter<GameObject> moveToUnit = new ActionNodeParameter<GameObject>(moveToPoint, unit);
        ActionNodeParameter<GameObject> unitNearBy = new ActionNodeParameter<GameObject>(pointNearBy, unit);
        
        List<Node> l1 = new List<Node>();
        l1.Add(unitNearBy);
        l1.Add(moveToUnit);
        root2 = new Sequence(l1);

        List<Node> l2 = new List<Node>();
        l2.Add(root2);
        l2.Add(moveToCastle);

        root = new Selector(l2);
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
    }
}
