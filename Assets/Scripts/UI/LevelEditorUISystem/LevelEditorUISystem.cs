using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorUISystem : MonoBehaviour
{
    public static LevelEditorUISystem instance{ get; private set; }

    public enum SettingUISystemSpecific
    {
        PlayerSetting,
        SpawnerSetting,
        DestinationSetting,
        ObstacleSetting,
        EnemyWaveSetting,
        SaveSetting,
        LoadSetting
    }

    public enum ButtonColor
    {
        ReadyColor,
        NotReadyColor,
        SelectColor,
        RemoveSelectColor
    }

    private bool coroutineFlag; // message coroutine control flag
    private GameObject currentPanel; // 현재 창이 켜져있는 panel


    /*
     * color prioirity
     * removeSelectColor > selectColor > readyColor > notReadyColor
     */
    private static Color32 readyColor = new Color32(127, 255, 193, 255);
    private static Color32 notReadyColor = new Color32(255, 255, 255, 255);
    private static Color32 selectColor = new Color32(156, 255, 244, 255);
    private static Color32 removeSelectColor = new Color32(255, 0, 0, 255);


    //UI System
    private PlayerSettingUISystem playerSettingUISystem;
    private BlankCreateButtonUISystem spawnerCreateButtonUISystem;
    private BlankCreateButtonUISystem destinationCreateButtonUISystem;
    private BlankDeleteUISystem blankDeleteUISystem;
    private ObstacleSettingUISystem obstacleSettingUISystem;
    private ObstacleCreateButtonUISystem triangleCreateButtonUISystem;
    private ObstacleCreateButtonUISystem squareCreateButtonUISystem;
    private ObstacleCreateButtonUISystem pentagonCreateButtonUISystem;
    private ObstacleCreateButtonUISystem hexagonCreateButtonUISystem;
    private ObstacleDeleteUISystem obstacleDeleteUISystem;
    private EnemyWaveSettingUISystem enemyWaveSettingUISystem;
    private SaveLoadExitUISystem saveExitUISystem;
    

    [Header("Setting Button")]
    public GameObject playerSettingButton;
    public GameObject spawnerCreateButton;
    public GameObject destinationCreateButton;
    public GameObject triangleCreateButton;
    public GameObject squareCreateButton;
    public GameObject pentagonCreateButton;
    public GameObject hexagonCreateButton;
    public GameObject obstacleSettingButton;
    public GameObject enemyWaveSettingButton;
    public GameObject saveButton;
    public GameObject loadButton;
    public GameObject exitButton;

    [Header("Preserved Prefabs")]
    public GameObject warningMessagePrefab;

    private void Awake()
    {
        instance = this;

        playerSettingUISystem = this.gameObject.GetComponent<PlayerSettingUISystem>();
        spawnerCreateButtonUISystem = this.spawnerCreateButton.GetComponent<BlankCreateButtonUISystem>();
        destinationCreateButtonUISystem = this.destinationCreateButton.GetComponent<BlankCreateButtonUISystem>();
        blankDeleteUISystem = this.gameObject.GetComponent<BlankDeleteUISystem>();
        obstacleSettingUISystem = this.gameObject.GetComponent<ObstacleSettingUISystem>();
        triangleCreateButtonUISystem = this.triangleCreateButton.GetComponent<ObstacleCreateButtonUISystem>();
        squareCreateButtonUISystem = this.squareCreateButton.GetComponent<ObstacleCreateButtonUISystem>();
        pentagonCreateButtonUISystem = this.pentagonCreateButton.GetComponent<ObstacleCreateButtonUISystem>();
        hexagonCreateButtonUISystem = this.hexagonCreateButton.GetComponent<ObstacleCreateButtonUISystem>();
        obstacleDeleteUISystem = this.gameObject.GetComponent<ObstacleDeleteUISystem>();
        enemyWaveSettingUISystem = this.gameObject.GetComponent<EnemyWaveSettingUISystem>();
        saveExitUISystem = this.gameObject.GetComponent<SaveLoadExitUISystem>();

        Init();
    }

    /* void Init()
     * LevelEditorUISystem의 변수 초기화
     */
    public void Init()
    {
        coroutineFlag = false;
        currentPanel = null;

        this.playerSettingUISystem.Init();
        this.spawnerCreateButtonUISystem.Init();
        this.destinationCreateButtonUISystem.Init();
        this.obstacleSettingUISystem.Init();
        this.enemyWaveSettingUISystem.Init();
        this.saveExitUISystem.Init();
    }

    public GameObject GetCurrentPanel()
    {
        return this.currentPanel;
    }

    public PlayerSettingUISystem GetPlayerSettingUISystem()
    {
        return this.playerSettingUISystem;
    }

    public BlankCreateButtonUISystem GetSpawnerCreateSettingUISystem()
    {
        return this.spawnerCreateButtonUISystem;
    }

    public BlankCreateButtonUISystem GetDestinationCreateSettingUISystem()
    {
        return this.destinationCreateButtonUISystem;
    }

    public BlankDeleteUISystem GetBlankDeleteUISystem()
    {
        return this.blankDeleteUISystem;
    }

    public ObstacleSettingUISystem GetObstacleSettingUISystem()
    {
        return this.obstacleSettingUISystem;
    }

    public ObstacleCreateButtonUISystem GetObstacleCreateButtonUISystem(StageData.ObstacleInfo.ObstacleSpecific obstacleSpecific)
    {
        if (obstacleSpecific == StageData.ObstacleInfo.ObstacleSpecific.Triangle)
            return this.triangleCreateButtonUISystem;
        else if (obstacleSpecific == StageData.ObstacleInfo.ObstacleSpecific.Square)
            return this.squareCreateButtonUISystem;
        else if (obstacleSpecific == StageData.ObstacleInfo.ObstacleSpecific.Pentagon)
            return this.pentagonCreateButtonUISystem;
        else
            return this.hexagonCreateButtonUISystem;
    }

    public ObstacleDeleteUISystem GetObstacleDeleteUISystem()
    {
        return this.obstacleDeleteUISystem;
    }

    public EnemyWaveSettingUISystem GetEnemyWaveSettingUISystem()
    {
        return this.enemyWaveSettingUISystem;
    }

    public SaveLoadExitUISystem GetSaveExitUISystem()
    {
        return this.saveExitUISystem;
    }

    /*  void OnClickSettingButton
     *  1. 어떤 Setting button을 누르면,
     *  2-1. 아무런 패널이 켜져있지 않으면, 현재 클릭한 setting button과 관련된 패널을 켜준다.
     *  2-2. 어떤 패널이 켜져있는데, 해당 패널이 현재 클릭한 setting button과 관련된 패널이면, 패널을 끈다.
     *  2-3. 어떤 패널이 켜져있는데, 해당 패널이 현재 클릭한 setting button과 관련되지 않은 패널이면,
     *       기존에 켜져있던 패널은 끄고, 현재 클릭한 setting button과 관련된 패널을 켠다.
     */
    public void OnClickSettingButton(GameObject panel)
    {
        if (panel == this.currentPanel)
        {
            ClosePanel(panel);
            panel = null;
        }
        else
        {
            if (currentPanel != null)
                ClosePanel(currentPanel);

            if (panel.Equals(this.obstacleSettingUISystem.obstacleSettingPanel))
                this.obstacleSettingUISystem.ShowPanel();
            else
                panel.SetActive(true); 
        }

        currentPanel = panel;
    }

    /* void ClosePanel(GameObject panel)
     * 1. 해당하는 panel 끄기.
     */
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    /* IEnumerator ShowMessage(int flag)
     * 1. flag에 해당하는 message 보이기
     */
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

    /* IEnumerator ShowMessage(int flag, int min, int max)
     * 1. flag에 해당하는 message 보이기
     */
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

    /* IEnumerator ShowMessage(int flag, float min, float max)
     * 1. flag에 해당하는 message 보이기
     */
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

    /* string CreateMessage(int flag, int min, int max)
     * 1. flag에 해당하는 message 만들기
     */
    public string CreateMessage(int flag, int min, int max)
    {
        string message = "";

        if (flag == 0)
            message = string.Format("Please correct the value in player life " +
                "input field as integer between {0} and {1}!", min, max);
        else if (flag == 1)
            message = string.Format("Please correct the value in start cost " +
                "input field as integer between {0} and {1}!", min, max);
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
            message = "Please finish the spawner and destination setting!";
        else if (flag == 13)
            message = message = string.Format("Please correct the value in stage chapter " +
                "input field as integer between {0} and {1}!", min, max);
        else if (flag == 14)
            message = string.Format("Please correct the value in stage level " +
                "input field as integer between {0} and {1}!", min, max);
        else if (flag == 15)
            message = "Saved success stage chpater and stage level setting!";
        else if (flag == 16)
            message = "Please enter values in the input field of stage chapter and stage level";

        return message;
    }

    /* string CreateMessage(int flag, float min, float max)
     * 1. flag에 해당하는 message 만들기
     */
    public string CreateMessage(int flag, float min, float max)
    {
        string message = "";

        if (flag == 0)
            message = string.Format("Please correct the value in player life " +
                "input field as integer between {0} and {1}!", min, max);
        else if (flag == 1)
            message = string.Format("Please correct the value in start cost " +
                "input field as integer between {0} and {1}!", min, max);
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
            message = "Please finish the spawner and destination setting!";
        else if (flag == 13)
            message = message = string.Format("Please correct the value in stage chapter " +
                "input field as integer between {0} and {1}!", min, max);
        else if (flag == 14)
            message = string.Format("Please correct the value in stage level " +
                "input field as integer between {0} and {1}!", min, max);
        else if (flag == 15)
            message = "Saved success stage chpater and stage level setting!";
        else if (flag == 16)
            message = "Please enter values in the input field of stage chapter and stage level";

        return message;
    }

    /* void ChangeButtonColor(SettingUISystemSpecific settingUISystem, ButtonColor color)
     * 1. 해당 UI system의 setting button 색을 color로 변경한다.
     */
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
        else if (settingUISystem == SettingUISystemSpecific.SaveSetting)
            settingButton = saveButton;
        else if (settingUISystem == SettingUISystemSpecific.LoadSetting)
            settingButton = loadButton;

        if (color == ButtonColor.ReadyColor)
            settingButton.GetComponent<Image>().color = readyColor;
        else if (color == ButtonColor.NotReadyColor)
            settingButton.GetComponent<Image>().color = notReadyColor;
        else
            settingButton.GetComponent<Image>().color = selectColor;
    }

    /* void ChangeButtonColor(GameObject button, ButtonColor color)
     * 1. 특정 button의 색을 color로 변경한다.
     */
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

    /* void ChangeButtonColor(GameObject button, ButtonColor stateButtonColor, bool isSelected, bool isRemoveSelected)
     * 1. 특정 button의 색을 stateButtonColor로 변경한다.
     * 2. isRemoveSelected 가 true면, 빨간색
     * 3. isSeleted가 ture면, 하늘색
     * 4. 우선도는 removeSelectColor > selectColor > readyColor > notReadyColor
     */
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

    /* void ChangeReadyFlag(SettingUISystemSpecific settingUISystem, bool flag)
     * 1. LevelEditor의 준비상태를 flag(true or false)로 변경한다.
    */
    public void ChangeReadyFlag(SettingUISystemSpecific settingUISystem, bool flag)
    {
        if (settingUISystem == SettingUISystemSpecific.PlayerSetting)
            LevelEditor.instance.GetReadyFlag().playerSetting = flag;
        else if (settingUISystem == SettingUISystemSpecific.SpawnerSetting)
            LevelEditor.instance.GetReadyFlag().spawnerSetting = flag;
        else if (settingUISystem == SettingUISystemSpecific.DestinationSetting)
            LevelEditor.instance.GetReadyFlag().destinationSetting = flag;
        else if (settingUISystem == SettingUISystemSpecific.ObstacleSetting)
            LevelEditor.instance.GetReadyFlag().obstacleSetting = flag;
        else if (settingUISystem == SettingUISystemSpecific.EnemyWaveSetting)
            LevelEditor.instance.GetReadyFlag().enemyWaveSetting = flag;
        else if (settingUISystem == SettingUISystemSpecific.SaveSetting)
            LevelEditor.instance.GetReadyFlag().saveSetting = flag;
        else if (settingUISystem == SettingUISystemSpecific.LoadSetting)
            LevelEditor.instance.GetReadyFlag().loadSetting = flag;
    }

}

