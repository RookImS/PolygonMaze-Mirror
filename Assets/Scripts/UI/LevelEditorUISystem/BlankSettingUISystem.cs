using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankSettingUISystem : MonoBehaviour
{
    public enum BlankSpecific
    {
        Spawner,
        Destination
    }

    private GameObject selectedBlank;

    [Header("Spawner Setting Panel")]
    public GameObject spawnerPanel;

    [Header("Destination Setting Panel")]
    public GameObject destinationPanel;

    private void Awake()
    {
        selectedBlank = null;
    }

    public void OnClickBlank(GameObject selectedBlank, BlankSpecific blankSepcific)
    {
        GameObject panel = null;
        GameObject canvas = LevelEditorUISystem.instance.gameObject;

        if (blankSepcific == BlankSpecific.Spawner)
            panel = this.spawnerPanel;
        else if (blankSepcific == BlankSpecific.Destination)
            panel = this.destinationPanel;

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


    public void OnClickDeleteButtonInSpawnerPanel()
    {
        GameObject disabledWall = LevelEditor.instance.DeleteSpawner(this.selectedBlank);
        Destroy(this.selectedBlank);
        disabledWall.SetActive(true);

        this.spawnerPanel.SetActive(false);

        if (LevelEditor.instance.GetSpawnerList().Count <= 0)
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.SpawnerSetting,
                LevelEditorUISystem.ButtonColor.NotReadyColor);

    }

    public void OnClickCancelButtonInSpawnerPanel()
    {
        this.spawnerPanel.SetActive(false);
    }


    public void OnClickDeleteButtonInDestinationPanel()
    {
        GameObject disabledWall = LevelEditor.instance.DeleteDestination(this.selectedBlank);
        Destroy(this.selectedBlank);
        disabledWall.SetActive(true);

        this.destinationPanel.SetActive(false);

        if (LevelEditor.instance.GetDestinationList().Count <= 0)
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.DestinationSetting,
                LevelEditorUISystem.ButtonColor.NotReadyColor);
    }

    public void OnClickCancelButtonInDestinationPanel()
    {
        this.destinationPanel.SetActive(false);
    }
}
