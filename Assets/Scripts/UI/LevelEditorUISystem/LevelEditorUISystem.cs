using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelEditorUISystem : MonoBehaviour
{
    public static LevelEditorUISystem instance { get; private set; }

    private GameObject canvas;
    private GameObject selectedBlank;

    public GameObject playerSettingPanel;
    public GameObject mainPanel;
    public GameObject spawnerPanel;
    public GameObject destinationPanel;
    public GameObject obstaclePanel;
    public GameObject enemyWaveSettingPanel;
    public GameObject savePanel;
    public GameObject exitPanel;
    public GameObject warningMessagePrefab;


    private void Awake()
    {
        instance = this;
        canvas = this.gameObject;
        selectedBlank = null;
    }

    //related player setting
    public void OnClickPlayerSettingPanel()
    {
        if(playerSettingPanel.activeSelf == true)
            playerSettingPanel.SetActive(false);
        else
            playerSettingPanel.SetActive(true);
    }

    public void OnEndInPlayerLifeInputField(GameObject inputField)
    {
        string message = "Please modify the value in player life input field as integer value between 1 and 20!";
        OnEndInputField(inputField, message, 0);
    }

    public void OnEndInStartCostInputField(GameObject inputField)
    {
        string message = "Please modify the value in start cost input field as integer value between 50 and 2000!";
        OnEndInputField(inputField, message, 1);
    }

    private void OnEndInputField(GameObject inputField, string message, int flag)
    {
        int num = 0;

        if (!int.TryParse(inputField.GetComponent<TMP_InputField>().text, out num))
        {
            StartCoroutine(ShowWarningMessage(message));
            inputField.GetComponent<TMP_InputField>().text = "";
        }
        else
        {
            if (flag == 0) // playerLife
            {
                if (!((1 <= num) && (num <= 20)))
                {
                    StartCoroutine(ShowWarningMessage(message));
                    inputField.GetComponent<TMP_InputField>().text = "";
                }
            }
            else // startCost
            {
                if (!((50 <= num) && (num <= 2000)))
                {
                    StartCoroutine(ShowWarningMessage(message));
                    inputField.GetComponent<TMP_InputField>().text = "";
                }
            }
        }
    }

    IEnumerator ShowWarningMessage(string message)
    {
        GameObject warningMessage = GameObject.Instantiate(warningMessagePrefab, this.gameObject.transform);
        warningMessage.GetComponent<TMP_Text>().text = message;
        warningMessage.transform.SetParent(this.gameObject.transform);

        yield return new WaitForSeconds(2);

        Destroy(warningMessage);

    }

    public void OnClickSaveButtonInPlayerSettingPanel(GameObject panel)
    {

    }

    //related blank(spawner and destination)
    public void ClickBlank(GameObject selectedBlank, int flag)
    {
        GameObject panel = null;

        if (flag == 1)
            panel = spawnerPanel;
        else if (flag == 2)
            panel = destinationPanel;

        this.selectedBlank = selectedBlank;
        float blankPanelMaxPositionX = canvas.GetComponent<RectTransform>().rect.width
            - panel.GetComponent<RectTransform>().rect.width;
        float blankPanelMinPositionY = 0 + panel.GetComponent<RectTransform>().rect.height;

        Vector2 viewPosition = Camera.main.WorldToViewportPoint(selectedBlank.transform.position);
        Vector2 proportionalPosition = new Vector2(viewPosition.x * canvas.GetComponent<RectTransform>().sizeDelta.x,
            viewPosition.y * canvas.GetComponent<RectTransform>().sizeDelta.y);

        if (blankPanelMaxPositionX < proportionalPosition.x)
        {
            proportionalPosition = new Vector2(proportionalPosition.x - panel.GetComponent<RectTransform>().rect.width
                , proportionalPosition.y);
        }

        if (blankPanelMinPositionY > proportionalPosition.y)
        {
            proportionalPosition = new Vector2(proportionalPosition.x
                , proportionalPosition.y + panel.GetComponent<RectTransform>().rect.height);
        }

        panel.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        panel.GetComponent<RectTransform>().anchoredPosition = proportionalPosition;

        panel.SetActive(true);
    }

    public void ClickDeleteButtonInBlankPanel(GameObject panel)
    {
        GameObject disabledWall = LevelEditor.instance.DeleteBlank(this.selectedBlank);
        Destroy(this.selectedBlank);
        disabledWall.SetActive(true);

        panel.SetActive(false);
    }

    public void ClickCancleButton(GameObject panel)
    {
        panel.SetActive(false);
    }

}

