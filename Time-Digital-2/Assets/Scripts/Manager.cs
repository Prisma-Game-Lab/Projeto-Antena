using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager current;
    public GameObject fadeImage;
    public float fadeSmoth;
    public GameObject eButton;
    public float respawnTime;

    private bool oneTime;
    private bool canShow;
    private bool canHide;
    [HideInInspector]
    public bool turnOff;
    private playerMovement player;
    private List<EnemyAI> enemys;
    private List<int> pathIndex;
    private SceneController sceneController;


    private void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        canShow = true;
        canHide = false;
        oneTime = true;
        player = playerMovement.current;
        sceneController = this.GetComponent<SceneController>();
        enemys = new List<EnemyAI>();
        pathIndex = new List<int>();
        fillEnemysList();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = playerMovement.current;
        }
        //Se player morreu, rodar uma só vez
        if (player.isDead && oneTime)
        {
            print("restartou");
            //Respanwna player e reinicia level sem resetar a scene
            StartCoroutine("resetLevel", respawnTime);
            oneTime = false;
        }
        if (player.button && canShow)
        {
            canShow = false;
            eButton.SetActive(true);
            canHide = true;
        }
        else if (!player.button && canHide)
        {
            canHide = false;
            eButton.SetActive(false);
            canShow = true;
        }
    }
    //Reinicia os inimigos e a posição e estado do player
    private IEnumerator resetLevel(float time)
    {
        player.playerRb.velocity = Vector3.zero;
        fadeImage.SetActive(true);
        yield return new WaitForSeconds(time);
        resetEnemys();
        resetKeys();
        player.transform.position = player.lastCheckpointPos;
        player.transform.rotation = player.lastCheckpointRot;
        player.isDead = false;
        oneTime = true;
        fadeImage.SetActive(false);
    }
    private void resetKeys()
    {
        if (player.keys.keysHolding != null)
        {
            for (int i = 0; i < player.keys.keysHolding.Count; i++)
            {
                player.keys.keysHolding[i].gameObject.SetActive(true);
            }
            player.keys.keysHolding.Clear();
        }
    }
    //Reinicia posição, estado e caminho dos inimigos
    private void resetEnemys()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].pathManager.pathIndex = pathIndex[i];
            enemys[i].transform.position = enemys[i].pathManager.initialPos;
            enemys[i].transform.rotation = enemys[i].pathManager.initialRot;
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
            pathIndex.Add(enemys[i].pathManager.pathIndex);
        }
    }

    public void Respawn()
    {
        sceneController.Resume();
        player.isDead = true;
    }
}

