using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Distancia do player para entrar em modo de ataque
    public float attackModeViewRange;
    //Distancia para sair do modo de ataque
    public float disengageAttackDistance;
    //Distancia para realizar o ataque
    public float attackRange;
    //Força aplicada no ataque (placeholder)
    public float attackForce;
    //Velocidade de perseguição
    public float attackModeSpeed;
    //Distancia mínima de uma patrulha
    public float wanderRangeMin;
    //Distancia máxima de uma patrulha
    public float wanderRangeMax;
    //Chance de parar e esperar um tempo depois de terminar uma patrulha
    public float chanceToStopWandering;
    //Tempo mínimo esperado antes de fazer uma nova patrulha
    public float stopWanderingTimeMin;
    //Tempo máximo esperado antes de fazer uma nova patrulha
    public float stopWanderingTimeMax;

    //Contadores de tempo
    private float timeToWait;
    private float timer;

    //Guarda velocidade inicial
    private float currentSpeed;

    //Posição de destino de uma patrulha
    private Vector3 wanderTargetPosition;

    //Estados que definem comportamentos da AI
    private enum stateMachine { isWaiting, isReadyToWander, isMoving, isAttacking}
    //Guarda estado atual
    stateMachine myState;

    private NavMeshAgent navMeshAgent;
    private NavMeshHit navHit;

    //Referencia ao script playerMovement
    private playerMovement player;

    void Start()
    {
        //Guarda referencia para a instancia do script playerMovement
        player = playerMovement.current;

        navMeshAgent = GetComponent<NavMeshAgent>();

        //Define estado inicial para patrulhar
        myState = stateMachine.isReadyToWander;
        //Guarda velocidade inicial
        currentSpeed = navMeshAgent.speed;
        //Aumenta velocidade angular de rotação de 120 para 320
        navMeshAgent.angularSpeed = 320;
    }

    void Update()
    {
        //Se já não estiver em modo de ataque checa se a distancia entre este objeto e o player é menor ou igual a attackModeViewRange 
        if (myState != stateMachine.isAttacking && Vector3.Distance(player.transform.position, transform.position) <= attackModeViewRange && player.isMoving)
            myState = stateMachine.isAttacking;
        //Se estiver preparado para pratulhar, patrulha
        if (myState == stateMachine.isReadyToWander)
        {
            wander();
        }
        //Se estiver patrulhando checa se ja chegou ao seu destino
        else if (myState == stateMachine.isMoving){
            checkIfReachedDestination();

        }
        //Se estiver esperando entre uma patrulha e outra, calcula o tempo que tem que esperar
        else if (myState == stateMachine.isWaiting)
        {
            waitForTime();
        }
        //Se estiver em modo de ataque, executa comportamento de ataque
        else if (myState == stateMachine.isAttacking)
        {
            followAndAttack();
        }
    }

    //Persegue e ataca o player se estiver a uma distancia minima, muda comportamento caso esteja muito longe do player
    private void followAndAttack()
    {
        //Aumenta velocidade e persegue o player
        navMeshAgent.speed = attackModeSpeed;
        navMeshAgent.SetDestination(player.transform.position);

        //Guarda a distancia do player
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        //Checa se o player esta no alcance do ataque e ataca 
        if (distanceToPlayer <= attackRange)
        {
            //Ataque
            //player.rb.AddForce((transform.forward.normalized+Vector3.up*0.1f) * attackForce, ForceMode.Impulse);
        }

        //Checa se player esta muito longe, caso esteja, muda de estado para voltar a patrulhar e retoma velocidade inicial
        else if (distanceToPlayer >= disengageAttackDistance)
        {
            myState = stateMachine.isReadyToWander;
            navMeshAgent.speed = currentSpeed;
        }
    }

    //Calcula quanto tempo se deve esperar e muda de estado para voltar patrulhar
    private void waitForTime()
    {
        //Se esperou o tempo pre definido
        timer += Time.deltaTime;
        if (timer >= timeToWait)
        {
            //Reseta o contador e muda de estado
            timer = 0f;
            myState = stateMachine.isReadyToWander;
        }
    }

    //Checa se já chegou ao destino
    private void checkIfReachedDestination()
    {
        //Guarda distancia do destino
        float destinationDistance = Vector3.Distance(transform.position, wanderTargetPosition);

        //Checa se chegou minimamente perto do destino
        if (destinationDistance <= 0.8f)
        {
            //Calcula as chances da AI mudar de estado para esperar um tempo antes de iniciar nova patrulha
            float randomPorcentage = Random.Range(0f, 100f);
            if (chanceToStopWandering >= randomPorcentage)
            {
                //Caso deva esperar, predefine o tempo que se deve esperar antes de iniciar nova patrulha
                timeToWait = Random.Range(stopWanderingTimeMin, stopWanderingTimeMax);
                myState = stateMachine.isWaiting;
            }
            //Caso contrário inicia direto uma nova patrulha
            else
                myState = stateMachine.isReadyToWander;
        }
    }

    //Inicia patrulha definindo um destino randomico e válido no NavMesh
    private void wander()
    {
        //Se posição aleatória em volta da posição atual for válida
        if (RandomWanderTarget(transform.position, out wanderTargetPosition))
        {
            //Marca posição aleatória como destino e muda de estado
            navMeshAgent.SetDestination(wanderTargetPosition);
            myState = stateMachine.isMoving;
        }
    }

    //Recebe posição atual (centro) e retorna posição aleatória em torno dessa posição
    private bool RandomWanderTarget(Vector3 center, out Vector3 result)
    {
        //Define alcance da posição aleatória da patrulha
        float wanderRange = Random.Range(wanderRangeMin, wanderRangeMax);
        //Calcula posição aleatória em torno da posição atual com o alcance definido anteriormente
        Vector3 randomPosition = center + UnityEngine.Random.insideUnitSphere * wanderRange;

        //Se posição aleatória for válida no NavMesh
        if(NavMesh.SamplePosition(randomPosition, out navHit, 1.0f, NavMesh.AllAreas))
        {
            //Retorna true e a posição no NavMesh
            result = navHit.position;
            return true;
        }
        else
        {
            //Caso contrário não achei uma posição válida, retorna posição atual e false
            result = center;
            return false;
        }
    }

}
