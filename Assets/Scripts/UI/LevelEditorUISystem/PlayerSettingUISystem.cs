using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingUISystem : MonoBehaviour
{
    private int playerLife;
    private int startCost;
    
    [Header("Player Setting Panel")]
    public GameObject playerSettingPanel;
    public GameObject playerLifeInputField;
    public GameObject startCostInputField;

    private void Awake()
    {
        this.playerLife = -1;
        this.startCost = -1;
    }

    public void OnClickSaveButtonInPlayerSettingPanel()
    {
        if (!int.TryParse(this.playerLifeInputField.GetComponent<TMP_InputField>().text, out this.playerLife)
            && !int.TryParse(this.startCostInputField.GetComponent<TMP_InputField>().text, out this.startCost))
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(0));
            this.playerLife = -1;
            this.startCost = -1;
        }
        else if (!int.TryParse(this.playerLifeInputField.GetComponent<TMP_InputField>().text, out this.playerLife))
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(1));
            this.playerLife = -1;
        }
        else if (!int.TryParse(this.startCostInputField.GetComponent<TMP_InputField>().text, out this.startCost))
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(2));
            this.startCost = -1;
        }
        else
        {
            LevelEditor.instance.SetPlayerLife(this.playerLife);
            LevelEditor.instance.SetStartCost(this.startCost);

            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(3));
            this.playerSettingPanel.SetActive(false);

            if (LevelEditor.instance.GetPlayerLife() != -1 && LevelEditor.instance.GetStartCost() != -1)
                LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                    LevelEditorUISystem.ButtonColor.ReadyColor);
        }

        if(this.playerLife == -1)
            this.playerLifeInputField.GetComponent<TMP_InputField>().text = "";
        if(this.startCost == -1)
            this.startCostInputField.GetComponent<TMP_InputField>().text = "";
    }

    public void OnClickResetButtonInPlayerSettingPanel()
    {
        this.playerLife = -1;
        this.startCost = -1;

        if (this.playerLife == -1)
            this.playerLifeInputField.GetComponent<TMP_InputField>().text = "";
        if (this.startCost == -1)
            this.startCostInputField.GetComponent<TMP_InputField>().text = "";

        LevelEditor.instance.SetPlayerLife(this.playerLife);
        LevelEditor.instance.SetStartCost(this.startCost);
        
        if (LevelEditor.instance.GetPlayerLife() == -1 && LevelEditor.instance.GetStartCost() == -1)
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                LevelEditorUISystem.ButtonColor.NotReadyColor);
    }

    public void OnClickCancelButton()
    {
        if (this.playerLife == -1 && this.startCost == -1)
            this.playerSettingPanel.SetActive(false);
        else
        {
            if (int.TryParse(this.playerLifeInputField.GetComponent<TMP_InputField>().text, out this.playerLife)
                && int.TryParse(this.startCostInputField.GetComponent<TMP_InputField>().text, out this.startCost))
            {
                if (LevelEditor.instance.GetPlayerLife() == this.playerLife
                    && LevelEditor.instance.GetStartCost() == this.startCost)
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
        int num = -1;

        if (!int.TryParse(inputField.GetComponent<TMP_InputField>().text, out num))
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(flag));
            if (flag == 1)
                this.playerLife = -1;
            else
                this.startCost = -1;
        }
        else
        {
            if (flag == 1)
            { // playerLife
                if (!((1 <= num) && (num <= 20)))
                {
                    this.playerLife = -1;
                    StartCoroutine(LevelEditorUISystem.instance.ShowMessage(flag));
                    inputField.GetComponent<TMP_InputField>().text = "";
                }
                else
                    this.playerLife = num;
            }
            else if (flag == 2)
            { // costStart
                if (!((50 <= num) && (num <= 2000)))
                {
                    this.startCost = -1;
                    StartCoroutine(LevelEditorUISystem.instance.ShowMessage(flag));
                    inputField.GetComponent<TMP_InputField>().text = "";
                }
                else
                    this.startCost = num;
            }
        }
    }

}
