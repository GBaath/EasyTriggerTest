using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : CharacterAnimationBase
{
    const string aim = "P_Aim";
    const string fire = "P_Shoot";
    const string evade = "P_Evade";

    public float animationWalkspeedMultiplier = 1;
    public float pY = 0;
    [SerializeField]private float pYStart = 0;
    public float pYSend = 0;
    public bool iFrames;
    public enum PlayerAnimState
    {
        None,
        Aim,
        Fire,
        Evade
    }
    public PlayerAnimState pState = PlayerAnimState.None;

    public void SetPlayerAnimState(PlayerAnimState state)
    {
        this.pState = state;
        TriggerCycle();
    }
    public void EndVertical()
    {
        pYSend = pYStart;
    }


    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();

        //sync from base
        animator.speed = playSpeed;

        //update refs from base
        idle = "P_Idle";
        move = "P_Move";
        jump = "P_Jump";
        drop = "P_Drop";
        die = "P_Die";

    }
    private void Update()
    { 
          Game.instance.playerSpeed = animationWalkspeedMultiplier;

        if(state == AnimationState.Jump ||  state == AnimationState.Drop)
        {
            Game.instance.playerYMove = pYSend + pY;
        }
    }

    protected override void TriggerCycle()
    {
        float startTime = 1 - Conductor.instance.timeUntilNext;
        //check for playerspecific actions
        switch (pState)
        {
            case PlayerAnimState.None:
                base.TriggerCycle();
                break;
            case PlayerAnimState.Aim:
                animator.Play(aim, 0, startTime);
                break;
            case PlayerAnimState.Fire:
                animator.Play(fire);
                break;
            case PlayerAnimState.Evade:
                animator.Play(evade);
                break;
        }


        //set ymove point start reference //cant use rootmotion, breaks other anims sometimes
        switch (state)
        {
            case AnimationState.Jump:
                pYStart += 64;
                break;
            case AnimationState.Drop:
                pYStart -= 64;
                break;
        }

    }
    public void AnimationShoot()
    {
        Game.instance.player.Shoot();
    }
}
