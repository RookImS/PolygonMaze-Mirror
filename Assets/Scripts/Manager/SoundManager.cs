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
    public AudioSourceCollection skillLoopAudioSourceCollection;
    public AudioSourceCollection skillAudioSourceCollection;
    public AudioSourceCollection towerAudioSourceCollection;
    public AudioSourceCollection otherAudioSourceCollection;
    private AudioSource[] skillLoopAudioPlayer;
    private AudioSource[] skillAudioPlayer;
    private AudioSource[] towerAudioPlayer;
    private AudioSource[] otherAudioPlayer;
    private AudioSource[] audioPlayerForMute;

    private bool isMuteBGM;
    private bool isMuteSE;
    private float bgmVolume;
    private float seVolume;

    public enum SoundSpecific
    {
        BUTTON,
        OTHERUI,
        ENEMY
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
        skillLoopAudioPlayer = skillLoopAudioSourceCollection.audioSources;
        skillAudioPlayer = skillAudioSourceCollection.audioSources;
        towerAudioPlayer = towerAudioSourceCollection.audioSources;
        otherAudioPlayer = otherAudioSourceCollection.audioSources;
        audioPlayerForMute = otherAudioSourceCollection.audioSourcesForMute;

        isMuteBGM = false;
        isMuteSE = false;
        bgmVolume = 1.0f;
        seVolume = 1.0f;

        PlayBGM("OutGame_BGM");
    }

    public void StopBGMPlayer()
    {
        bgmPlayer.Stop();
    }

    public void PlayBGM(string soundName)
    {
        Sound[] sounds = bgmSounds;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (soundName == sounds[i].soundName)
            {
                if (bgmPlayer.clip != null)
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
                for (int j = 0; j < otherAudioPlayer.Length; j++)
                {
                    if (!otherAudioPlayer[j].isPlaying)
                    {
                        otherAudioPlayer[j].clip = sounds[i].clip;
                        if (soundName == "Player_Game_Clear_Sound"
                            || soundName == "Player_Game_Over_Sound")
                            otherAudioPlayer[j].PlayDelayed(0.5f);
                        else
                            otherAudioPlayer[j].Play();
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
                for (int j = 0; j < towerAudioPlayer.Length; j++)
                {
                    if (!towerAudioPlayer[j].isPlaying)
                    {
                        towerAudioPlayer[j].clip = sounds[i].clip;
                        towerAudioPlayer[j].Play();
                        return;
                    }
                }

                Debug.Log("Tower Audio Source is full");
                return;
            }  
        }
    }

    public void PlaySoundForMute(string soundName)
    {
        Sound[] sounds = buttonSounds;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (soundName == sounds[i].soundName)
            {
                for (int j = 0; j < audioPlayerForMute.Length; j++)
                {
                    if (!audioPlayerForMute[j].isPlaying)
                    {
                        audioPlayerForMute[j].clip = sounds[i].clip;
                        audioPlayerForMute[j].Play();
                        return;
                    }
                }

                Debug.Log("Audio Source For Mute is full");
                return;
            }
        }
    }


    public void PlaySkillSound(SkillSoundSpecific skillSoundSpecific, string soundName)
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
                for (int j = 0; j < skillAudioPlayer.Length; j++)
                {
                    if (!skillAudioPlayer[j].isPlaying)
                    {
                        skillAudioPlayer[j].clip = sounds[i].clip;
                        skillAudioPlayer[j].Play();
                        return;
                    }
                }

                Debug.Log("Skill Audio Source is full");
                return;
            }  
        }
    }

    public AudioSource GetLoopSkillAudio()
    {
        for (int i = 0; i < skillLoopAudioPlayer.Length; i++)
        {
            if (!skillLoopAudioPlayer[i].isPlaying)
            {
                return skillLoopAudioPlayer[i];
            }
        }

        Debug.Log("Skill Audio Source is full");
        return null;
    }

    public void PlayLoopSkillSound(AudioSource audioSource, SkillSoundSpecific skillSoundSpecific, string soundName)
    {
        Sound[] sounds;

        if (audioSource == null)
            return;

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
                audioSource.clip = sounds[i].clip;
                audioSource.Play();
                break;
            }
        }
    }

    public void StopAudio(AudioSource audioSource)
    {
        if (audioSource != null)
            audioSource.Stop();
    }

    public void ControlBGMPlayerSound(float value)
    {
        bgmVolume = value;
        bgmPlayer.volume = value;
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }

    public void ControlBGMPlayerMute(bool ismute)
    {
        bgmPlayer.mute = ismute;
        isMuteBGM = ismute;
    }

    public bool GetIsMuteBGM()
    {
        return isMuteBGM;
    }

    public void ControlSEPlayerSound(float value)
    {
        seVolume = value;

        foreach (AudioSource audioSource in skillLoopAudioPlayer)
            audioSource.volume = value;

        foreach (AudioSource audioSource in skillAudioPlayer)
            audioSource.volume = value;
        
        foreach(AudioSource audioSource in towerAudioPlayer)
            audioSource.volume = value;

        foreach (AudioSource audioSource in otherAudioPlayer)
            audioSource.volume = value;
    }

    public float GetSEVolume()
    {
        return seVolume;
    }

    public void ControlSEPlayerMute(bool ismute)
    {
        foreach (AudioSource audioSource in skillLoopAudioPlayer)
            audioSource.mute = ismute;

        foreach (AudioSource audioSource in skillAudioPlayer)
            audioSource.mute = ismute;

        foreach (AudioSource audioSource in towerAudioPlayer)
            audioSource.mute = ismute;

        foreach (AudioSource audioSource in otherAudioPlayer)
            audioSource.mute = ismute;

        isMuteSE = ismute;
    }

    public bool GetIsMuteSE()
    {
        return isMuteSE;
    }

    public void SetIsMuteSE(bool isMuteSE)
    {
        this.isMuteSE = isMuteSE;
    }
}
