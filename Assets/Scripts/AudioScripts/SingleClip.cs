using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleClip
{
    AudioClip myClip;
    public SingleClip(AudioClip tmpClip)
    {
        myClip = tmpClip;
    }
    public void Play(AudioSource tmpSource)
    {
        tmpSource.clip = myClip;
        tmpSource.Play();

    }
}

