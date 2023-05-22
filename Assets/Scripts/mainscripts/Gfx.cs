using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Gfx : MonoBehaviour
{
	Main main;
	public int myRes;

    public int screenWidth;
    public int screenHeight;
    public Vector3 myResVector;
    public Vector3 resetVector;
    public Camera cam;

    public int levelWidth;
    public int levelHeight;

    public GameObject level;
    private Vector3 levelPos;

    public GameObject background;

    public GameObject fadeObject;
    public Sprite blackSprite;

    Dictionary<string, Sprite[]> levelSprites;

    public void Init(Main inMain) {

        main = inMain;

        // --------------------------------------------------------------------------------
        // Setup Screen Resolution
        // --------------------------------------------------------------------------------

        screenWidth = 1920;// main.screenWidth;
        screenHeight = 1080;// main.screenHeight;
		resetVector  = new Vector3(1.0f, 1.0f, 1.0f);
		myRes        = Mathf.CeilToInt(screenHeight / 267F);
        
        int tMyRes = Mathf.CeilToInt (screenWidth / 440F);
		if (tMyRes > myRes){myRes = tMyRes;}

        myResVector  = new Vector3(myRes, myRes, 1.0f);

        // --------------------------------------------------------------------------------
        // Setup Camera
        // --------------------------------------------------------------------------------

        cam = main.cam;
		cam.pixelRect = new Rect(0, 0, screenWidth, screenHeight);
        cam.orthographicSize = screenHeight;
        cam.transform.position = new Vector3(1200, -screenHeight / 2, -10f);

        // --------------------------------------------------------------------------------
        // Setup Level
        // --------------------------------------------------------------------------------

        level = new GameObject("Level");
		level.transform.localScale = myResVector;
        level.transform.position = new Vector3(0, 0, 1);
        levelPos = level.transform.position;

        levelSprites = new Dictionary<string, Sprite[]>();

        MakeGameObject("Level1", Resources.Load<Sprite>("Level/Level"),0,0,"Level");

        background = MakeGameObject("Background", Resources.Load<Sprite>("Level/Background"),screenWidth/2/myRes, screenHeight/2/myRes,"Background");
        SetParent(background, null);
        SetScale(background, screenWidth/100, screenHeight/100);

    }



    public Sprite[] GetLevelSprites(string inName) {

        if (!levelSprites.ContainsKey(inName)) {
            levelSprites.Add(inName, Resources.LoadAll<Sprite>(inName));
        }

        return levelSprites[inName];

    }



    public void MoveLevel(float inX, float inY) {

        SetPos(level, -inX*myRes, -inY*myRes);

    }



    public GameObject MakeGameObject(string inName, Sprite inSprite, float inX = 0, float inY = 0, string inLayerName="GameObjects") {

        GameObject o = new GameObject(inName);
        SpriteRenderer sr = o.AddComponent<SpriteRenderer>();
    
        o.transform.parent = level.transform;
        sr.sprite = inSprite;
        sr.sortingLayerName = inLayerName;
        o.transform.localScale = resetVector;

        Vector3 tPos = o.transform.localPosition;
        tPos.x = inX; tPos.y = -inY;
        o.transform.localPosition = tPos;

        return o;

    }
    public GameObject AddGameObjectWithAnimator<T>(string inName, Sprite inSprite, out SpriteRenderer renderer, Transform parent, RuntimeAnimatorController animatorController, T charAnim, float inX = 0, float inY = 0, string inLayerName = "GameObjects",GameObject[] customChildren = null) where T : CharacterAnimationBase
    {

        GameObject o = new GameObject(inName);
        SpriteRenderer sr = o.AddComponent<SpriteRenderer>();
        Animator an = o.AddComponent<Animator>();
        //need to set customChild parameter here, refs in animations won't update if added later because unity reasons
        foreach (var child in customChildren)
        {
            if (child != null) 
            {
                child.transform.parent = o.transform;
            }
        }

        o.AddComponent<T>();



        an.runtimeAnimatorController = animatorController;
        sr.sprite = inSprite;
        o.transform.parent = parent;
        sr.sortingLayerName = inLayerName;
        o.transform.localScale = resetVector;
        Vector3 tPos = o.transform.localPosition;
        tPos.x = inX; tPos.y = -inY;
        o.transform.localPosition = tPos;

        renderer = sr;

        return o;

    }



    public void SetParent(GameObject g, Transform inParent) {

        g.transform.parent = inParent;

    }



	public void SetScale(GameObject g, float inX, float inY){

		Vector3 v = g.transform.localScale;
		v.x = inX;
		v.y = inY;
		g.transform.localScale = v;

	}



	public void SetScaleX(GameObject g, float inX){

		Vector3 v = g.transform.localScale;
		v.x = inX;
		g.transform.localScale = v;

	}



	public void SetPos(GameObject g, float inX, float inY){

		Vector3 v = g.transform.localPosition;
		v.x = inX;
		v.y = -inY;
		g.transform.localPosition = v;

	}



	public void SetPosX(GameObject g, float inX){

		Vector3 v = g.transform.localPosition;
		v.x = inX;
		g.transform.localPosition = v;

	}



	public void SetPosY(GameObject g, float inY){

		Vector3 v = g.transform.localPosition;
		v.y = -inY;
		g.transform.localPosition = v;

	}



    public void SetDirX(GameObject g, int inDir) {

        Vector3 v = g.transform.localScale;
        v.x = Mathf.Abs(v.x)*inDir;
        g.transform.localScale = v;

    }



	public void SetSprite(GameObject g, Sprite s){

		g.GetComponent<SpriteRenderer> ().sprite = s;

	}


}