using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour, IRecieveBeats
{
    const string fadeout = "FadeOut";

    Camera cam;
    [SerializeField]float zoomTime = 100;
    [SerializeField]float defaultSize;

    Animator animator;

    private void Start()
    {
        Invoke(nameof(OnStart),.1f);
    }
    private void OnStart()
    {
        AddToList();
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!cam)
            return;

        cam.orthographicSize += Time.deltaTime * zoomTime;
    }

    public void FadeOut()
    {
        animator.Play(fadeout);
    }
    public void AddToList()
    {
        Conductor.instance.recievers.Add(this);
    }

    public void BeatUpdate()
    {
        cam.orthographicSize = defaultSize;
    }

    public void OffBeatUpdate()
    {

    }
}
