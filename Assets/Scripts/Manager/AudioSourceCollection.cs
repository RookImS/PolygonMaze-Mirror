using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceCollection : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] public AudioSource[] audioSources;
    [SerializeField] public AudioSource[] audioSourcesForMute;
}
