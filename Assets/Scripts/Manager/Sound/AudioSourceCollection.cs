using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceCollection : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] public List<AudioSource> audioSources;
    [SerializeField] public List<AudioSource> audioSourcesForMute;
}
