using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private NavMeshHit navHit;

    [SerializeField]
    private float attackModeViewRange;
    [SerializeField]
    private float disengageAttackDistance;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float attackForce;

    [SerializeField]
    private float wanderRangeMin;
    [SerializeField]
    private float wanderRangeMax;
    [SerializeField]
    private float chanceToStopWandering;
    [SerializeField]
    private float stopWanderingTimeMin;
    [SerializeField]
    private float stopWanderingTimeMax;

    [SerializeField]
    private GameObject player;

    private float timeToWait;
    private float timer;

    private float currentSpeed;

    private Vector3 wanderTargetPosition;

    private enum stateMachine { isWaiting,isReadyToWander, isMoving, isSensing, isAttacking, isLooking}
    stateMachine myState;

    void Start()
    {
        myState = stateMachine.isReadyToWander;
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentSpeed = navMeshAgent.speed;
    }

    void Update()
    {
        if (myState != stateMachine.isAttacking && Vector3.Distance(player.transform.position, transform.position) <= attackModeViewRange)
            myState = stateMachine.isAttacking;

        if (myState == stateMachine.isReadyToWander)
        {
            wander();
        }
        else if (myState == stateMachine.isMoving)
        {
            checkIfReachedDestination();

        }else if (myState == stateMachine.isWaiting)
        {
            waitForTime();
        }else if (myState == stateMachine.isAttacking)
        {
            followAndAttack();
        }
    }

    private void followAndAttack()
    {
        navMeshAgent.speed = 11f;
        navMeshAgent.SetDestination(player.transform.position);

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= attackRange)
            player.GetComponent<Rigidbody>().AddForce((transform.forward.normalized+Vector3.up*0.1f) * attackForce, ForceMode.Impulse);

        else if (distanceToPlayer >= disengageAttackDistance)
        {
            myState = stateMachine.isReadyToWander;
            navMeshAgent.speed = currentSpeed;
        }
    }

    private void waitForTime()
    {
        timer += Time.deltaTime;
        if (timer >= timeToWait)
        {
            timer = 0f;
            myState = stateMachine.isReadyToWander;
        }
    }

    private void checkIfReachedDestination()
    {
        float destinationDistance = Vector3.Distance(transform.position, wanderTargetPosition);
        Debug.Log("distancia: " + destinationDistance);
        if (destinationDistance <= 0.8f)
        {
            float randomPorcentage = Random.Range(0f, 100f);
            if (chanceToStopWandering >= randomPorcentage)
            {
                timeToWait = Random.Range(stopWanderingTimeMin, stopWanderingTimeMax);
                myState = stateMachine.isWaiting;
            }
            else
                myState = stateMachine.isReadyToWander;

            Debug.Log("chegou");
        }
    }

    private void wander()
    {
        if (RandomWanderTarget(transform.position, out wanderTargetPosition))
        {
            Debug.Log("Achei caminho");
            navMeshAgent.SetDestination(wanderTargetPosition);
            myState = stateMachine.isMoving;
        }
    }

    private bool RandomWanderTarget(Vector3 center, out Vector3 result)
    {
        float wanderRange = Random.Range(wanderRangeMin, wanderRangeMax);
        Vector3 randomPosition = center + UnityEngine.Random.insideUnitSphere * wanderRange;

        if(NavMesh.SamplePosition(randomPosition, out navHit, 1.0f, NavMesh.AllAreas))
        {
            result = navHit.position;
            return true;
        }
        else
        {
            Debug.Log("Nao achei caminho");
            result = center;
            return false;
        }
    }

}
