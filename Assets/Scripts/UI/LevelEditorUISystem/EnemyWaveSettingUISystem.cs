using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyWaveSettingUISystem : MonoBehaviour
{
    public class EnemyWaveUI
    {
        public GameObject enemyWaveOptionalSettingPanel;
        public GameObject enemyWaveInfoPanel;
        public List<GameObject> enemyWaveInfos;
    }

    private List<EnemyWaveUI> enemyWaveUIList;
    private static string waveStatTextFormat = "Wave: {0} / {1}";
    private static string waveTextForamt = "Wave{0}";

    [Header("Enemy Wave Setting")]
    public GameObject enemyWaveSettingMainPanel;
    public GameObject enemyWaveListPanel;
    public TextMeshProUGUI waveStatText;
    public GameObject enemyWaveSettingPanel;

    [Header("Preserved Prefabs")]
    public GameObject commonButtonPrefab;
    public GameObject enemyWaveOptionalSettingPanelPrefab;
    public GameObject enemyOneWavePanelPrefab;

    [Header("Preserved values")]
    public float waveButtonOffset = 20f;
    public int maxWaveNum = 5;

    private void Awake()
    {
        enemyWaveUIList = new List<EnemyWaveUI>();
    }

    public void OnClickCancelButtonInEnemyWaveSettingMainPanel() { }

    public void OnClickAddWaveButton(GameObject waveAddButton)
    {
        RectTransform waveAddButtonRect = waveAddButton.GetComponent<RectTransform>();
        GameObject newWaveButton = null;
        GameObject removeButton = null;
        EnemyWaveUI enemyWaveUI = null;

        
        int waveNum = this.enemyWaveUIList.Count + 1;   

        waveStatText.text = string.Format(waveStatTextFormat, waveNum, maxWaveNum);

        //creat waveButton
        newWaveButton = GameObject.Instantiate(commonButtonPrefab, this.enemyWaveListPanel.transform);
        newWaveButton.transform.GetChild(0).GetComponent<TMP_Text>().text = string.Format(waveTextForamt, waveNum);

        newWaveButton.GetComponent<RectTransform>().anchoredPosition = waveAddButtonRect.anchoredPosition;

        newWaveButton.transform.GetComponent<Button>().onClick.AddListener(() => OnClickWaveButton(waveNum));

        //move addWaveButton
        waveAddButtonRect.anchoredPosition
            = new Vector2(waveAddButtonRect.anchoredPosition.x,
            waveAddButtonRect.anchoredPosition.y - waveAddButtonRect.rect.height - waveButtonOffset);


        if (waveNum == maxWaveNum)
        {
            newWaveButton = GameObject.Instantiate(commonButtonPrefab, this.enemyWaveListPanel.transform);
            newWaveButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Remove";

            newWaveButton.GetComponent<RectTransform>().anchoredPosition = waveAddButtonRect.anchoredPosition;

            newWaveButton.transform.GetComponent<Button>().onClick.AddListener(() => OnClickRemoveButton());

            //move addWaveButton
            waveAddButtonRect.anchoredPosition
                = new Vector2(waveAddButtonRect.anchoredPosition.x,
                waveAddButtonRect.anchoredPosition.y - waveAddButtonRect.rect.height - waveButtonOffset);

            Destroy(waveAddButton);
        }


        enemyWaveUI = new EnemyWaveUI();
        enemyWaveUI.enemyWaveOptionalSettingPanel = GameObject.Instantiate(enemyWaveOptionalSettingPanelPrefab, this.enemyWaveSettingPanel.transform);
        enemyWaveUI.enemyWaveOptionalSettingPanel.GetComponent<RectTransform>().anchoredPosition
            = new Vector2(0, 0);
        enemyWaveUI.enemyWaveOptionalSettingPanel.SetActive(false);

        enemyWaveUI.enemyWaveInfoPanel = GameObject.Instantiate(enemyOneWavePanelPrefab, this.enemyWaveSettingPanel.transform);
        enemyWaveUI.enemyWaveInfoPanel.GetComponent<RectTransform>().anchoredPosition
            = new Vector2(enemyWaveUI.enemyWaveInfoPanel.GetComponent<RectTransform>().rect.width * 2, 0);
        enemyWaveUI.enemyWaveInfoPanel.SetActive(false);

        enemyWaveUI.enemyWaveInfos = new List<GameObject>();

        enemyWaveUIList.Add(enemyWaveUI);
    }

    private void OnClickWaveButton(int waveNum)
    {
        EnemyWaveUI enemyWaveUI = null;
        int index = waveNum - 1;
        int i = 0;
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
                && enemyWaveUI.enemyWaveInfoPanel.activeSelf == true)
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
            this.enemyWaveUIList[index].enemyWaveInfoPanel.SetActive(true);
        }
        else if (flag == 1)
        {
            this.enemyWaveSettingPanel.SetActive(false);
            OnClickCancelWaveButton(enemyWaveUI);
        }
        else
        {
            OnClickCancelWaveButton(enemyWaveUI);
            this.enemyWaveUIList[index].enemyWaveOptionalSettingPanel.SetActive(true);
            this.enemyWaveUIList[index].enemyWaveInfoPanel.SetActive(true);
        }
    }

    private void OnClickRemoveButton()
    {
        
    }

    private void OnClickCancelWaveButton(EnemyWaveUI enemyWaveUI)
    {
        enemyWaveUI.enemyWaveOptionalSettingPanel.SetActive(false);
        enemyWaveUI.enemyWaveInfoPanel.SetActive(false);
    }
}
