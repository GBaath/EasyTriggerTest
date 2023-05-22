﻿using UnityEngine;
using UnityEngine.UIElements;

public class Player : IRecieveBeats 
{

    Main main;
    Game game;
    Gfx  gfx;
    Snd  snd;

    Sprite[] sprites;
    Sprite handSprite;

    SpriteRenderer animatorSprite;
    SpriteRenderer handRenderer;

    PlayerAnimation charAnimation;

    GameObject gameObject;
    GameObject animationComponent;
    
    GameObject aimHand;
    GameObject crossHair;

    Vector3 playerPosition;

    float x;
    float y;
    float startY;
    float angle;

    bool doAimLogic;
    bool iFrames;


    public Player (Main inMain, int posX, int posY) {

        main = inMain;
        game = main.game;
        gfx  = main.gfx;
        snd  = main.snd;

        sprites = gfx.GetLevelSprites("Players/Player1");
        handSprite = gfx.GetLevelSprites("NewArt/Sprites_Edited")[1];


        x = posX;
        y = posY;
        startY = posY;


        //add animatorcomp is has
        if (main.playerAnimatorController)
        {
            gameObject = gfx.MakeGameObject("Player", null ,x, y,"Player");
            
            //setup animation sprites
            aimHand = gfx.MakeGameObject("AimHand", handSprite,x,y,"GameObjectsFront");
            GameObject[] children = new GameObject[1];
            children[0] = aimHand;

            animationComponent = gfx.AddGameObjectWithAnimator("P_Animator", sprites[22], out animatorSprite, gameObject.transform,main.playerAnimatorController, charAnimation,default,default,"GameObjects",children);
            charAnimation = animationComponent.GetComponent<PlayerAnimation>();
            
            aimHand.transform.parent = animationComponent.transform;
            aimHand.transform.localScale = gfx.resetVector;
            aimHand.SetActive(false);
            handRenderer = aimHand.GetComponent<SpriteRenderer>();


            //add crosshair
            crossHair = gfx.MakeGameObject("CrossHair", gfx.GetLevelSprites("NewArt/Crosshair")[0], 0, 0, "Default");
            crossHair.SetActive(false);


            //magic numbers for placement on sprite
            aimHand.transform.localPosition = new Vector3(5, 27);
        }
        else //default code here
        {
            gameObject = gfx.MakeGameObject("Player", sprites[22], x, y, "Player");
        }

        playerPosition = gameObject.transform.localPosition;
    }

    public void FrameEvent(int inMoveX, int inMoveY, bool inAim, bool inShoot, float inSpeed=1, float yDif = 0) 
    {        

        x += inMoveX*inSpeed;
        y = startY - yDif;

        if (inMoveX != 0 & !doAimLogic)
            Movecheck(inMoveX);
        else IdleCheck(inMoveX);



        if (inMoveY == 1) DropCheck();
        if (inMoveY == -1) JumpCheck();

        if (inAim) AimCheck();
        else if(!inShoot) StopAim();

        if (inShoot) ShootCheck();


        UpdatePos();

        if (doAimLogic)
            Aim(out angle);


        //set iframe from animation
        iFrames = charAnimation.iFrames;

        //evade if aiming up +-20deg
        Evade(angle < 120 && angle > 70);
    }


    void UpdatePos() 
    {

        playerPosition.x = x;
        playerPosition.y = -y;

        //hard coded edges of platform
        x = Mathf.Clamp(x, 470, 600);

        gameObject.transform.localPosition = playerPosition;
    }
    void Aim(out float angle)
    {
        //aim arm toward crosshair
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(aimHand.transform.position);
        angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;

        aimHand.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        crossHair.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //set flip sprites
        animatorSprite.flipX = crossHair.transform.position.x < gameObject.transform.position.x ? true : false;
        handRenderer.flipY = animatorSprite.flipX;

        //move arm to sync with rotation
        float y = aimHand.transform.localPosition.y;
        Vector2 handPos = animatorSprite.flipX ? new Vector2(-5,y):new Vector2(5,y);
        aimHand.transform.localPosition = handPos;


        
    }
    //call this multiple times in animation
    public void Shoot()
    {
        //These should be place in gamemanager list if nr of enemes ever increase
        foreach (Enemy enemy in Game.instance.gameObjects)
        {

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            //could be done with ray overlapcheck, but then game is 2 ez
            if (enemy.collider.bounds.Contains(pos))
                enemy.Hit();

        }
        
    }
    void Evade(bool okAngle)
    {
        if (charAnimation.pState == PlayerAnimation.PlayerAnimState.Fire)
            return;


        //is aiming up, and not in evade
        if (doAimLogic&okAngle & charAnimation.pState != PlayerAnimation.PlayerAnimState.Evade)
        {
            charAnimation.SetPlayerAnimState(PlayerAnimation.PlayerAnimState.Evade);
            aimHand.SetActive(false);
        }
        //out of angle
        else if(doAimLogic && !okAngle)
        {
            charAnimation.SetPlayerAnimState(PlayerAnimation.PlayerAnimState.Aim);
            aimHand.SetActive(true);
        }
        else if (!okAngle || !doAimLogic)
        {
            aimHand.SetActive(false);
            //charAnimation.SetAnimState(CharacterAnimationBase.AnimationState.Idle);
            if(charAnimation.pState != PlayerAnimation.PlayerAnimState.None)
                charAnimation.SetPlayerAnimState(PlayerAnimation.PlayerAnimState.None);
        }
    }




    #region "AnimChecks"

    private void AimCheck()
    {
        if (charAnimation.state != CharacterAnimationBase.AnimationState.Idle && charAnimation.state != CharacterAnimationBase.AnimationState.Move)
            return;
        if (charAnimation.pState == PlayerAnimation.PlayerAnimState.Fire)
            return;
        if (charAnimation.pState == PlayerAnimation.PlayerAnimState.Evade)
            return;

        aimHand.SetActive(true);
        crossHair.SetActive(true);
        doAimLogic = true;
        charAnimation.SetPlayerAnimState(PlayerAnimation.PlayerAnimState.Aim);
    }
    void StopAim()
    {
        aimHand.SetActive(false);
        crossHair.SetActive(false);
        doAimLogic = false;

        if(charAnimation.pState != PlayerAnimation.PlayerAnimState.None)
            charAnimation.SetPlayerAnimState(PlayerAnimation.PlayerAnimState.None);
    }

    private void ShootCheck()
    {
        if (charAnimation.pState != PlayerAnimation.PlayerAnimState.Aim && charAnimation.pState != PlayerAnimation.PlayerAnimState.Fire)
            return;

        charAnimation.SetPlayerAnimState(PlayerAnimation.PlayerAnimState.Fire);
        //Shoot(); 
    }

    private void JumpCheck()
    {
        //during jump, drop or when on top row
        if (charAnimation.state == CharacterAnimationBase.AnimationState.Jump || charAnimation.pYSend == 64)
            return;
        if (charAnimation.state == CharacterAnimationBase.AnimationState.Drop)
            return;

        charAnimation.SetAnimState(CharacterAnimationBase.AnimationState.Jump );
    }
    private void DropCheck()
    {
        //during jump, drop or when on bottom row
        if (charAnimation.state == CharacterAnimationBase.AnimationState.Drop || charAnimation.pYSend == -64)
            return;
        if (charAnimation.state == CharacterAnimationBase.AnimationState.Jump)
            return;

        charAnimation.SetAnimState(CharacterAnimationBase.AnimationState.Drop);
    }
    private void IdleCheck(int inMoveX)
    {
        if (inMoveX == 0 && charAnimation.state == CharacterAnimationBase.AnimationState.Move)
            charAnimation.SetAnimState(CharacterAnimationBase.AnimationState.Idle);

    }
    private void Movecheck(int inMoveX)
    {
        animatorSprite.flipX = inMoveX < 0 ? true : false;

        if (inMoveX == 0)
            return;
        //only cycle to move from idle
        if (charAnimation.state != CharacterAnimationBase.AnimationState.Idle)
            return;

                    animatorSprite.flipX = inMoveX < 0 ? true : false;
        charAnimation.SetAnimState(CharacterAnimationBase.AnimationState.Move);
    }
    #endregion

    #region "BeatEvents"
    public void BeatUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OffBeatUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void AddToList()
    {
        Conductor.instance.recievers.Add(this);
    }
    #endregion
}