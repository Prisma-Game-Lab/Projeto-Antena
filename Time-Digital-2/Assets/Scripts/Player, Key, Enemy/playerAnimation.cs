using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{

    private playerMovement pm;
    public Animator animator;

    void Start()
    {
        pm = this.GetComponent<playerMovement>();
    }

    void Update()
    {
        UpdateAnim();
    }

    void UpdateAnim()
    {
        if (pm.isDead)
        {
            animator.SetBool("death", true);
        }else{
            animator.SetBool("death", false);
        }

        if (pm.isSafe)
        {
            animator.SetBool("deitado", true);
        }else{
            animator.SetBool("deitado", false);
        }

        if (pm.isMoving)
        {
            animator.SetBool("walk", true);
        }else{
            animator.SetBool("walk", false);
        }


    }
}
