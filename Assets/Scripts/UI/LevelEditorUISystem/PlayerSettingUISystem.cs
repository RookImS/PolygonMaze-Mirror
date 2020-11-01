using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerSettingUISystem : MonoBehaviour
{
    private static int playerLifeMin = 1;
    private static int playerLifeMax = 50;

    private static int startCostMin = 50;
    private static int startCostMax = 2000;
    
    [Header("Player Setting Panel")]
    public GameObject playerSettingPanel;
    public TMP_InputField playerLifeInputField;
    public TMP_InputField startCostInputField;

    /* void OnClickResetButtonInPlayerSettingPanel()
     * 1. Initialize player life and start cost to 0 in LevelEditor.
     * 2. Initialize the text in the input field of player life and input field of start cost to "".
     * 3. Change the color of the player setting button to NotReadyColor.
     */
    public void OnClickResetButtonInPlayerSettingPanel()
    {
        LevelEditor.instance.SetPlayerLife(0);
        LevelEditor.instance.SetStartCost(0);

        if (LevelEditor.instance.GetPlayerLife() == 0)
            this.playerLifeInputField.text = "";
        if (LevelEditor.instance.GetStartCost() == 0)
            this.startCostInputField.text = "";

        if (LevelEditor.instance.GetPlayerLife() == 0 && LevelEditor.instance.GetStartCost() == 0)
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                LevelEditorUISystem.ButtonColor.NotReadyColor);
        
    }

    /* void OnClickCancelButton()
     * 1. close player setting panel.
     */
    public void OnClickCancelButton()
    {
        playerSettingPanel.SetActive(false);
    }

    /* void OnEndInPlayerLifeInputField()
     * 1. if the value in player life input field is a valid value,
     *    player life in LevelEditor become the value.
     *    else player life in LevelEditor become 0.
     * 2. if player life in LevelEditor is 0,
     *    Text of player life input field become "".
     * 3. check if the player setting is complete.
     */
    public void OnEndInPlayerLifeInputField()
    {
        TMP_InputField inputField = this.playerLifeInputField;

        int num = 0;
        if (!int.TryParse(inputField.text, out num))
            LevelEditor.instance.SetPlayerLife(0);
        else
        {
            if (!((playerLifeMin <= num) && (num <= playerLifeMax)))
                LevelEditor.instance.SetPlayerLife(0);
            else
                LevelEditor.instance.SetPlayerLife(num);
        }

        if (LevelEditor.instance.GetPlayerLife() == 0)
            this.playerLifeInputField.text = "";

        CheckSettingComplete();
    }

    /* void OnEndInPlayerLifeInputField()
     * 1. if the value in start cost input field is a valid value,
     *    start cost in LevelEditor become the value.
     *    else start cost in LevelEditor become 0.
     * 2. if start cost in LevelEditor is 0,
     *    Text of start cost input field become "".
     * 3. check if the player setting is complete.
     */
    public void OnEndInStartCostInputField()
    {
        TMP_InputField inputField = this.startCostInputField;

        int num = 0;
        if (!int.TryParse(inputField.text, out num))
            LevelEditor.instance.SetStartCost(0);
        else
        {
            if (!((startCostMin <= num) && (num <= startCostMax)))
                LevelEditor.instance.SetStartCost(0);
            else
                LevelEditor.instance.SetStartCost(num);
        }

        if (LevelEditor.instance.GetStartCost() == 0)
            this.startCostInputField.text = "";

        CheckSettingComplete();
    }

    /* void CheckSettingComplete()
     * 1. if player life and start cost in LevelEditor are 0, show message(0).
     *    else if player life in LevelEditor is 0, show message(1).
     *    else if start cost in LevelEditor is 0, show message(2).
     *    else show message(3).
     */

    public void CheckSettingComplete()
    {
        bool readyFlag = true;

        if(LevelEditor.instance.GetPlayerLife() == 0)
        {
            readyFlag = false;

            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(0, playerLifeMin, playerLifeMax));
        }
        else if (LevelEditor.instance.GetStartCost() == 0)
        {
            readyFlag = false;

            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(1, startCostMin, startCostMax));
        }

        if(readyFlag == false)
        {
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                LevelEditorUISystem.ButtonColor.NotReadyColor);
        }
        else
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(2));

            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                LevelEditorUISystem.ButtonColor.ReadyColor);
        }
    }
}
