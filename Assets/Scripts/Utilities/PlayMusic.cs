using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioSource song;

    private void Start()
    {
        song.Play();
    }
}
