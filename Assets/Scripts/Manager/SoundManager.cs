using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoSingleton<SoundManager>
{
    [System.Serializable]
    public class Sound
    {
        public string soundName;
        public AudioClip clip;
    }

    [Header("Sound Register")]
    [Header("BGM Sound Register")]
    [SerializeField] public Sound[] bgmSounds;

    [Header("Button Sound Register")]
    [SerializeField] public Sound[] buttonSounds;

    [Header("Other UI Sound Register")]
    [SerializeField] public Sound[] otherUISounds;

    [Header("Eenmy Sound Register")]
    [SerializeField] public Sound[] enemySounds;

    [Header("Tower Sound Register")]
    [SerializeField] public Sound[] towerSounds;

    [Header("Skill Sound Register")]
    [Header("Yellow Sound Register")]
    [SerializeField] public Sound[] yellowSounds;
    [Header("Red Sound Register")]
    [SerializeField] public Sound[] redSounds;
    [Header("Green Sound Register")]
    [SerializeField] public Sound[] greenSounds;
    [Header("Blue Sound Register")]
    [SerializeField] public Sound[] blueSounds;

    [Header("Audio Source")]
    [SerializeField] public AudioSource bgmPlayer;
    [SerializeField] public AudioSource[] audioPlayer;

    public enum SoundSpecific
    {
        BUTTON,
        OTHERUI,
        Enemy
    }

    public enum TowerSoundSpecific
    {
        TRIANGLE,
        SQUARE,
        PENTAGON,
        HEXAGON
    }

    public enum SkillSoundSpecific
    {
        YELLOW,
        RED,
        GREEN,
        BLUE
    }

    private void Start()
    {
        PlayBGM("OutGame_BGM");
    }

    public void PlayBGM(string soundName)
    {
        Sound[] sounds = bgmSounds;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (soundName == sounds[i].soundName)
            {
                if(bgmPlayer.clip != null)
                    if (soundName == "OutGame_BGM" && bgmPlayer.clip.name == "OutGame_BGM")
                            return;
                
                bgmPlayer.clip = sounds[i].clip;
                bgmPlayer.PlayDelayed(1.5f);
                return;
            }
        }
    }
  

    public void PlaySound(SoundSpecific soundSpecific, string soundName)
    {
        Sound[] sounds;

        if (soundSpecific == SoundSpecific.BUTTON)
            sounds = buttonSounds;
        else if (soundSpecific == SoundSpecific.OTHERUI)
            sounds = otherUISounds;
        else
            sounds = enemySounds;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (soundName == sounds[i].soundName)
            {
                for (int j = 0; j < audioPlayer.Length; j++)
                {
                    if (!audioPlayer[j].isPlaying)
                    {
                        audioPlayer[j].clip = sounds[i].clip;
                        audioPlayer[j].Play();
                        return;
                    }
                }

                Debug.Log("Audio Source is full");
                return;
            }
        }
    }

    public void PlaySound(TowerSoundSpecific towerSoundSpecific, string soundName)
    {
        Sound[] sounds = towerSounds;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (soundName == sounds[i].soundName)
            {
                for (int j = 0; j < audioPlayer.Length; j++)
                {
                    if (!audioPlayer[j].isPlaying)
                    {
                        audioPlayer[j].clip = sounds[i].clip;
                        audioPlayer[j].Play();
                        return;
                    }
                }

                Debug.Log("Audio Source is full");
                return;
            }  
        }
    }


    public void PlaySound(SkillSoundSpecific skillSoundSpecific, string soundName)
    {
        Sound[] sounds;

        if (skillSoundSpecific == SkillSoundSpecific.YELLOW)
            sounds = yellowSounds;
        else if (skillSoundSpecific == SkillSoundSpecific.RED)
            sounds = redSounds;
        else if (skillSoundSpecific == SkillSoundSpecific.GREEN)
            sounds = greenSounds;
        else
            sounds = blueSounds;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (soundName == sounds[i].soundName)
            {
                for (int j = 0; j < audioPlayer.Length; j++)
                {
                    if (!audioPlayer[j].isPlaying)
                    {
                        audioPlayer[j].clip = sounds[i].clip;
                        audioPlayer[j].Play();
                        return;
                    }
                }

                Debug.Log("Audio Source is full");
                return;
            }  
        }
    }
}
