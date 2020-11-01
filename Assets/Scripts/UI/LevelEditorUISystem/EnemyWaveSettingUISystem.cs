using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.ShaderGraph.Internal;

public class EnemyWaveSettingUISystem : MonoBehaviour
{
    public class EnemyWaveUI
    {
        public GameObject enemyWaveButton;
        public LevelEditorUISystem.ButtonColor stateButtonColor;
        public bool isSelected;
        public bool isRemoveSelected;

        public GameObject enemyWaveOptionalSettingPanel;
        public GameObject enemyOneWavePanel;
        public GameObject enemiesRemovePanel;
        public bool removePanelActive;

        public List<EnemiesUI> enemiesUIList;
    }

    public class EnemiesUI
    {
        public GameObject enemiesPanel;
        public LevelEditorUISystem.ButtonColor stateButtonColor;
        public bool isSelected;
        public bool isRemoveSelected;
    }

    private List<EnemyWaveUI> enemyWaveUIList;
    private List<EnemyWaveUI> selectedWaveUIList;
    private List<StageData.EnemyWaveInfo> selectedWaveInfoList;
    private List<EnemiesUI> selectedEnemiesUIList;
    private List<StageData.Enemies> selectedEnemiesList;

    private static float enemySpawnDurationMin = 0.5f;
    private static float enemySpawnDurationMax = 5f;
    private static float breakTimeMin = 5f;
    private static float breakTimeMax = 30f;
    private static int enemiesCountMin = 1;
    private static int enemiesCountMax = 100;
    private static string waveStatTextFormat = "Max Wave : {0}";
    private static string waveTextForamt = "Wave{0}";
    private static string allEnemiesTextFormat = "Enemies  List : {0} ";
    private static string enemiesTextFormat = "Enemies{0}";

    [Header("Related To Enemy Wave Setting Main Panel")]
    public GameObject enemyWaveSettingMainPanel;

    [Header("Related To Enemy Wave List Panel")]
    public TMP_Text waveStatText;
    public GameObject waveButtons; // wave buttons
    public GameObject waveRemovePanel;

    
    [Header("Enemy Wave Setting")]
    public GameObject enemyWaveSettingPanel;

    [Header("Preserved Prefabs")]
    public GameObject commonButtonPrefab;
    public GameObject enemyWaveOptionalSettingPanelPrefab;
    public GameObject enemyOneWavePanelPrefab;
    public GameObject enemiesRemovePanelPrefab;
    public GameObject enemiesPanelPrefab;


    private void Awake()
    {
        enemyWaveUIList = new List<EnemyWaveUI>();
        selectedWaveUIList = new List<EnemyWaveUI>();
        selectedWaveInfoList = new List<StageData.EnemyWaveInfo>();
        selectedEnemiesUIList = new List<EnemiesUI>();
        selectedEnemiesList = new List<StageData.Enemies>();
    }

    public void OnClickCancelButtonInEnemyWaveSettingMainPanel()
    {
        enemyWaveSettingMainPanel.SetActive(false);
    }

    public void OnClickAddWaveButtonInListPanel()
    {
        EnemyWaveUI enemyWaveUI = new EnemyWaveUI();
        StageData.EnemyWaveInfo enemyWaveInfo = new StageData.EnemyWaveInfo();

        TMP_InputField spawnerDurationInputField = null;
        TMP_Dropdown nextPhaseTrigger = null;
        TMP_InputField endInBreakTimeInputField = null;

        int index = this.enemyWaveUIList.Count;
        int waveNum = index + 1;

        //wave stat text
        waveStatText.text = string.Format(waveStatTextFormat, waveNum);

        //creat waveButton
        enemyWaveUI
            .enemyWaveButton
            = GameObject.Instantiate(commonButtonPrefab, this.waveButtons.transform);
        enemyWaveUI
            .enemyWaveButton
            .transform
            .GetChild(0)
            .GetComponent<TMP_Text>()
            .text
            = string.Format(waveTextForamt, waveNum);

        enemyWaveUI
            .enemyWaveButton
            .transform
            .GetComponent<Button>()
            .onClick
            .AddListener(() => OnClickWaveButton(enemyWaveUI));

        enemyWaveUI
            .stateButtonColor
            = LevelEditorUISystem.ButtonColor.NotReadyColor;

        enemyWaveUI
            .isSelected
            = false;

        enemyWaveUI
            .isRemoveSelected
            = false;

        //create enemy wave UI
        enemyWaveUI.enemyWaveOptionalSettingPanel
            = GameObject
            .Instantiate(enemyWaveOptionalSettingPanelPrefab, this.enemyWaveSettingPanel.transform);

        spawnerDurationInputField
            = enemyWaveUI
            .enemyWaveOptionalSettingPanel
            .transform
            .GetChild(2)
            .GetComponent<TMP_InputField>();
        spawnerDurationInputField
            .onEndEdit
            .AddListener((_) => OnEndInEnemySpawnerDurationInputField(enemyWaveUI));

        nextPhaseTrigger
            = enemyWaveUI
            .enemyWaveOptionalSettingPanel
            .transform
            .GetChild(4)
            .GetComponent<TMP_Dropdown>();
        nextPhaseTrigger
            .onValueChanged
            .AddListener((_) => OnValueChangedNextPhaseTrigger(enemyWaveUI));

        endInBreakTimeInputField
            = enemyWaveUI
            .enemyWaveOptionalSettingPanel
            .transform
            .GetChild(6)
            .GetComponent<TMP_InputField>();
        endInBreakTimeInputField
            .onEndEdit
            .AddListener((_) => OnEndInBreakTimeInputField(enemyWaveUI));
        
        enemyWaveUI.enemyOneWavePanel = GameObject.Instantiate(enemyOneWavePanelPrefab, this.enemyWaveSettingPanel.transform);
        enemyWaveUI.enemyOneWavePanel.GetComponent<RectTransform>().anchoredPosition
            = new Vector2(enemyWaveUI.enemyWaveOptionalSettingPanel.GetComponent<RectTransform>().rect.width, 0);

        enemyWaveUI.enemiesRemovePanel = GameObject.Instantiate(enemiesRemovePanelPrefab, this.enemyWaveSettingPanel.transform);
        enemyWaveUI
            .enemiesRemovePanel
            .transform
            .GetChild(1)
            .GetComponent<Button>()
            .onClick
            .AddListener(() => OnClickRemoveButtonInEnemiesRemovePanel(enemyWaveUI));
        enemyWaveUI
            .enemiesRemovePanel
            .transform
            .GetChild(2)
            .GetComponent<Button>()
            .onClick
            .AddListener(() => OnClickCancleButtonInEnemiesRemovePanel(enemyWaveUI));
        enemyWaveUI
            .enemiesRemovePanel
            .transform
            .GetChild(3)
            .GetComponent<Button>()
            .onClick
            .AddListener(() => OnClickCancleButtonInEnemiesRemovePanel(enemyWaveUI));
        enemyWaveUI.removePanelActive = false;

        enemyWaveUI
            .enemyOneWavePanel
            .transform
            .GetChild(2)
            .GetChild(0)
            .GetComponent<Button>()
            .onClick
            .AddListener(() => OnClickAddButtonInOneWavePanel(enemyWaveUI));

        enemyWaveUI
            .enemyOneWavePanel
            .transform
            .GetChild(2)
            .GetChild(1)
            .GetComponent<Button>()
            .onClick
            .AddListener(() => OnClickRemoveButtonInOneWavePanel(enemyWaveUI));


        enemyWaveUI.enemiesUIList = new List<EnemiesUI>();
        this.enemyWaveUIList.Add(enemyWaveUI);


        LevelEditor.instance.GetEnemyWaveInfoList().Add(enemyWaveInfo);
        enemyWaveInfo.enemyOneWave = new List<StageData.Enemies>();
        enemyWaveInfo.breakTime = 0f;
        enemyWaveInfo.nextPhaseTrigger = StageData.EnemyWaveInfo.NextPhaseTrigger.EnemyExterminated;
        enemyWaveInfo.enemySpawnDuration = 0f;
   
    }

    public void OnClickRemoveButtonInListPanel()
    {
        if(waveRemovePanel.activeSelf == false)
            waveRemovePanel.SetActive(true);
        else
            OnClickCancleButtonInWaveRemovePanel();
    }

    private void OnClickWaveButton(EnemyWaveUI enemyWaveUI)
    {
        int index = enemyWaveUIList.IndexOf(enemyWaveUI);
        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[index];
        EnemyWaveUI temp = null;

        if (waveRemovePanel.activeSelf == true)
        {
            if (enemyWaveUI.isRemoveSelected == false)
            {
                enemyWaveUI.isRemoveSelected = true;
                selectedWaveUIList.Add(enemyWaveUI);

                selectedWaveInfoList.Add(enemyWaveInfo);
            }
            else
            {
                enemyWaveUI.isRemoveSelected = false;
                selectedWaveUIList.Remove(enemyWaveUI);

                selectedWaveInfoList.Remove(enemyWaveInfo);
            }

            LevelEditorUISystem
                .instance
                .ChangeButtonColor(enemyWaveUI.enemyWaveButton
                , enemyWaveUI.stateButtonColor
                , enemyWaveUI.isSelected
                , enemyWaveUI.isRemoveSelected);
        }
        else
        {
            /*
             * flag = 0: 창이 다 꺼져있으면
             * flag = 1: 창이 켜져있는데 동일한 창을 켜려고하면
             * flag = 2: 창이 켜져있는데 다른 창이 켜면
             */
            int flag = 0;
            
            foreach (EnemyWaveUI enmyWaveUITemp in this.enemyWaveUIList)
            {
                if (enmyWaveUITemp.enemyWaveOptionalSettingPanel.activeSelf == true
                    && enmyWaveUITemp.enemyOneWavePanel.activeSelf == true)
                {
                    if (enmyWaveUITemp == enemyWaveUI)
                        flag = 1;
                    else
                        flag = 2;

                    temp = enmyWaveUITemp;

                    break;
                }
            }

            if (flag == 0)
            {
                this.enemyWaveSettingPanel.SetActive(true);

                enemyWaveUI.isSelected = true;
                enemyWaveUI.enemyWaveOptionalSettingPanel.SetActive(true);
                enemyWaveUI.enemyOneWavePanel.SetActive(true);

                if (enemyWaveUI.removePanelActive == true)
                    enemyWaveUI.enemiesRemovePanel.SetActive(true);
            }
            else if (flag == 1)
            {
                this.enemyWaveSettingPanel.SetActive(false);

                enemyWaveUI.isSelected = false;
                OnClickCancelWaveButton(enemyWaveUI);

                if (enemyWaveUI.removePanelActive == true)
                    enemyWaveUI.enemiesRemovePanel.SetActive(false);
            }
            else
            {
                enemyWaveUI.enemyWaveOptionalSettingPanel.SetActive(true);

                temp.isSelected = false;
                OnClickCancelWaveButton(temp);

                if (temp.removePanelActive == true)
                    temp.enemiesRemovePanel.SetActive(false);

                LevelEditorUISystem
                .instance
                .ChangeButtonColor(temp.enemyWaveButton
                , temp.stateButtonColor
                , temp.isSelected
                , temp.isRemoveSelected);

                enemyWaveUI.isSelected = true;
                enemyWaveUI.enemyOneWavePanel.SetActive(true);

                if (enemyWaveUI.removePanelActive == true)
                    enemyWaveUI.enemiesRemovePanel.SetActive(true);
            }

            LevelEditorUISystem
                .instance
                .ChangeButtonColor(enemyWaveUI.enemyWaveButton
                , enemyWaveUI.stateButtonColor
                , enemyWaveUI.isSelected
                , enemyWaveUI.isRemoveSelected);
        }
    }

    private void OnClickCancelWaveButton(EnemyWaveUI enemyWaveUI)
    {
        enemyWaveUI.enemyWaveOptionalSettingPanel.SetActive(false);
        enemyWaveUI.enemyOneWavePanel.SetActive(false);
    }

    public void OnClickRemoveButtonInWaveRemovePanel()
    {
        //Remove enemy wave UI
        foreach (EnemyWaveUI enemyWaveUI in selectedWaveUIList)
        {
            Destroy(enemyWaveUI.enemyWaveButton);
            Destroy(enemyWaveUI.enemyWaveOptionalSettingPanel);
            Destroy(enemyWaveUI.enemyOneWavePanel);
            enemyWaveUI.enemiesUIList = null;

            this.enemyWaveUIList.Remove(enemyWaveUI);
        }

        foreach (StageData.EnemyWaveInfo enemyWaveInfo in selectedWaveInfoList)
            LevelEditor.instance.GetEnemyWaveInfoList().Remove(enemyWaveInfo);


        //Processing after deletion
        if (selectedWaveUIList.Count > 0)
        {
            waveStatText.text = string.Format(waveStatTextFormat, enemyWaveUIList.Count);
            for (int i = 0; i < enemyWaveUIList.Count; i++)
            {
                enemyWaveUIList[i]
                    .enemyWaveButton
                    .transform
                    .GetChild(0)
                    .GetComponent<TMP_Text>()
                    .text
                    = string.Format(waveTextForamt, i+1);
            }

            selectedWaveUIList.Clear();
            selectedWaveInfoList.Clear();
        }

        IsCompleteWavesSetting();

        waveRemovePanel.SetActive(false);
    }

    public void OnClickCancleButtonInWaveRemovePanel()
    {
        //change color to current state color
        foreach (EnemyWaveUI enemyWaveUI in selectedWaveUIList)
        {
            LevelEditorUISystem.instance.ChangeButtonColor(enemyWaveUI.enemyWaveButton
                ,enemyWaveUI.stateButtonColor);
        }

        selectedWaveUIList.Clear();
        selectedWaveInfoList.Clear();

        waveRemovePanel.SetActive(false);
    }

    public void OnEndInEnemySpawnerDurationInputField(EnemyWaveUI enemyWaveUI)
    {
        int index = enemyWaveUIList.IndexOf(enemyWaveUI);
        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[index];

        TMP_InputField inputField
            = enemyWaveUI
            .enemyWaveOptionalSettingPanel
            .transform
            .GetChild(2)
            .GetComponent<TMP_InputField>();

        float num = 0f;
        if (!float.TryParse(inputField.text, out num))
            enemyWaveInfo.enemySpawnDuration = 0f;
        else
        {
            if (!((enemySpawnDurationMin <= num) && (num <= enemySpawnDurationMax)))
                enemyWaveInfo.enemySpawnDuration = 0f;
            else
                enemyWaveInfo.enemySpawnDuration = num;
        }

        if (enemyWaveInfo.enemySpawnDuration == 0f)
            inputField.text = "";

        IsCompleteOneWaveSetting(enemyWaveUI);
        
    }

    public void OnValueChangedNextPhaseTrigger(EnemyWaveUI enemyWaveUI)
    {
        int index = enemyWaveUIList.IndexOf(enemyWaveUI);
        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[index];


        TMP_Dropdown dropdown
            = enemyWaveUI
            .enemyWaveOptionalSettingPanel
            .transform
            .GetChild(4)
            .GetComponent<TMP_Dropdown>();

        enemyWaveInfo.nextPhaseTrigger = (StageData.EnemyWaveInfo.NextPhaseTrigger)dropdown.value;

        IsCompleteOneWaveSetting(enemyWaveUI);
        
    }

    public void OnEndInBreakTimeInputField(EnemyWaveUI enemyWaveUI)
    {
        int index = enemyWaveUIList.IndexOf(enemyWaveUI);
        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[index];


        TMP_InputField inputField
            = enemyWaveUI
            .enemyWaveOptionalSettingPanel
            .transform
            .GetChild(6)
            .GetComponent<TMP_InputField>();

        float num = 0f;
        if (!float.TryParse(inputField.text, out num))
            enemyWaveInfo.breakTime = 0f;
        else
        {
            if (!((breakTimeMin <= num) && (num <= breakTimeMax)))
                enemyWaveInfo.breakTime = 0f;
            else
                enemyWaveInfo.breakTime = num;
        }

        if (enemyWaveInfo.breakTime == 0f)
            inputField.text = "";

        IsCompleteOneWaveSetting(enemyWaveUI);
        
    }

    public void OnClickAddButtonInOneWavePanel(EnemyWaveUI enemyWaveUI)
    {
        int wIndex = enemyWaveUIList.IndexOf(enemyWaveUI);
        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[wIndex];

        EnemiesUI enemiesUI = new EnemiesUI();
        Button enemiesButton;
        TMP_Dropdown enemiesDropdown;
        TMP_InputField enemiesCountInputField;
        int eIndex = enemyWaveUI.enemiesUIList.Count;
        int enemiesNum = eIndex + 1;

        StageData.Enemies enemies = new StageData.Enemies();
        
        enemyWaveUI
            .enemyOneWavePanel
            .transform
            .GetChild(0)
            .GetComponent<TMP_Text>()
            .text = string.Format(allEnemiesTextFormat, enemiesNum);

        enemiesUI.enemiesPanel = GameObject.Instantiate(this.enemiesPanelPrefab
            , enemyWaveUI
            .enemyOneWavePanel
            .transform
            .GetChild(1)
            .GetChild(0)
            .GetChild(0)); // viewport/Enemies

        enemiesButton = enemiesUI
                            .enemiesPanel
                            .transform
                            .GetChild(0) //Enemies Button
                            .GetComponent<Button>();

        enemiesButton
            .gameObject
            .transform
            .GetChild(0) //Text of Enemies Button
            .GetComponent<TMP_Text>()
            .text
            = string.Format(enemiesTextFormat, enemiesNum);

        enemiesButton
            .onClick
            .AddListener(() => OnClickEnemiesButton(enemyWaveUI, enemiesUI));

        enemiesDropdown = enemiesUI
                                .enemiesPanel
                                .transform
                                .GetChild(1) // dropDown
                                .GetComponent<TMP_Dropdown>();

        enemiesDropdown
            .onValueChanged
            .AddListener((_) => OnValueChangedEnemySpecific(enemyWaveUI, enemiesUI));

        enemiesCountInputField = enemiesUI
                                    .enemiesPanel
                                    .transform
                                    .GetChild(2) // inputField
                                    .GetComponent<TMP_InputField>();
        enemiesCountInputField
            .onEndEdit
            .AddListener((_) => OnEndInEnemiesCountInputField(enemyWaveUI, enemiesUI));


        enemiesUI.stateButtonColor = LevelEditorUISystem.ButtonColor.NotReadyColor;
        enemiesUI.isSelected = false;
        enemiesUI.isRemoveSelected = false;
        enemyWaveUI.enemiesUIList.Add(enemiesUI);


        enemyWaveInfo.enemyOneWave.Add(enemies);
    }

    public void OnClickRemoveButtonInOneWavePanel(EnemyWaveUI enemyWaveUI)
    {
        if (enemyWaveUI.enemiesRemovePanel.activeSelf == false)
        {
            enemyWaveUI.enemiesRemovePanel.SetActive(true);
            enemyWaveUI.removePanelActive = true;
        }
        else
            OnClickCancleButtonInEnemiesRemovePanel(enemyWaveUI);
    }

    public void OnClickRemoveButtonInEnemiesRemovePanel(EnemyWaveUI enemyWaveUI)
    {
        int index = enemyWaveUIList.IndexOf(enemyWaveUI);
        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[index];
        TMP_Text enemiesStatText;

        //Remove enemy wave UI
        foreach (EnemiesUI enemiesUI in selectedEnemiesUIList)
        {
            Destroy(enemiesUI.enemiesPanel);
            enemyWaveUI.enemiesUIList.Remove(enemiesUI);
        }

        foreach (StageData.Enemies enemies in selectedEnemiesList)
            enemyWaveInfo.enemyOneWave.Remove(enemies);


        //Processing after deletion
        if (selectedEnemiesUIList.Count > 0)
        {
            enemiesStatText = enemyWaveUI.enemyOneWavePanel.transform.GetChild(0).GetComponent<TMP_Text>();
            enemiesStatText.text = string.Format(allEnemiesTextFormat, enemyWaveUI.enemiesUIList.Count); 

            for(int i = 0; i < enemyWaveUI.enemiesUIList.Count; i++)
            {
                enemyWaveUI
                    .enemiesUIList[i]
                    .enemiesPanel
                    .transform
                    .GetChild(0)
                    .GetChild(0)
                    .GetComponent<TMP_Text>()
                    .text = string.Format(enemiesTextFormat, i+1);
            }

            selectedEnemiesUIList.Clear();
            selectedEnemiesList.Clear();
        }

        IsCompleteOneWaveSetting(enemyWaveUI);


        enemyWaveUI.enemiesRemovePanel.SetActive(false);
        enemyWaveUI.removePanelActive = false;
    }

    public void OnClickCancleButtonInEnemiesRemovePanel(EnemyWaveUI enemyWaveUI)
    {
        GameObject EnemiesButton = null;


        //change color to current state color
        foreach (EnemiesUI enemiesUI in selectedEnemiesUIList)
        {
            EnemiesButton = enemiesUI.enemiesPanel.transform.GetChild(0).gameObject;
            LevelEditorUISystem.instance.ChangeButtonColor(EnemiesButton
                , enemyWaveUI.stateButtonColor);
        }

        selectedWaveUIList.Clear();
        selectedWaveInfoList.Clear();

        enemyWaveUI.enemiesRemovePanel.SetActive(false);
        enemyWaveUI.removePanelActive = false;
    }

    public void OnClickEnemiesButton(EnemyWaveUI enemyWaveUI, EnemiesUI enemiesUI)
    {
        int wIndex = enemyWaveUIList.IndexOf(enemyWaveUI);
        int eIndex = enemyWaveUI.enemiesUIList.IndexOf(enemiesUI);

        GameObject enemiesButton = null;

        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[wIndex];
        StageData.Enemies enemies = enemyWaveInfo.enemyOneWave[eIndex];



        if (enemyWaveUI.enemiesRemovePanel.activeSelf == true)
        {
            enemiesButton = enemiesUI.enemiesPanel.transform.GetChild(0).gameObject;

            if (enemiesUI.isRemoveSelected == false)
            {
                enemiesUI.isRemoveSelected = true;
                selectedEnemiesUIList.Add(enemiesUI);

                selectedEnemiesList.Add(enemies);
            }
            else
            {
                enemiesUI.isRemoveSelected = false;
                selectedEnemiesUIList.Remove(enemiesUI);

                selectedEnemiesList.Remove(enemies);
            }

            LevelEditorUISystem
                .instance
                .ChangeButtonColor(enemiesButton
                , enemiesUI.stateButtonColor
                , enemiesUI.isSelected
                , enemiesUI.isRemoveSelected);
        }
    }

    public void OnValueChangedEnemySpecific(EnemyWaveUI enemyWaveUI, EnemiesUI enemiesUI)
    {
        int wIndex = enemyWaveUIList.IndexOf(enemyWaveUI);
        int eIndex = enemyWaveUI.enemiesUIList.IndexOf(enemiesUI);

        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[wIndex];
        StageData.Enemies enemies = enemyWaveInfo.enemyOneWave[eIndex];


        TMP_Dropdown dropdown
            = enemiesUI
            .enemiesPanel
            .transform
            .GetChild(1)
            .GetComponent<TMP_Dropdown>();

        enemies.enemySpecific = (StageData.Enemies.EnemySpecific)dropdown.value;

        IsCompleteEnemiesSetting(enemyWaveUI, enemiesUI);
    }

    public void OnEndInEnemiesCountInputField(EnemyWaveUI enemyWaveUI, EnemiesUI enemiesUI)
    {
        int wIndex = enemyWaveUIList.IndexOf(enemyWaveUI);
        int eIndex = enemyWaveUI.enemiesUIList.IndexOf(enemiesUI);

        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[wIndex];
        StageData.Enemies enemies = enemyWaveInfo.enemyOneWave[eIndex];


        TMP_InputField inputField
            = enemiesUI
            .enemiesPanel
            .transform
            .GetChild(2)
            .GetComponent<TMP_InputField>();

        int num = 0;
        if (!int.TryParse(inputField.text, out num))
            enemies.count = 0;
        else
        {
            if (!((enemiesCountMin <= num) && (num <= enemiesCountMax)))
                enemies.count = 0;
            else
                enemies.count = num;
        }

        if (enemies.count == 0)
            inputField.text = "";

        IsCompleteEnemiesSetting(enemyWaveUI, enemiesUI);
    }


    public void IsCompleteEnemiesSetting(EnemyWaveUI enemyWaveUI, EnemiesUI enemiesUI)
    {
        int wIndex = enemyWaveUIList.IndexOf(enemyWaveUI);
        int eIndex = enemyWaveUI.enemiesUIList.IndexOf(enemiesUI);

        GameObject enemiesButton = enemiesUI.enemiesPanel.transform.GetChild(0).gameObject;

        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[wIndex];
        StageData.Enemies enemies = enemyWaveInfo.enemyOneWave[eIndex];

        bool readyFlag = true;

        if (enemiesCountMin > enemies.count
            || enemies.count > enemiesCountMax)
        {
            readyFlag = false;
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(3, enemiesCountMin, enemiesCountMax));
        }

        if (readyFlag == false)
            enemiesUI.stateButtonColor = LevelEditorUISystem.ButtonColor.NotReadyColor;

        else
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(4));

            enemiesUI.stateButtonColor = LevelEditorUISystem.ButtonColor.ReadyColor;
        }

        LevelEditorUISystem
                .instance
                .ChangeButtonColor(enemiesButton
                , enemiesUI.stateButtonColor
                , enemiesUI.isSelected
                , enemiesUI.isRemoveSelected);

        IsCompleteOneWaveSetting(enemyWaveUI);
    }

    public void IsCompleteOneWaveSetting(EnemyWaveUI enemyWaveUI)
    {
        int index = enemyWaveUIList.IndexOf(enemyWaveUI);
        StageData.EnemyWaveInfo enemyWaveInfo = LevelEditor.instance.GetEnemyWaveInfoList()[index];

        bool readyFlag = true;

        if (enemySpawnDurationMin > enemyWaveInfo.enemySpawnDuration
            || enemyWaveInfo.enemySpawnDuration > enemySpawnDurationMax)
        {
            readyFlag = false;
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(5, enemySpawnDurationMin, enemySpawnDurationMax));
        }

        else if (breakTimeMin > enemyWaveInfo.breakTime
            || enemyWaveInfo.breakTime > breakTimeMax)
        {
            readyFlag = false;
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(6, breakTimeMin, breakTimeMax));
        }
        else if (enemyWaveInfo.enemyOneWave.Count <= 0)
        {
            readyFlag = false;
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(7));
        }
        else
        {
            foreach (EnemiesUI enemiesUI in enemyWaveUI.enemiesUIList)
            {
                if (enemiesUI.stateButtonColor == LevelEditorUISystem.ButtonColor.NotReadyColor)
                {
                    readyFlag = false;
                    StartCoroutine(LevelEditorUISystem.instance.ShowMessage(8));

                    break;
                }
            }
        }

        if (readyFlag == false)
            enemyWaveUI.stateButtonColor = LevelEditorUISystem.ButtonColor.NotReadyColor;
        else
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(9));

            enemyWaveUI.stateButtonColor = LevelEditorUISystem.ButtonColor.ReadyColor;
        }

        LevelEditorUISystem
                .instance
                .ChangeButtonColor(enemyWaveUI.enemyWaveButton
                , enemyWaveUI.stateButtonColor
                , enemyWaveUI.isSelected
                , enemyWaveUI.isRemoveSelected);

        IsCompleteWavesSetting();
    }

    public void IsCompleteWavesSetting()
    {
        bool readyFlag = true;

        if (enemyWaveUIList.Count <= 0)
        {
            readyFlag = false;
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(10));
        }
        else
        {
            foreach (EnemyWaveUI enemyWaveUI in enemyWaveUIList)
            {
                if (enemyWaveUI.stateButtonColor == LevelEditorUISystem.ButtonColor.NotReadyColor)
                {
                    readyFlag = false;
                    StartCoroutine(LevelEditorUISystem.instance.ShowMessage(11));

                    break;
                }
            }
        }
        
        if (readyFlag == false)
        {
            LevelEditorUISystem
                .instance
                .ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.EnemyWaveSetting
                , LevelEditorUISystem.ButtonColor.NotReadyColor);
        }
        else
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(12));

            LevelEditorUISystem
                .instance
                .ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.EnemyWaveSetting
                , LevelEditorUISystem.ButtonColor.ReadyColor);
        }
    }
}
