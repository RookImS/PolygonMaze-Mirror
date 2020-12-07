using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NonFunctionUISound : MonoBehaviour
{
    public void PlayCommonButtonSE()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");
    }

    public void PlayConfirmButtonSE()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Confirm_Button");
    }

    public void PlayCancleButtonSE()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Cancle_Button");
    }

    public void PlayButtonFailSE()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Button_Fail");
    }

    public void PlayTowerButtonSE()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Tower_Button");
    }
}
