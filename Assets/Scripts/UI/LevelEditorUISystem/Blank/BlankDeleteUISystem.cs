using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankDeleteUISystem : MonoBehaviour
{
    public enum BlankSpecific
    {
        Spawner,
        Destination
    }

    [Header("Spawner Remove Panel")]
    public GameObject spawnerRemovePanel;

    [Header("Destination Remove Panel")]
    public GameObject destinationRemovePanel;

    private void Awake()
    {
        
    }

    /* void OnClickBlank(GameObject selectedBlank, BlankSpecific blankSepcific)
     * 1. 어떠한 setting 패널이 켜져있지 않을 때 blank를 클릭하면,
     *    blank 삭제 패널이 켜진다.
     */
    public void OnClickBlank(GameObject selectedBlank, BlankSpecific blankSepcific)
    {
        if (LevelEditorUISystem.instance.GetCurrentPanel() == null)
        {
            GameObject panel = null;
            GameObject canvas = LevelEditorUISystem.instance.gameObject;

            if (blankSepcific == BlankSpecific.Spawner)
                panel = this.spawnerRemovePanel;
            else if (blankSepcific == BlankSpecific.Destination)
                panel = this.destinationRemovePanel;

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
    }

    /* void OnClickDeleteButtonInSpawnerPanel()
     * 1. spawner 삭제 패널에서 삭제 버틀을 누르면,
     *    해당 spanwer이 삭제되고,
     *    spawner setting이 준비되지않은 상태가 된다.
     */
    public void OnClickDeleteButtonInSpawnerPanel()
    {
        LevelEditor.instance.DeleteSpawner();
        this.spawnerRemovePanel.SetActive(false);

        if (LevelEditor.instance.GetSpawnerInfo() == null)
        {
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.SpawnerSetting,
                LevelEditorUISystem.ButtonColor.NotReadyColor);

            LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.SpawnerSetting
                , false);
        }

    }

    /* void OnClickCancelButtonInSpawnerPanel()
     * 1. spawner 삭제 패널을 닫는다.
     */
    public void OnClickCancelButtonInSpawnerPanel()
    {
        this.spawnerRemovePanel.SetActive(false);
    }

    /* void OnClickDeleteButtonInDestinationPanel()
     * 1. destination 삭제 패널에서 삭제 버틀을 누르면,
     *    해당 destination 삭제되고,
     *    destination setting이 준비되지않은 상태가 된다.
     */
    public void OnClickDeleteButtonInDestinationPanel()
    {
        LevelEditor.instance.DeleteDestination();
        this.destinationRemovePanel.SetActive(false);

        if (LevelEditor.instance.GetDestinationInfo() == null)
        {
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.DestinationSetting,
                LevelEditorUISystem.ButtonColor.NotReadyColor);

            LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.DestinationSetting
                , false);
        }
    }

    /* OnClickCancelButtonInDestinationPanel()
     * 1. destination 삭제 패널을 닫는다.
     */
    public void OnClickCancelButtonInDestinationPanel()
    {
        this.destinationRemovePanel.SetActive(false);
    }
}
