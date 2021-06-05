using System.Collections.Generic;
using Commands;
using UnityEngine;

public class UnitAI : MonoBehaviour {
    [SerializeField] GameObject castle;
    Commandable commandable;
    GameObject target;

    NodeStates SetTargetEnemy() {
        commandable.CurrentCommand = new Command(target);
        return NodeStates.SUCCESS;
    }

    NodeStates SetTargetCastle() {
        commandable.CurrentCommand = new Command(castle);
        return NodeStates.SUCCESS;
    }

    NodeStates DeleteCommand() {
        commandable.CurrentCommand = null;
        return NodeStates.SUCCESS;
    }

    NodeStates TargetResource() {
        return commandable.CurrentCommand.TargetType == CommandTargetType.Resource
            ? NodeStates.SUCCESS
            : NodeStates.FAILURE;
    }

    NodeStates TargetEnemy() {
        return commandable.CurrentCommand.TargetType == CommandTargetType.Enemy
            ? NodeStates.SUCCESS
            : NodeStates.FAILURE;
    }

    NodeStates TargetCastle() {
        return commandable.CurrentCommand.TargetType == CommandTargetType.Castle
            ? NodeStates.SUCCESS
            : NodeStates.FAILURE;
    }

    NodeStates TargetPoint() {
        return commandable.CurrentCommand.TargetType == CommandTargetType.Point
            ? NodeStates.SUCCESS
            : NodeStates.FAILURE;
    }

    NodeStates MoveToTarget() {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.005f);
        return NodeStates.SUCCESS;
    }

    NodeStates CommandNull() {
        if (commandable.CurrentCommand == null) {
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
        if (target == null || !target.CompareTag("Enemy")) {
            target = EntityManager.Instance
                .GetNearestTarget(gameObject); // TODO: EXPLICITLY provide functionality to get nearest enemy
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

    Node _root;

    void Start() {
        target = castle;
        commandable = gameObject.GetComponent<Commandable>();

        var setEnemy = new ActionNode(SetTargetEnemy);
        var setCastle = new ActionNode(SetTargetCastle);
        var deleteCommand = new ActionNode(DeleteCommand);

        var targetCastle = new ActionNode(TargetCastle);
        var enemy = new ActionNode(TargetEnemy);
        var resource = new ActionNode(TargetResource);
        var point = new ActionNode(TargetPoint);
        var targetNull = new ActionNode(CommandNull);

        var moveTo = new ActionNode(MoveToTarget);
        var inAttackRange = new ActionNode(TargetInAttackRange);
        var inInteractionRange = new ActionNode(TargetInInteractionRange);

        var hasResource = new ActionNode(HasResource);
        var collectResource = new ActionNode(CollectResource);
        var dropResource = new ActionNode(DropResource);

        var visible = new ActionNode(EnemyVisible);
        var attack = new ActionNode(Attack);
        var wait = new ActionNode(Wait);


        var goToPointCommand = new Sequence(new List<Node> {point, moveTo, inInteractionRange, deleteCommand});

        var targetNullDropCommand = new Sequence(new List<Node> {targetNull, deleteCommand});
        var attackOrWait = new Selector(new List<Node> {attack, wait});
        var fight = new Sequence(new List<Node> {inAttackRange, attackOrWait});

        var fightEnemyCommand = new Sequence(new List<Node> {enemy, fight, moveTo});

        var targetNullOrHasResource = new Selector(new List<Node> {targetNull, hasResource});
        var canCollectResourceOrDrop = new Sequence(new List<Node> {targetNullOrHasResource, deleteCommand});
        var collectResourceIfInRange = new Sequence(new List<Node> {inInteractionRange, collectResource, setCastle});
        var collectOrMoveToResource = new Selector(new List<Node> {collectResourceIfInRange, moveTo});

        var collectResourceCommand = new Sequence(new List<Node>
            {resource, canCollectResourceOrDrop, collectOrMoveToResource});

        var noResourceDrop = new Sequence(new List<Node> {new Inverter(hasResource), deleteCommand});
        var dropResourceIfInRange = new Sequence(new List<Node> {inInteractionRange, dropResource, deleteCommand});
        var dropResourceOrMoveToCastle = new Selector(new List<Node> {dropResourceIfInRange, moveTo});

        var dropResourceCommand =
            new Sequence(new List<Node> {targetCastle, noResourceDrop, dropResourceOrMoveToCastle});

        var hasResourceSetCommand = new Sequence(new List<Node> {hasResource, setCastle});
        var hasEnemySetCommand = new Sequence(new List<Node> {visible, enemy});

        var noCommand = new Selector(new List<Node> {hasResourceSetCommand, hasEnemySetCommand});

        _root = new Selector(new List<Node>
            {goToPointCommand, fightEnemyCommand, collectResourceCommand, dropResourceCommand, noCommand});
    }

    // Update is called once per frame
    void Update() {
        _root.Evaluate();
    }
}
