using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAI : MonoBehaviour
{
    [SerializeField] GameObject castle;
    Commandable commandable;

    GameObject target;

    NodeStates SetTargetEnemy() {
        commandable.AcquireCommand(target);
        return NodeStates.SUCCESS;
    }

    NodeStates SetTargetCastle() {
        commandable.AcquireCommand(castle);
        return NodeStates.SUCCESS;
    }

    NodeStates DeleteCommand() {
        commandable.AcquireCommand(null);
        return NodeStates.SUCCESS;
    }

    NodeStates TargetResource() {
        return commandable.CurrentCommand == CommandTargetType.Resource ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }

    NodeStates TargetEnemy() {
        return commandable.CurrentCommand == CommandTargetType.Enemy ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }

    NodeStates TargetCastle() {
        return commandable.CurrentCommand == CommandTargetType.Castle ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }

    NodeStates TargetPoint() {
        return commandable.CurrentCommand == CommandTargetType.Point ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }

    NodeStates MoveToTarget() {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.005f);
        return NodeStates.SUCCESS;
    }

    NodeStates TargetNull() {
        if (commandable.CurrentCommand.Target == null) {
            return NodeStates.SUCCESS;
        }
        return NodeStates.FAILURE;
    }

    NodeStates TargetInAttackRange() {
        return NodeStates.FAILURE;
    }

    NodeStates TargetInInteractionRange() {
        return NodeStates.FAILURE;
    }

    NodeStates HasResource() {
        return NodeStates.FAILURE;
    }

    NodeStates CollectResource() {
        return NodeStates.FAILURE;
    }

    NodeStates DropResource() {
        return NodeStates.FAILURE;
    }

    // Если цель НЕ противник, то найти ближайшего противника и проверить, что он в зоне видимости
    // Если в зоне видимости, то задать цель этим юнитом -> SUCCESS
    // Если цель юнит, то проверить не вышел ли он из зоны видимости
    // Если не вышел, то -> SUCCESS
    // Иначе FAILURE
    NodeStates EnemyVisible() {
        if (target == null || target.tag != "Enemy") {
            target = GetNearestTarget(this.gameObject);
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 3) {
            Debug.Log(target.name);
            return NodeStates.SUCCESS;
        }

        return NodeStates.FAILURE;
    }

    NodeStates Attack() {
        return NodeStates.FAILURE;
    }

    NodeStates Wait() {
        return NodeStates.FAILURE;
    }

    Node root;

    void Start()
    {
        target = castle;
        commandable = gameObject.GetComponent<Commandable>();

        ActionNode setEnemy = new ActionNode(SetTargetEnemy);
        ActionNode setCastle = new ActionNode(SetTargetCastle);
        ActionNode deleteCommand = new ActionNode(DeleteCommand);

        ActionNode castle = new ActionNode(TargetCastle);
        ActionNode enemy = new ActionNode(TargetEnemy);
        ActionNode resource = new ActionNode(TargetResource);
        ActionNode point = new ActionNode(TargetPoint);
        ActionNode targetNull = new ActionNode(TargetNull);

        ActionNode moveTo = new ActionNode(MoveToTarget);
        ActionNode inAttackRange = new ActionNode(TargetInAttackRange);
        ActionNode inInteractionRange = new ActionNode(TargetInInteractionRange);

        ActionNode hasResource = new ActionNode(HasResource);
        ActionNode collectResource = new ActionNode(CollectResource);
        ActionNode dropResource = new ActionNode(DropResource);

        ActionNode visible = new ActionNode(EnemyVisible);
        ActionNode attack = new ActionNode(Attack);
        ActionNode wait = new ActionNode(Wait);


        Sequence goToPointCommand = new Sequence(new List<Node>{point, moveTo, inInteractionRange, deleteCommand});

        Sequence targetNullDropCommand = new Sequence(new List<Node>{targetNull, deleteCommand});
        Selector attackOrWait = new Selector(new List<Node>{attack, wait});
        Sequence fight = new Sequence(new List<Node>{inAttackRange, attackOrWait});

        Sequence fightEnemyCommand = new Sequence(new List<Node>{enemy, fight, moveTo});

        Selector targetNullOrHasResource = new Selector(new List<Node>{targetNull, hasResource});
        Sequence canCollectResourceOrDrop= new Sequence(new List<Node>{targetNullOrHasResource, deleteCommand});
        Sequence collectResourceIfInRange = new Sequence(new List<Node>{inInteractionRange, collectResource, setCastle});
        Selector collectOrMoveToResource = new Selector(new List<Node>{collectResourceIfInRange, moveTo});

        Sequence collectResourceCommand = new Sequence(new List<Node>{resource, canCollectResourceOrDrop, collectOrMoveToResource});

        Sequence noResourceDrop = new Sequence(new List<Node>{new Inverter(hasResource), deleteCommand});
        Sequence dropResourceIfInRange = new Sequence(new List<Node>{inInteractionRange, dropResource, deleteCommand});
        Selector dropResourceOrMoveToCastle = new Selector(new List<Node>{dropResourceIfInRange, moveTo});

        Sequence dropResourceCommand = new Sequence(new List<Node>{castle, noResourceDrop, dropResourceOrMoveToCastle});

        Sequence hasResourceSetCommand = new Sequence(new List<Node>{hasResource, setCastle});
        Sequence hasEnemySetCommand = new Sequence(new List<Node>{visible, enemy});

        Selector noCommand = new Selector(new List<Node>{hasResourceSetCommand, hasEnemySetCommand});

        root = new Selector(new List<Node>{goToPointCommand, fightEnemyCommand, collectResourceCommand, dropResourceCommand, noCommand});
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
    }
}