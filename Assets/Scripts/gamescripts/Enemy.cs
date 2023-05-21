﻿using UnityEngine;

public class Enemy : GeneralObject {

    public const string enemyTag = "Enemy";

    SpriteRenderer animatorSprite;

    EnemyAnimation charAnimation;

    GameObject animationComponent;
    GameObject gameObject;

    public CapsuleCollider2D collider;

    float speed = .4f;


    public Enemy(Main inMain, int inX, int inY) {

        SetGeneralVars(inMain, inX, inY);

        sprites = gfx.GetLevelSprites("Enemies/Enemy3_2");



        //add animatorcomp is has
        if (main.enemyAnimatorController)
        {
            gameObject = gfx.MakeGameObject("Enemy", null, x, y, "Enemy");
            animationComponent = gfx.AddGameObjectWithAnimator("E_Animator", sprites[22], out animatorSprite, gameObject.transform, main.enemyAnimatorController, charAnimation);
            charAnimation = animationComponent.GetComponent<EnemyAnimation>();

            speed = 0;
        }
        else
        {
            gameObject = gfx.MakeGameObject("Enemy", sprites[22], x, y, "Enemy");
        }


        gameObject.tag = enemyTag;

        collider = gameObject.AddComponent<CapsuleCollider2D>();

        //magic size numbers (:
        collider.offset = new Vector2(-2, 17);
        collider.size = new Vector2(10, 40);

        SetDirection(-1);
    }



    public override bool FrameEvent() {


        // enemy logic here


        // temp logic :)
        //------------------------------------------------------------
        x = x + speed*direction;
        if ((direction==1 && x > 600) || (direction==-1 && x < 480)) {
            SetDirection(-direction);
        }
        //------------------------------------------------------------



        UpdatePos();


        return isOK;

    }


    void UpdatePos() {

        gfx.SetPos(gameObject, x, y);

    }



    void SetDirection(int inDirection) {

        direction = inDirection;
        gfx.SetDirX(gameObject, direction);

    }

    public void Hit()
    {
        Debug.Log("HIT");
    }

    public override void Kill() {
       
    }






}