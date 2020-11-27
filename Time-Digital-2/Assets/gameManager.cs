using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager current;

    public float respawnTime;
    private bool oneTime;
    private playerMovement player;
    private List<EnemyAI> enemys;

    private void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        oneTime = true;
        player = playerMovement.current;
        enemys = new List<EnemyAI>();
        fillEnemysList();
    }

    // Update is called once per frame
    void Update()
    {
        //Se player morreu, rodar uma só vez
        if (player.isDead && oneTime)
        {
            //Respanwna player e reinicia level sem resetar a scene
            StartCoroutine("resetLevel");
            oneTime = false;
        }
    }
    //Reinicia os inimigos e a posição e estado do player
    private IEnumerator resetLevel()
    {
        Debug.Log(respawnTime);
        yield return new WaitForSeconds(respawnTime);
        resetEnemys();
        player.isDead = false;
        player.transform.position = player.lastCheckpointPos;
        oneTime = true;
    }
    //Reinicia posição, estado e caminho dos inimigos
    private void resetEnemys()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].transform.position = enemys[i].pathManager.initialPos;
            enemys[i].pathManager.pathIndex = 0;
            enemys[i].myState = EnemyAI.stateMachine.isReadyToWander;
        }
    }
    //Preenche lista do tipo EnemyAI
    private void fillEnemysList()
    {
        //List<GameObject> enemysObject = new List<GameObject>();
        GameObject[] enemysObject = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemysObject.Length; i++)
        {
            enemys.Add(enemysObject[i].GetComponent<EnemyAI>());
        }
    }
}
