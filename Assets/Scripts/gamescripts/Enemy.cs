using UnityEngine;
using UnityEngine.UI;

public class Enemy : GeneralObject, IRecieveBeats 
{

    public const string enemyTag = "Enemy";

    Sprite aimSprite;

    SpriteRenderer animatorSprite;
    EnemyAnimation charAnimation;

    public Slider linkedSlider;
    public CapsuleCollider2D collider;



    GameObject animationComponent;
    GameObject gameObject;
    public GameObject laserAim, grenade;


    float speed = .4f;

    int hp = 20;
    public int enemyIndex = 0;

    bool doAction;
    bool doMainAction;



    public Enemy(Main inMain, int inX, int inY) {

        SetGeneralVars(inMain, inX, inY);

        sprites = gfx.GetLevelSprites("Enemies/Enemy3_2");
        aimSprite = gfx.GetLevelSprites("NewArt/square32")[0];



        //add animatorcomp is has
        if (main.enemyAnimatorController)
        {
            gameObject = gfx.MakeGameObject("Enemy", null, x, y, "Enemy");

            //setup animation sprites
            laserAim = gfx.MakeGameObject("LaserAim", aimSprite, x, y, "GameObjectsFront");
            grenade = gfx.MakeGameObject("Grenade", aimSprite, x, y, "GameObjectsFront");
            GameObject[] children = new GameObject[2];
            children[0] = laserAim;
            children[1] = grenade;

            animationComponent = gfx.AddGameObjectWithAnimator("E_Animator", sprites[22], out animatorSprite, gameObject.transform, main.enemyAnimatorController, charAnimation,default,default,"GameObjects",children);
            charAnimation = animationComponent.GetComponent<EnemyAnimation>();

            speed = 0;

            //scale of laserbeam
            laserAim.transform.localPosition = new Vector3(143, 23, 0);
            laserAim.transform.localRotation = Quaternion.Euler(0, 0, 90);
            laserAim.transform.localScale = new Vector3(.5f, 80, 1);
            var sr = laserAim.GetComponent<SpriteRenderer>();
            sr.color = new Color(255, 0, 0, 0);
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

        //I put all my actionstuff in the animators
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
    void DoAction()
    {
        doAction = false;
        //is to left
        if(enemyIndex == 0)
        {
            //fire
            if(doMainAction)
            {
                charAnimation.SetEnemyAnimationState(EnemyAnimation.EnemyAnimationState.shoot);


                doMainAction = false;
            }
            //explode nade
            else
            {
                //explodes in animation no logic needed
            }
        }
        //is to right
        else
        {
            //fire
            if (doMainAction)
            {
                charAnimation.SetEnemyAnimationState(EnemyAnimation.EnemyAnimationState.shoot);



                doMainAction = false;
            }
            //switch height
            else
            {
                charAnimation.SetEnemyAnimationState(EnemyAnimation.EnemyAnimationState.switchHeight);
            }
        }
    }






    #region HIT
    public void Hit()
    {
        hp--;
        if(linkedSlider)
            linkedSlider.value = hp;
        if(hp <= 0)
        {
            linkedSlider.gameObject.SetActive(false);
            linkedSlider = null;
            Kill();
        }
    }

    public override void Kill() 
    {
        charAnimation.SetAnimState(CharacterAnimationBase.AnimationState.Die);
    }
    #endregion
    #region Beats
    public void BeatUpdate()
    {
        //if queued, do else 50% next
        if (doAction)
        {
            DoAction();

            return;
        }

        if (Random.Range(0, 100) > 50)
            doAction = true;
        if(Random.Range(0, 100) < 50)
          doMainAction = true;

        //show warnings
        if (!doAction)
            return;

        if (doMainAction)
        {
            //spawn laser pointer, aim
            charAnimation.SetEnemyAnimationState(EnemyAnimation.EnemyAnimationState.aim);
        }
        else
        {
            //throw nade
            if(enemyIndex == 0) 
            {
                charAnimation.SetEnemyAnimationState(EnemyAnimation.EnemyAnimationState.grenade);
            }
            //switch height
            else
            {
                //no queue
                charAnimation.SetEnemyAnimationState(EnemyAnimation.EnemyAnimationState.switchHeight);
                doAction = false;
            }
        }
    }

    public void OffBeatUpdate()
    {

    }

    public void AddToList()
    {
        Conductor.instance.recievers.Add(this);
    }
    #endregion
}