using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : CharacterAnimationBase
{



    protected override void Start()
    {
        base.Start();


        animator = GetComponent<Animator>();

        //sync from base
        animator.speed = playSpeed;

        animator.applyRootMotion = true;


        //update refs from base
        idle = "E_Idle";
        move = "E_Move";
        jump = "E_Jump";
        drop = "E_Drop";
        die = "E_Die";
    }

}
