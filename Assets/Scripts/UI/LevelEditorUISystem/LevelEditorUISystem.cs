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
        NotReadyColor,
        SelectColor,
        RemoveSelectColor
    }

    private bool coroutineFlag;
    /*
     * color prioirity
     * removeSelectColor > selectColor > readyColor = notReadyColor
     */
    private Color32 readyColor;
    private Color32 notReadyColor;
    private Color32 selectColor;
    private Color32 removeSelectColor;


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
        Init();
    }

    private void Init()
    {
        coroutineFlag = false;
        readyColor = new Color32(127, 255, 193, 255);
        notReadyColor = new Color32(255, 255, 255, 255);
        selectColor = new Color32(156, 255, 244, 255);
        removeSelectColor = new Color32(255, 0, 0, 255);

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
        else if (panel.Equals(this.enemyWaveSettingUISystem.enemyWaveSettingMainPanel))
            enemyWaveSettingUISystem.OnClickCancelButtonInEnemyWaveSettingMainPanel();
    }

    public IEnumerator ShowMessage(int flag)
    {
        string message = "";

        if (!this.coroutineFlag)
        {
            message = CreateMessage(flag, 0, 0);

            GameObject warningMessage = GameObject.Instantiate(warningMessagePrefab, this.gameObject.transform);
            warningMessage.GetComponent<TMP_Text>().text = message;

            this.coroutineFlag = true;

            yield return new WaitForSeconds(2);

            this.coroutineFlag = false;

            Destroy(warningMessage);
        }

    }

    public IEnumerator ShowMessage(int flag, int min, int max)
    {
        string message = "";

        if (!this.coroutineFlag)
        {
            message = CreateMessage(flag, min, max);

            GameObject warningMessage = GameObject.Instantiate(warningMessagePrefab, this.gameObject.transform);
            warningMessage.GetComponent<TMP_Text>().text = message;

            this.coroutineFlag = true;

            yield return new WaitForSeconds(2);

            this.coroutineFlag = false;

            Destroy(warningMessage);
        }

    }

    public IEnumerator ShowMessage(int flag, float min, float max)
    {
        string message = "";

        if (!this.coroutineFlag)
        {
            message = CreateMessage(flag, min, max);

            GameObject warningMessage = GameObject.Instantiate(warningMessagePrefab, this.gameObject.transform);
            warningMessage.GetComponent<TMP_Text>().text = message;

            this.coroutineFlag = true;

            yield return new WaitForSeconds(2);

            this.coroutineFlag = false;

            Destroy(warningMessage);
        }

    }

    public string CreateMessage(int flag, int min, int max)
    {
        string message = "";

        if (flag == 0)
            message = string.Format("Please correct the value in player life " +
                "input field as integer between {0} and {1]!", min, max);
        else if (flag == 1)
            message = string.Format("Please correct the value in start cost " +
                "input field as integer between {0} and {1]!", min, max);
        else if (flag == 2)
            message = string.Format("Saved success\nPlayer life: {0}\nStart cost: {1}"
                , LevelEditor.instance.GetPlayerLife(), LevelEditor.instance.GetStartCost());
        else if (flag == 3)
            message = string.Format("Please correct the value in enemies count " +
                "input field as integer between {0} and {1}", min, max);
        else if (flag == 4)
            message = "Saved success enemies setting!";
        else if (flag == 5)
            message = string.Format("Please correct the value in enemy spawn duration " +
                "input field as the number between {0} and {1}!", min, max);
        else if (flag == 6)
            message = string.Format("Please correct the value in break time " +
                "input field as the number between {0} and {1}!", min, max);
        else if (flag == 7)
            message = "Please add at least one enemies and fill in input field!";
        else if (flag == 8)
            message = "Please finish all enemies setting!";
        else if (flag == 9)
            message = "Saved succes one enemy wave setting";
        else if (flag == 10)
            message = "Please add at least one wave and finish one wave setting";
        else if (flag == 11)
            message = "Please finish all waves setting!";
        else if (flag == 12)
            message = " Saved success enemy waves setting.";

        return message;
    }

    public string CreateMessage(int flag, float min, float max)
    {
        string message = "";

        if (flag == 0)
            message = "Please correct the values in player life input field and the start cost of input field to normal integers";
        else if (flag == 1)
            message = string.Format("Please correct the value in player life " +
                "input field as integer between {0} and {1]!", min, max);
        else if (flag == 2)
            message = string.Format("Please correct the value in start cost " +
                "input field as integer between {0} and {1]!", min, max);
        else if (flag == 3)
            message = string.Format("Saved success\nPlayer life: {0}\nStart cost: {1}"
                , LevelEditor.instance.GetPlayerLife(), LevelEditor.instance.GetStartCost());
        else if (flag == 4)
            message = string.Format("Please correct the value in enemies count " +
                "input field as integer between {0} and {1}", min, max);
        else if (flag == 5)
            message = "Please add at least one  enemies and fill in input field!";
        else if (flag == 6)
            message = string.Format("Please correct the value in enemy spawn duration " +
                "input field as the number between {0} and {1}!", min, max);
        else if (flag == 7)
            message = string.Format("Please correct the value in break time " +
                "input field as the number between {0} and {1}!", min, max);
        else if (flag == 8)
            message = "Please finish setting all the waves!";
        else if (flag == 9)
            message = "Please add at least one wave!";
        else if (flag == 10)
            message = "Saved success enemy waves setting!";

        return message;
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
        else if (color == ButtonColor.NotReadyColor)
            settingButton.GetComponent<Image>().color = notReadyColor;
        else
            settingButton.GetComponent<Image>().color = selectColor;
    }
    
    public void ChangeButtonColor(GameObject button, ButtonColor color)
    {
        if (color == ButtonColor.ReadyColor)
            button.GetComponent<Image>().color = readyColor;
        else if (color == ButtonColor.NotReadyColor)
            button.GetComponent<Image>().color = notReadyColor;
        else if (color == ButtonColor.SelectColor)
            button.GetComponent<Image>().color = selectColor;
        else if (color == ButtonColor.RemoveSelectColor)
            button.GetComponent<Image>().color = removeSelectColor;
    }

    public void ChangeButtonColor(GameObject button, ButtonColor stateButtonColor, bool isSelected, bool isRemoveSelected)
    {
        if (isRemoveSelected == true)
            button.GetComponent<Image>().color = removeSelectColor;
        else if (isSelected == true)
            button.GetComponent<Image>().color = selectColor;
        else if (stateButtonColor == ButtonColor.ReadyColor)
            button.GetComponent<Image>().color = readyColor;
        else if (stateButtonColor == ButtonColor.NotReadyColor)
            button.GetComponent<Image>().color = notReadyColor;
    }
}

