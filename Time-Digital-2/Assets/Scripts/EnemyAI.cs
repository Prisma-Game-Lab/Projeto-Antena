using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Distancia do player para entrar em modo de ataque
    public float viewRange;
    //Distancia para sair do modo de ataque
    public float disengageDistance;
    //Distancia para realizar o ataque
    public float attackRange;
    //Velocidade de patrulhamento
    public float wanderSpeed;
    //Velocidade de perseguição
    public float followSpeed;
    //Lista do caminho que a AI deve fazer
    public List<AIPath> pathPoints = new List<AIPath>();

    [Tooltip("Segue um caminho baseado nas posições dos filhos desse GameObject, ordem da hierarquia representa a ordem do caminho a ser seguido.")]
    public GameObject path;

    private bool stopAttack;
    private int pathIndex;
    //Contador de tempo
    private float timeToWait;
    private float timer;
    //Posição do destino de uma patrulha
    private Vector3 navMeshPosition;
    //Estados que definem comportamentos da AI

    public enum stateMachine { isWaiting, isReadyToWander, isMoving, isAttacking }
    [HideInInspector]
    public stateMachine myState;
    //Referencia ao script playerMovement
    private playerMovement player;

    private NavMeshAgent navMeshAgent;
    private NavMeshHit navHit;

    public AudioSource morte;

    void Start()
    {
        if (path != null)
        {
            //Preenche as posições da lista do caminho
            fillPathPoints();
        }
        //Guarda referencia para a instancia do script playerMovement
        player = playerMovement.current;
        navMeshAgent = GetComponent<NavMeshAgent>();

        //Define estado inicial para patrulhar
        myState = stateMachine.isReadyToWander;
        //Define velocidade inicial de patrulhamento
        navMeshAgent.speed = wanderSpeed;
        //Velocidade angular de rotação
        navMeshAgent.angularSpeed = 320;
        pathIndex = 0;
    }

    void Update()
    {
        //Se já não estiver em modo de ataque checa se a distancia entre este objeto e o player é menor ou igual a ViewRange 
        if (myState != stateMachine.isAttacking && Vector3.Distance(player.transform.position, transform.position) <= viewRange && player.isMoving && !player.isSafe)
            myState = stateMachine.isAttacking;
        //Se estiver preparado para pratulhar, patrulha
        if (myState == stateMachine.isReadyToWander)
        {
            wander();
        }
        //Se estiver patrulhando checa se ja chegou ao seu destino
        else if (myState == stateMachine.isMoving)
        {
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

    //Preenche a lista do caminho com as posições dos gameObjects filhos de path 
    private void fillPathPoints()
    {
        int i = 0;
        int pathPointsCount = pathPoints.Count;
        //Preenche lista do caminho
        for (; i < pathPointsCount && i < path.transform.childCount; i++)
        {
            pathPoints[i].destinationPos = path.transform.GetChild(i).transform.position;
        }
        //Caso tenha mais elementos na lista do caminho do que filhos de path, elimina o excesso
        if (pathPointsCount > path.transform.childCount)
        {
            for (; i < pathPointsCount; pathPointsCount--)
            {
                pathPoints.RemoveAt(i);
            }
        }
    }
    //Persegue e ataca o player se estiver a uma distancia minima, muda comportamento caso esteja muito longe do player
    private void followAndAttack()
    {

        //Aumenta velocidade e persegue o player
        navMeshAgent.speed = followSpeed;
        navMeshAgent.SetDestination(player.transform.position);

        //Guarda a distancia do player
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        //Se o player entrar em um esconderijo e estiver seguro, muda o estado para esperar e continuar a patrulha
        if (player.isSafe)
        {
            navMeshAgent.speed = wanderSpeed;
            timeToWait = 4f;
            myState = stateMachine.isWaiting;
        }

        //Checa se o player esta no alcance do ataque e ataca 
        if (distanceToPlayer <= attackRange)
        {
            //Ataque
            //player.rb.AddForce((transform.forward.normalized+Vector3.up*0.1f)*5f, ForceMode.Impulse);

            //Debug.Log("Morreu!\n");
            //morte.Play(); -> nao funciona pq a cena restarta
        }

        //Checa se player esta muito longe, caso esteja, muda de estado para voltar a patrulhar e retoma velocidade inicial
        else if (distanceToPlayer >= disengageDistance)
        {
            myState = stateMachine.isReadyToWander;
            navMeshAgent.speed = wanderSpeed;
        }
    }

    //Calcula quanto tempo se deve esperar e muda de estado para voltar patrulhar
    private void waitForTime()
    {
        //Conta o tempo predefinido
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
        //Pega o index atual
        int index;
        //Se o tiver algum caminho a fazer
        if (pathPoints.Count > 0)
        {
            //Calcula o index de pathIndex-1
            if (pathIndex == 0)
                index = pathPoints.Count - 1;
            else
                index = pathIndex - 1;
        }
        else index = 0;

        //Pega distancia do destino
        float destinationDistance = Vector3.Distance(transform.position, pathPoints[index].destinationPos);
        //Checa se chegou minimamente perto do destino
        if (destinationDistance <= 1f)
        {
            //Caso deva esperar, pega o tempo que se deve esperar antes de iniciar nova patrulha
            if (pathPoints[index].shouldWait)
            {
                timeToWait = pathPoints[index].waitTime;
                myState = stateMachine.isWaiting;
            }
            //Caso contrário inicia direto uma nova patrulha
            else
            {
                myState = stateMachine.isReadyToWander;
            }
        }
    }

    //Inicia patrulha definindo um destino randomico e válido no NavMesh
    private void wander()
    {
        //Se posição aleatória em volta da posição atual for válida
        if (RandomWanderTarget(transform.position, out navMeshPosition))
        {
            //Marca posição aleatória como destino e muda de estado
            navMeshAgent.SetDestination(navMeshPosition);
            myState = stateMachine.isMoving;
        }
    }

    //Recebe posição atual (centro) e retorna posição aleatória em torno dessa posição
    private bool RandomWanderTarget(Vector3 center, out Vector3 result)
    {
        Vector3 nextPosition;
        //Se lista do caminho nao for vazia
        if (pathPoints.Count > 0)
        {
            //Pega posição do próximo destino
            nextPosition = pathPoints[pathIndex].destinationPos;
            pathIndex++;

            //Se passou do ultimo elemento do caminho, volta para o primeiro
            if (pathIndex >= pathPoints.Count)
            {
                pathIndex = 0;
            }
        }
        else nextPosition = center;

        //Se posição do destino for válida no NavMesh
        if (pathPoints.Count > 0 && NavMesh.SamplePosition(nextPosition, out navHit, 1.0f, NavMesh.AllAreas) && path != null)
        {
            //Retorna true e a posição no NavMesh
            result = navHit.position;
            return true;
        }
        else
        {
            //Caso contrário não é uma posição válida, retorna posição atual e false
            result = center;
            return false;
        }
    }
}
