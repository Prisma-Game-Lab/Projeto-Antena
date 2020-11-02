using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private NavMeshHit navHit;

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

    private float timeToWait;
    private float timer;

    private Vector3 wanderTargetPosition;

    private enum stateMachine { isWaiting,isReadyToWander, isMoving, isSensing, isAttacking, isLooking}
    stateMachine myState;

    void Start()
    {
        myState = stateMachine.isReadyToWander;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
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
