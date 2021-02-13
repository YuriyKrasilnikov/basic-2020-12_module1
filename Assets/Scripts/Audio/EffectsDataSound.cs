using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsDataSound : MonoBehaviour
{
    [SerializeField]
    private List<DataSound> dataSounds = new List<DataSound>();

    public AudioClip GetAudioClip(string nameClip)
    {
        AudioClip clip = null;
        
        foreach (var sound in dataSounds)
        {
            if (sound.name == nameClip)
                clip = sound.audioClip;
        }

        return clip;
    }

    [Serializable]
    private class DataSound
    {
        public string name;
        public AudioClip audioClip;
    }

}
