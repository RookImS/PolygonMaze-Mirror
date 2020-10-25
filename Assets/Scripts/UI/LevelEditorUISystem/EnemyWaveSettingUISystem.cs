using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyWaveSettingUISystem : MonoBehaviour
{
    public class EnemyWaveUI
    {
        public GameObject enemyWaveButton;
        public LevelEditorUISystem.ButtonColor currentButtonColor;
        public GameObject enemyWaveOptionalSettingPanel;
        public GameObject enemyOneWavePanel;
        public StageData.EnemyWaveInfo enemyWaveInfo;
    }

    public List<EnemyWaveUI> enemyWaveUIList;
    private List<EnemyWaveUI> selectWaveUIList;
    private string waveStatTextFormat;
    private string waveTextForamt;

    [Header("Related To Enemy Wave Setting Main Panel")]
    public GameObject enemyWaveSettingMainPanel;

    [Header("Related To Enemy Wave List Panel")]
    public TextMeshProUGUI waveStatText;
    public GameObject wavesScrollView;
    public GameObject otherButtons; // add button & remove button
    public GameObject waveButtons; // wave buttons
    public GameObject removePanel;
    
    [Header("Enemy Wave Setting")]
    public GameObject enemyWaveSettingPanel;

    [Header("Preserved Prefabs")]
    public GameObject commonButtonPrefab;
    public GameObject enemyWaveOptionalSettingPanelPrefab;
    public GameObject enemyOneWavePanelPrefab;


    private void Awake()
    {
        enemyWaveUIList = new List<EnemyWaveUI>();
        selectWaveUIList = new List<EnemyWaveUI>();
        waveStatTextFormat = "Max Wave : {0}";
        waveTextForamt = "Wave{0}";
    }

    public void OnClickCancelButtonInEnemyWaveSettingMainPanel()
    {
        enemyWaveSettingMainPanel.SetActive(false);
    }

    public void OnClickAddWaveButton()
    {

        EnemyWaveUI enemyWaveUI = new EnemyWaveUI();
        int waveNum = this.enemyWaveUIList.Count + 1;

        //wave stat text
        waveStatText.text = string.Format(waveStatTextFormat, waveNum);

        //creat waveButton
        enemyWaveUI.enemyWaveButton = GameObject.Instantiate(commonButtonPrefab, this.waveButtons.transform);
        enemyWaveUI.enemyWaveButton.transform.GetChild(0).GetComponent<TMP_Text>().text = string.Format(waveTextForamt, waveNum);

        enemyWaveUI.enemyWaveButton.transform.GetComponent<Button>().onClick.AddListener(() => OnClickWaveButton(waveNum));
        enemyWaveUI.currentButtonColor = LevelEditorUISystem.ButtonColor.NotReadyColor;

        //create enemy wave UI
        enemyWaveUI.enemyWaveOptionalSettingPanel = GameObject.Instantiate(enemyWaveOptionalSettingPanelPrefab, this.enemyWaveSettingPanel.transform);
        enemyWaveUI.enemyWaveOptionalSettingPanel
            .transform.GetChild(2)
            .GetComponent<TMP_InputField>()
            .onEndEdit
            .AddListener((waveNum) => OnEndInEnemySpawnerDurationInputField(waveNum.ToString()));
        enemyWaveUI.enemyOneWavePanel = GameObject.Instantiate(enemyOneWavePanelPrefab, this.enemyWaveSettingPanel.transform);
        enemyWaveUI.enemyOneWavePanel.GetComponent<RectTransform>().anchoredPosition
            = new Vector2(enemyWaveUI.enemyWaveOptionalSettingPanel.GetComponent<RectTransform>().rect.width, 0);

        //create enemy wave info list
        enemyWaveUI.enemyWaveInfo = new StageData.EnemyWaveInfo();

        enemyWaveUIList.Add(enemyWaveUI);
    }

    public void OnClickRemoveButton()
    {
        removePanel.SetActive(true);
    }

    private void OnClickWaveButton(int waveNum)
    {
        Debug.Log("waveNum : " + waveNum);
        Debug.Log(enemyWaveUIList);
        EnemyWaveUI enemyWaveUI = null;
        int index = waveNum - 1;
        int i = 0;

        if (removePanel.activeSelf == true)
        {
            selectWaveUIList.Add(enemyWaveUIList[index]);
            LevelEditorUISystem.instance.ChangeButtonColor(enemyWaveUIList[index].enemyWaveButton
                ,LevelEditorUISystem.ButtonColor.SelectColor);

            return;
        }
        
        /*
         * flag = 0: 창이 다 꺼져있으면
         * flag = 1: 창이 켜져있는데 동일한 창을 켜려고하면
         * flag = 2: 창이 켜져있는데 다른 창이 켜면
         */
        int flag = 0;

        for (i = 0; i < enemyWaveUIList.Count; i++)
        {
            enemyWaveUI = enemyWaveUIList[i];

            if (enemyWaveUI.enemyWaveOptionalSettingPanel.activeSelf == true
                && enemyWaveUI.enemyOneWavePanel.activeSelf == true)
            {
                if (i == index)
                {
                    flag = 1;
                }
                else
                    flag = 2;

                break;
            }
        }

        if (flag == 0)
        {
            this.enemyWaveSettingPanel.SetActive(true);
            this.enemyWaveUIList[index].enemyWaveOptionalSettingPanel.SetActive(true);
            this.enemyWaveUIList[index].enemyOneWavePanel.SetActive(true);
        }
        else if (flag == 1)
        {
            OnClickCancelWaveButton(enemyWaveUI);
            this.enemyWaveSettingPanel.SetActive(false);
        }
        else
        {
            OnClickCancelWaveButton(enemyWaveUI);
            this.enemyWaveUIList[index].enemyWaveOptionalSettingPanel.SetActive(true);
            this.enemyWaveUIList[index].enemyOneWavePanel.SetActive(true);
        }
    }

    private void OnClickCancelWaveButton(EnemyWaveUI enemyWaveUI)
    {
        enemyWaveUI.enemyWaveOptionalSettingPanel.SetActive(false);
        enemyWaveUI.enemyOneWavePanel.SetActive(false);
    }

    public void OnClickRemoveButtonInRemovePanel()
    {
        //Remove enemy wave UI
        foreach (EnemyWaveUI enemyWaveUI in selectWaveUIList)
        {
            Destroy(enemyWaveUI.enemyWaveButton);
            Destroy(enemyWaveUI.enemyWaveOptionalSettingPanel);
            Destroy(enemyWaveUI.enemyOneWavePanel);
            enemyWaveUI.enemyWaveInfo = null;

            this.enemyWaveUIList.Remove(enemyWaveUI);
        }


        //Processing after deletion
        if (selectWaveUIList.Count > 0)
        {
            waveStatText.text = string.Format(waveStatTextFormat, enemyWaveUIList.Count);
            for (int i = 0; i < enemyWaveUIList.Count; i++)
            {
                int waveNum = i+1;
                enemyWaveUIList[i].enemyWaveButton.transform.GetChild(0).GetComponent<TMP_Text>().text = string.Format(waveTextForamt, waveNum);
                enemyWaveUIList[i].enemyWaveButton.transform.GetComponent<Button>().onClick.RemoveAllListeners();
                enemyWaveUIList[i].enemyWaveButton.transform.GetComponent<Button>().onClick.AddListener(() => OnClickWaveButton(waveNum));
            }

            selectWaveUIList.Clear();
        }
        
        removePanel.SetActive(false);
    }

    public void OnClickCancleButtonInRemovePanel()
    {
        //change color to previous
        foreach (EnemyWaveUI enemyWaveUI in selectWaveUIList)
        {
            LevelEditorUISystem.instance.ChangeButtonColor(enemyWaveUI.enemyWaveButton
                ,enemyWaveUI.currentButtonColor);
        }

        selectWaveUIList.Clear();
        removePanel.SetActive(false);
    }

    public void OnEndInEnemySpawnerDurationInputField(int waveNum)
    {
        //OnEndInputField(inputField, 4);
    }

    public void OnEndInBreakTimeInputField(int waveNum)
    {
        //OnEndInputField(inputField, 5);
    }

    private void OnEndInputField(GameObject inputField, int flag)
    {
        float num = -1f;
        Debug.Log(this.enemyWaveUIList);
        EnemyWaveUI enemyWaveUI = this.enemyWaveUIList.Find(x => x.enemyWaveOptionalSettingPanel == inputField.transform.parent);


        if (!float.TryParse(inputField.GetComponent<TMP_InputField>().text, out num))
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(flag));
            if (flag == 4)
                enemyWaveUI.enemyWaveInfo.enemySpawnDuration = -1f;
            else
                enemyWaveUI.enemyWaveInfo.breakTime = -1f;
        }
        else
        {
            if (flag == 4)
            { // enemySpawnDuration
                if (!((0.5 <= num) && (num <= 5)))
                {
                    enemyWaveUI.enemyWaveInfo.enemySpawnDuration = -1;
                    StartCoroutine(LevelEditorUISystem.instance.ShowMessage(flag));
                    inputField.GetComponent<TMP_InputField>().text = "";
                }
                else
                    enemyWaveUI.enemyWaveInfo.enemySpawnDuration = num;
            }
            else if (flag == 5)
            { // breakTime
                if (!((5 <= num) && (num <= 30)))
                {
                    enemyWaveUI.enemyWaveInfo.breakTime = -1;
                    StartCoroutine(LevelEditorUISystem.instance.ShowMessage(flag));
                    inputField.GetComponent<TMP_InputField>().text = "";
                }
                else
                    enemyWaveUI.enemyWaveInfo.breakTime = num;
            }
        }
    }


}
