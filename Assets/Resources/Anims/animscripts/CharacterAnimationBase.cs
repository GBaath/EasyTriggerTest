using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationBase : MonoBehaviour, IRecieveBeats
{

    //clip keys
    protected string idle = "Idle";
    protected string move = "Move";
    protected string jump = "Jump";
    protected string drop = "Drop";
    protected string die = "Die";


    protected float playSpeed = 1;
    protected Animator animator;


    public enum AnimationState
    {
        Idle,
        Move,
        Jump,
        Drop,
        Die
    }
    [SerializeField]public AnimationState state;
    [SerializeField]private AnimationState prevState = AnimationState.Idle;

    protected virtual void Start()
    {
        //sync with bpm
        playSpeed = Conductor.instance.animSpeed;

        //insta cycle to idle
        state = AnimationState.Move;

        //sub to conductor
        AddToList();
    }

    //state also set in animations to fix irregular bug that desyncs state with what's playing
    public void SetAnimState(AnimationState state)
    {
        //this is move walkcycle
        if(state != this.state)
        {
            prevState = this.state;
        }


        this.state = state;

        TriggerCycle();


    }

    virtual protected void TriggerCycle()
    {
        float startTime = 1-Conductor.instance.timeUntilNext;
        switch (state)
        {
            case AnimationState.Idle:
                animator.Play(idle, 0, startTime);
                break;
            case AnimationState.Move:
                if(prevState != state)
                {
                    animator.Play(move, 0, startTime);
                }
                break;
            case AnimationState.Jump:
                animator.Play(jump);
                break;
            case AnimationState.Drop:
                animator.Play(drop);
                break;
            case AnimationState.Die:
                animator.Play(die);
                break;
        }
    }
    public void SetAnimSpeed(float speed)
    {
        playSpeed = speed;
        animator.speed = speed;
    }


    public virtual void BeatUpdate()
    {
        prevState = state;
    }

    public void OffBeatUpdate()
    {

    }

    public void AddToList()
    {
        Conductor.instance.recievers.Add(this);
    }
}
