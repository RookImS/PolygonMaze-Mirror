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

    public void Init()
    {
        playerLifeInputField.text = "";
        startCostInputField.text = "";

        LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                LevelEditorUISystem.ButtonColor.NotReadyColor);
    }

    /* void OnClickResetButtonInPlayerSettingPanel()
     * 1. Level Editor의 player life와 start cost를 0으로 초기화한다.
     * 2. player life와 start cost의 input field의 text를 ""으로 초기화한다.
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
        {
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                LevelEditorUISystem.ButtonColor.NotReadyColor);

            LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting
                , false);

        }
        
    }


    /* void OnEndInPlayerLifeInputField()
     * 1. 만약 inputf field의 player life 값이 유효한 값이면
     *    LevelEditor의 player life 값이 해당 player life 값이 되고,
     *    유효하지 않은 값이라면, LevelEditor의 player life 값이 0이 된다.
     * 2. 만약 LevelEditor의 player life 값이 0이면,
     *    player life의 input field의 text가 ""이 된다.
     * 3. player setting이 완료되었는지 확인한다.
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

    /* void OnEndInStartCostInputField()
     * 1. 만약 inputf field의 start cost 값이 유효한 값이면
     *    LevelEditor의 start cost 값이 해당 start cost 값이 되고,
     *    유효하지 않은 값이라면, LevelEditor의 start cost 값이 0이 된다.
     * 2. 만약 LevelEditor의 start cost 값이 0이면,
     *    start cost의 input field의 text가 ""이 된다.
     * 3. player setting이 완료되었는지 확인한다.
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
     * 1. LevelEditor의 player life가 0이면, message(0)을 보여준다.
     *    LevelEditor의 start cost가 0이면, message(1)를 보여준다.
     *    그리고, player setting의 readyFlag를 false로 만든다.
     *    
     * 2. readyFlag가 false면, player setting이 아직 준비가 되지 않았다고 설정한다.
     *    readyFlag가 true면, player setting이 준비되었다고 설정한다.
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

            LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting
                , false);
        }
        else
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(2));

            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                LevelEditorUISystem.ButtonColor.ReadyColor);

            LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting
                , true);
        }
    }

    public void Load()
    {
        if (LevelEditor.instance.GetPlayerLife() < 0)
            playerLifeInputField.text = "";
        else
            playerLifeInputField.text = LevelEditor.instance.GetPlayerLife().ToString();

        if (LevelEditor.instance.GetStartCost() < 0)
            startCostInputField.text = "";
        else
            startCostInputField.text = LevelEditor.instance.GetStartCost().ToString();

        CheckSettingComplete();
    }
}
