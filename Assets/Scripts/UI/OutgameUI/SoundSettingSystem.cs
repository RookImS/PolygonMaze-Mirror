using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSettingSystem : MonoBehaviour
{
    public GameObject bgmSpeakerImage;
    public GameObject bgmVolumeText;
    public GameObject bgmSlider;

    public GameObject seSpeakerImage;
    public GameObject seVolumeText;
    public GameObject seSlider;

    public Sprite notMuteSpeaker;
    public Sprite muteSpeaker;

    private void Start()
    {
        Init();  
    }

    public void Init()
    {
        bgmSlider.GetComponent<Slider>().value = SoundManager.instance.GetBGMVolume();
        bgmVolumeText.GetComponent<TMP_Text>().text
            = Mathf.CeilToInt(bgmSlider.GetComponent<Slider>().value * 100).ToString();

        if (bgmSlider.GetComponent<Slider>().value <= 0)
            bgmSpeakerImage.GetComponent<Image>().sprite = muteSpeaker;
        else
            bgmSpeakerImage.GetComponent<Image>().sprite = notMuteSpeaker;

        if (SoundManager.instance.GetIsMuteBGM())
            bgmSpeakerImage.GetComponent<Image>().sprite = muteSpeaker;
        else
            bgmSpeakerImage.GetComponent<Image>().sprite = notMuteSpeaker;

        seSlider.GetComponent<Slider>().value = SoundManager.instance.GetSEVolume();
        seVolumeText.GetComponent<TMP_Text>().text
            = Mathf.CeilToInt(seSlider.GetComponent<Slider>().value * 100).ToString();

        if (seSlider.GetComponent<Slider>().value <= 0)
            seSpeakerImage.GetComponent<Image>().sprite = muteSpeaker;
        else
            seSpeakerImage.GetComponent<Image>().sprite = notMuteSpeaker;

        if (SoundManager.instance.GetIsMuteSE())
            seSpeakerImage.GetComponent<Image>().sprite = muteSpeaker;
        else
            seSpeakerImage.GetComponent<Image>().sprite = notMuteSpeaker;
    }


    public void OnClickBGMButton()
    {
        SoundManager.instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");

        if (SoundManager.instance.GetIsMuteBGM())
        {
            SoundManager.instance.ControlBGMPlayerMute(false);
            bgmSpeakerImage.GetComponent<Image>().sprite = notMuteSpeaker;
        }
        else
        {
            SoundManager.instance.ControlBGMPlayerMute(true);
            bgmSpeakerImage.GetComponent<Image>().sprite = muteSpeaker;
        }
    }

    public void OnChangedBGMSlider()
    {
        if (bgmSlider.GetComponent<Slider>().value <= 0)
            bgmSpeakerImage.GetComponent<Image>().sprite = muteSpeaker;
        else if(SoundManager.instance.GetBGMVolume() <= 0)
            bgmSpeakerImage.GetComponent<Image>().sprite = notMuteSpeaker;

        SoundManager.instance.ControlBGMPlayerSound(bgmSlider.GetComponent<Slider>().value);
        bgmVolumeText.GetComponent<TMP_Text>().text
            = Mathf.CeilToInt(bgmSlider.GetComponent<Slider>().value * 100).ToString();
    }

    public void OnClickSEButton()
    {
        SoundManager.instance.PlaySoundForMute("Common_Button");

        if (SoundManager.instance.GetIsMuteSE())
        {
            SoundManager.instance.ControlSEPlayerMute(false);
            seSpeakerImage.GetComponent<Image>().sprite = notMuteSpeaker;
        }
        else
        {
            SoundManager.instance.ControlSEPlayerMute(true);
            seSpeakerImage.GetComponent<Image>().sprite = muteSpeaker;
        }
    }

    public void OnChangedSESlider()
    {
        if (seSlider.GetComponent<Slider>().value <= 0)
            seSpeakerImage.GetComponent<Image>().sprite = muteSpeaker;
        else if (SoundManager.instance.GetSEVolume() <= 0)
            seSpeakerImage.GetComponent<Image>().sprite = notMuteSpeaker;

        SoundManager.instance.ControlSEPlayerSound(seSlider.GetComponent<Slider>().value);
        seVolumeText.GetComponent<TMP_Text>().text
            = Mathf.CeilToInt(seSlider.GetComponent<Slider>().value * 100).ToString();
    }
}
