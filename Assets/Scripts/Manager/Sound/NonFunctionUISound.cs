using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NonFunctionUISound : MonoBehaviour
{
    public void PlayCommonButtonSE()
    {
        if (SoundManager.instance != null)
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");
    }

    public void PlayConfirmButtonSE()
    {
        if (SoundManager.instance != null)
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Confirm_Button");
    }

    public void PlayCancleButtonSE()
    {
        if (SoundManager.instance != null)
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Cancle_Button");
    }

    public void PlayButtonFailSE()
    {
        if (SoundManager.instance != null)
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Button_Fail");
    }

    public void PlayTowerButtonSE()
    {
        if (SoundManager.instance != null)
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Tower_Button");
    }
}
