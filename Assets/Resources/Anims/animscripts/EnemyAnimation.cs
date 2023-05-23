using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static PlayerAnimation;

public class EnemyAnimation : CharacterAnimationBase
{
    const string aim = "E_Aim";
    const string fire = "E_Shoot";
    const string grenade = "E_Grenade";

    bool highGround = false;
    public enum EnemyAnimationState
    {
        none,
        grenade,
        aim,
        shoot,
        switchHeight
    }
    public EnemyAnimationState eState;

    public void SetEnemyAnimationState(EnemyAnimationState state)
    {
        eState = state;
        TriggerCycle();
    }

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

    protected override void TriggerCycle()
    {
        if(disabled) 
            return;
        float startTime = 1 - Conductor.instance.timeUntilNext;
        //check for playerspecific actions
        switch (eState)
        {
            case EnemyAnimationState.grenade:
                animator.Play(grenade);
                break;
            case EnemyAnimationState.shoot:
                animator.Play(fire);
                break;
            case EnemyAnimationState.aim:
                animator.Play(aim, 0, startTime);
                break;
            case EnemyAnimationState.switchHeight:
                if (highGround)
                {
                    highGround = false;
                    animator.Play(drop);
                    state = AnimationState.Drop;
                }
                else
                {
                    highGround = true;
                    animator.Play(jump);
                    state = AnimationState.Jump;
                }
                eState = EnemyAnimationState.none;
                break;
            case EnemyAnimationState.none:
                base.TriggerCycle();
                break;
        }
    }

    //used by grenade anim
    public void DealDamageAtYLevel(int y)
    {
        if ((int)Game.instance.player.playerPosition.y == y)
            Game.instance.player.Hit();

    }
    //used by shootanim
    public void DealDamageAtMyYLevel()
    {
        //wonderful float difs because funny animator likes to skip values sometimes
        if (Mathf.FloorToInt(Game.instance.player.playerPosition.y) == (int)transform.parent.localPosition.y+(int)transform.localPosition.y)
            Game.instance.player.Hit();

    }

}
