using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorUISystem : MonoBehaviour
{
    public static LevelEditorUISystem instance { get; private set; }

    public enum SettingUISystemSpecific
    {
        PlayerSetting,
        SpawnerSetting,
        DestinationSetting,
        ObstacleSetting,
        EnemyWaveSetting
    }

    public enum ButtonColor
    {
        ReadyColor,
        NotReadyColor
    }

    private bool coroutineFlag;
    private Color32 readyColor;
    private Color32 notReadyColor;


    //UI System
    private PlayerSettingUISystem playerSettingUISystem;
    private BlankSettingUISystem blankSettingUISystem;
    //private ObstacleSettingUISystem obstacleSettingUISystem;
    private EnemyWaveSettingUISystem enemyWaveSettingUISystem;

    [Header("Setting Button")]
    public GameObject playerSettingButton;
    public GameObject spawnerCreateButton;
    public GameObject destinationCreateButton;
    public GameObject obstacleSettingButton;
    public GameObject enemyWaveSettingButton;

    [Header("Save & Exit")]
    public GameObject savePanel;
    public GameObject exitPanel;

    [Header("Preserved Prefabs")]
    public GameObject warningMessagePrefab;


    private void Awake()
    {
        instance = this;

        coroutineFlag = false;
        readyColor = new Color32(127, 255, 193, 255);
        notReadyColor = new Color32(255, 255, 255, 255);

        playerSettingUISystem = this.gameObject.transform.GetComponent<PlayerSettingUISystem>();
        blankSettingUISystem = this.gameObject.transform.GetComponent<BlankSettingUISystem>();
        //
        enemyWaveSettingUISystem = this.gameObject.transform.GetComponent<EnemyWaveSettingUISystem>();
    }

    public PlayerSettingUISystem GetPlayerSettingUISystem()
    {
        return this.playerSettingUISystem;
    }

    public BlankSettingUISystem GetBlankSettingUISystem()
    {
        return this.blankSettingUISystem;
    }

    /*  
     *  Common Button
     */
    public void OnClickSettingButton(GameObject panel)
    {
        if (panel.activeSelf == true)
            ClosePanel(panel);
        else
            panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        if (panel.Equals(this.playerSettingUISystem.playerSettingPanel))
            playerSettingUISystem.OnClickCancelButton();
        /*
        else if (panel.Equals(this.enemyWaveSettingMainPanel))
            OnClickCancelButtonInEnemyWaveSettingMainPanel();
        */
    }

    public IEnumerator ShowMessage(int flag)
    {
        string message = "";

        if (!this.coroutineFlag)
        {
            if (flag == 0)
                message = "Please correct the values in player life input field and the start cost of input field to normal integers";
            else if (flag == 1)
                message = "Please correct the value in player life input field as integer between 1 and 20!";
            else if (flag == 2)
                message = "Please correct the value in start cost input field as integer between 50 and 2000!";
            else
                message = string.Format("Saved success\nPlayer life: {0}\nStart cost: {1}"
                    ,LevelEditor.instance.GetPlayerLife(), LevelEditor.instance.GetStartCost());

            GameObject warningMessage = GameObject.Instantiate(warningMessagePrefab, this.gameObject.transform);
            warningMessage.GetComponent<TMP_Text>().text = message;

            this.coroutineFlag = true;

            yield return new WaitForSeconds(2);

            this.coroutineFlag = false;

            Destroy(warningMessage);
        }

    }

    public void ShowWarningPanel(GameObject relatedPanel)
    {
        Debug.Log("Show Warning Panel");
    }

    public void ChangeButtonColor(SettingUISystemSpecific settingUISystem, ButtonColor color)
    {
        GameObject settingButton = null;

        if (settingUISystem == SettingUISystemSpecific.PlayerSetting)
            settingButton = playerSettingButton;
        else if (settingUISystem == SettingUISystemSpecific.SpawnerSetting)
            settingButton = spawnerCreateButton;
        else if (settingUISystem == SettingUISystemSpecific.DestinationSetting)
            settingButton = destinationCreateButton;
        else if (settingUISystem == SettingUISystemSpecific.ObstacleSetting)
            settingButton = obstacleSettingButton;
        else if (settingUISystem == SettingUISystemSpecific.EnemyWaveSetting)
            settingButton = enemyWaveSettingButton;

        if (color == ButtonColor.ReadyColor)
            settingButton.GetComponent<Image>().color = readyColor;
        else
            settingButton.GetComponent<Image>().color = notReadyColor;
    }


    /*  
     *  Related enemy wave setting
     */

    
}

