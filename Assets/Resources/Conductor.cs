using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor instance;

    public List<IRecieveBeats> recievers = new List<IRecieveBeats>();

    //get from soundplayer reference on maincamera
    private AudioSource musicSource;

    //change in scene view for songs
    public float songBpm;
    public float nrOfSongBeats;

    //seconds for each beat
    private float secPerBeat;

    //anim loop freq
    public float animSpeed;

    //pos in seconds
    private float songPosition;

    //current song pos in beats
    public float songPositionInBeats;

    private float dspSongTime;
    public float distanceFromBeat;
    private float closestBeat;

    //times in seconds to next beat
    public float timeUntilNext;

    private float nextBeat = 0;
    private float nextOffBeat = 0.5f;

    private float juiceOffset = 0f;

    void Start()
    {
        instance = this;

        secPerBeat = 60f / songBpm;

        animSpeed = 1/secPerBeat;

        musicSource = GetComponent<AudioSource>();
        musicSource.Play();
        //time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

    }

    void Update()
    {
        //seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime-juiceOffset);

        //beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        closestBeat = Mathf.RoundToInt(songPositionInBeats);
        distanceFromBeat = closestBeat - songPositionInBeats;
        timeUntilNext = nextBeat - songPositionInBeats;

        //progresses beats
        if (distanceFromBeat < 0)
            distanceFromBeat *= -1;

        if (songPositionInBeats > nextBeat)
            Beat();

        if (songPositionInBeats > nextOffBeat)
            OffBeat();

        if (songPositionInBeats > nrOfSongBeats)
        {
            ResetLoop();
        }
    }
    public void ResetLoop()
    {
        Start();
        songPosition = 0;
        songPositionInBeats = 0;
        nextBeat = 0;
        nextOffBeat = .5f;
    }

    void Beat()
    {
        nextBeat++;

        foreach (var reciever in recievers)
        {
            reciever.BeatUpdate();
        }
    }
    void OffBeat()
    {
        nextOffBeat++;

        foreach (var reciever in recievers)
        {
            reciever.OffBeatUpdate();
        }
    }

}


public interface IRecieveBeats
{
    void BeatUpdate();
    void OffBeatUpdate();
    void AddToList();
}