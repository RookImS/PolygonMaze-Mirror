using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingUISystem : MonoBehaviour
{
    [Header("Player Setting Panel")]
    public GameObject playerSettingPanel;
    public GameObject playerLifeInputField;
    public GameObject startCostInputField;

    public void OnClickSaveButtonInPlayerSettingPanel()
    {
        int playerLife = 0;
        int startCost = 0;

        if (!int.TryParse(this.playerLifeInputField.GetComponent<TMP_InputField>().text, out playerLife)
            && !int.TryParse(this.startCostInputField.GetComponent<TMP_InputField>().text, out startCost))
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(0));
            this.playerLifeInputField.GetComponent<TMP_InputField>().text = "";
            this.startCostInputField.GetComponent<TMP_InputField>().text = "";
        }
        else if (!int.TryParse(this.playerLifeInputField.GetComponent<TMP_InputField>().text, out playerLife))
        {
            Debug.Log(playerLife);
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(1));
            this.playerLifeInputField.GetComponent<TMP_InputField>().text = "";
        }
        else if (!int.TryParse(this.startCostInputField.GetComponent<TMP_InputField>().text, out startCost))
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(2));
            this.playerLifeInputField.GetComponent<TMP_InputField>().text = "";
        }
        else
        {
            LevelEditor.instance.SetPlayerLife(playerLife);
            LevelEditor.instance.SetStartCost(startCost);

            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(3));
            this.playerSettingPanel.SetActive(false);

            if (LevelEditor.instance.GetPlayerLife() != -1 && LevelEditor.instance.GetStartCost() != -1)
                LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                    LevelEditorUISystem.ButtonColor.ReadyColor);
        }
    }

    public void OnClickResetButtonInPlayerSettingPanel()
    {
        LevelEditor.instance.SetPlayerLife(-1);
        LevelEditor.instance.SetStartCost(-1);

        this.playerLifeInputField.GetComponent<TMP_InputField>().text = "";
        this.startCostInputField.GetComponent<TMP_InputField>().text = "";

        if(LevelEditor.instance.GetPlayerLife() == -1 && LevelEditor.instance.GetStartCost() == -1)
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                LevelEditorUISystem.ButtonColor.NotReadyColor);
    }

    public void OnClickCancelButton()
    {
        int playerLife = 0;
        int startCost = 0;

        if (this.playerLifeInputField.GetComponent<TMP_InputField>().text == ""
            && this.startCostInputField.GetComponent<TMP_InputField>().text == "")
            this.playerSettingPanel.SetActive(false);
        else
        {
            if (int.TryParse(this.playerLifeInputField.GetComponent<TMP_InputField>().text, out playerLife)
                && int.TryParse(this.startCostInputField.GetComponent<TMP_InputField>().text, out startCost))
            {
                if (LevelEditor.instance.GetPlayerLife() == playerLife
                    && LevelEditor.instance.GetStartCost() == startCost)
                    this.playerSettingPanel.SetActive(false);
                else
                    LevelEditorUISystem.instance.ShowWarningPanel(this.playerSettingPanel);
            }
        }

        
    }

    public void OnEndInPlayerLifeInputField()
    {
        OnEndInputField(this.playerLifeInputField, 1);
    }

    public void OnEndInStartCostInputField()
    {
        OnEndInputField(this.startCostInputField, 2);
    }

    private void OnEndInputField(GameObject inputField, int flag)
    {
        int num = 0;

        if (!int.TryParse(inputField.GetComponent<TMP_InputField>().text, out num))
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(flag));
            inputField.GetComponent<TMP_InputField>().text = "";
        }
        else
        {
            if (flag == 1) // playerLife
            {
                if (!((1 <= num) && (num <= 20)))
                {
                    StartCoroutine(LevelEditorUISystem.instance.ShowMessage(flag));
                    inputField.GetComponent<TMP_InputField>().text = "";
                }
            }
            else if (flag == 2) // costStart
            {
                if (!((50 <= num) && (num <= 2000)))
                {
                    StartCoroutine(LevelEditorUISystem.instance.ShowMessage(flag));
                    inputField.GetComponent<TMP_InputField>().text = "";
                }
            }
        }
    }

}
