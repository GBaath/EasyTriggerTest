using UnityEngine;

public class Enemy : GeneralObject {

    SpriteRenderer animatorSprite;

    EnemyAnimation charAnimation;

    GameObject animationComponent;
    GameObject gameObject;

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



    public override void Kill() {
       
    }






}