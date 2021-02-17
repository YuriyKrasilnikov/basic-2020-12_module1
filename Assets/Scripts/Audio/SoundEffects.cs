using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private EffectsDataSound effectsDataSound;

    void Start()
    {

        //var character = GetComponent<Character>();

        //var state = GetComponent<Character>().state;

        //Debug.Log( state );
    }


    public void Play(string nameClip)
    {
        var audioClip = effectsDataSound.GetAudioClip(nameClip);
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
    
}