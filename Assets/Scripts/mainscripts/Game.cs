using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    public static Game instance;


    Main main;
    int myRes;
    Gfx gfx;
    Snd snd;

    static string PLAY = "play";

    string gameStatus;

    int   camWidth;
    int   camHeight;
    float camX;
    float camY;

    public Player player;

    bool leftKey, rightKey, jumpKey, duckKey, shootKey, aimKey;
    int playerHorizontal, playerVertical;
    bool playerShoot;
    bool playerAim;
    public float playerSpeed = 1;
    public float playerYMove = 0;


    bool inputOnBeat;


    public List<GeneralObject> gameObjects;
    int gameObjectLength;



    public void Init(Main inMain) {

        instance = this;

        main  = inMain;
        gfx   = main.gfx;
        myRes = gfx.myRes;
        snd   = main.snd;

        camWidth  = gfx.screenWidth / myRes;
        camHeight = gfx.screenHeight / myRes;

        gameObjects = new List<GeneralObject>();
        gameObjectLength = 0;

        player = new Player(main, main.playerSpawwnCoordinates.x, main.playerSpawwnCoordinates.y);

        AddLevelObject(new Enemy(main, 740, 624));
        AddLevelObject(new Enemy(main, 372, 624));

        gameStatus  = PLAY;

    }

    

    void Update() {
       
        if (gameStatus==PLAY) {

            GoKeys();

            GoPlayer();

            GoCam();

            GoObjects();

        } 

    }



    void GoPlayer() {

        player.FrameEvent(playerHorizontal, playerVertical, playerAim, playerShoot, playerSpeed, playerYMove);

    }



    private void GoKeys()
    {
        inputOnBeat = Conductor.instance.distanceFromBeat < .3f;
        //only send input 1 frame
        jumpKey = false;
        duckKey = false;
        shootKey = false;
        // ---------------------------------------------------------------
        // NORMAL KEYBOARD
		// ---------------------------------------------------------------

		if (Input.GetKeyDown(KeyCode.A))  { leftKey   = true; }
        if (Input.GetKeyUp(KeyCode.A))    { leftKey   = false; }
        if (Input.GetKeyDown(KeyCode.D)) { rightKey  = true; }
        if (Input.GetKeyUp(KeyCode.D))   { rightKey  = false; }

        //aim key can be held
        if (Input.GetKeyUp(KeyCode.Mouse1)) { aimKey = false; }


        playerHorizontal = 0;
        if (leftKey) { playerHorizontal-=1; }
        if (rightKey) { playerHorizontal+=1; }


        //dont need these when 1 frame inputs
        /*
        if (shootKey) {
            if (playerShootRelease) {
                playerShootRelease = false;
                playerShoot = true;
            }
            
        } else {
            if (!playerShootRelease) {
                playerShootRelease = true;
            }
        playerShoot = false;
        }*/



        if (!inputOnBeat)
            return;

        //rhythm keys here
        if (Input.GetKeyDown(KeyCode.W))    { jumpKey   = true; }
        if (Input.GetKeyDown(KeyCode.S))  { duckKey   = true; }
        if (Input.GetKeyDown(KeyCode.Mouse0))          { shootKey  = true; }

        //TODO maybe put this out of beat sync
        if (Input.GetKeyDown(KeyCode.Mouse1)) { aimKey = true; }
       
        
        
        playerVertical = 0;
        //avoid same frame error
        if (jumpKey) { playerVertical-=1; }
        else if (duckKey) { playerVertical+=1; }

        playerAim = aimKey;
        playerShoot = shootKey;



    }


    
    void GoCam() {

        camX = 480 - camWidth/2;
        camY = 600 - camHeight/2;

        gfx.MoveLevel(camX, camY);
       
    }
    


    public void AddLevelObject(GeneralObject inObj) {

        gameObjects.Add(inObj);
        gameObjectLength++;

    }



    void GoObjects(bool inDoActive=true) {

        for (int i = 0; i<gameObjectLength; i++) {

            if (!gameObjects[i].FrameEvent()) {
                gameObjects.RemoveAt(i);
                i--;
                gameObjectLength--;
            }
        }

    }



}