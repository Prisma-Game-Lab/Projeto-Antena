using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigo1Animation : MonoBehaviour
{
    private EnemyAI eA;
    private Animator anim;
    public GameObject player;
    private playerMovement pm;

    void Start()
    {
        eA = this.GetComponent<EnemyAI>();
        anim = this.GetComponentInChildren<Animator>();
        pm = player.GetComponent<playerMovement>();
    }

    void Update()
    {
        enemyAnimation();
    }


    void enemyAnimation(){

        if(eA.navMeshAgent.speed == eA.followSpeed){
            anim.SetBool("run", true);
        }else if (eA.navMeshAgent.speed == eA.wanderSpeed){
            anim.SetBool("run", false);
        }

        if (eA.myState == EnemyAI.stateMachine.isMoving)
        {
            anim.SetBool("walk", true);
        }
        else if (eA.myState == EnemyAI.stateMachine.isWaiting)
        {
            anim.SetBool("walk", false);
        }
        /*else if (eA.myState == EnemyAI.stateMachine.isAttacking)
        {   
            anim.SetTrigger("attack");
        }*/
        if(pm.isDead)
        {
            anim.SetTrigger("attack");
        }

        if (eA.turnedOff)
        {
            anim.SetBool("run", false);
            anim.SetBool("walk", false);
        }
    }


}
