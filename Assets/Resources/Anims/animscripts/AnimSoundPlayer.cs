using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSoundPlayer : MonoBehaviour
{
    AudioSource source;

    private void Start()
    {
        source = Camera.main.GetComponent<AudioSource>();
    }


    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
