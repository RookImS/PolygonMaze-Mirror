using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObstacleDeleteUISystem : MonoBehaviour
{
    private GameObject selectedObstacle;

    [Header("Obstacle Remove Panel")]
    public GameObject obstacleRemovePanel;


    private void Awake()
    {
        selectedObstacle = null;
    }

    public void OnClickObstacle(GameObject selectedObstacle)
    {
        GameObject panel = obstacleRemovePanel;
        GameObject canvas = LevelEditorUISystem.instance.gameObject;

        if (LevelEditorUISystem.instance.GetCurrentPanel() == null
            && panel.activeSelf == false)
        {
            if (selectedObstacle.CompareTag("Obstacle"))
            {
                this.selectedObstacle = selectedObstacle;
                float obstaclePanelMaxPositionX = canvas.GetComponent<RectTransform>().rect.width
                    - panel.GetComponent<RectTransform>().rect.width;
                float obstaclePanelMinPositionY = 0 + panel.GetComponent<RectTransform>().rect.height;

                Vector2 viewPosition = Camera.main.WorldToViewportPoint(selectedObstacle.transform.position);
                Vector2 proportionalPosition = new Vector2(viewPosition.x * canvas.GetComponent<RectTransform>().sizeDelta.x,
                    viewPosition.y * canvas.GetComponent<RectTransform>().sizeDelta.y);

                if (obstaclePanelMaxPositionX < proportionalPosition.x)
                {
                    proportionalPosition = new Vector2(proportionalPosition.x - panel.GetComponent<RectTransform>().rect.width
                        , proportionalPosition.y);
                }

                if (obstaclePanelMinPositionY > proportionalPosition.y)
                {
                    proportionalPosition = new Vector2(proportionalPosition.x
                        , proportionalPosition.y + panel.GetComponent<RectTransform>().rect.height);
                }

                panel.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                panel.GetComponent<RectTransform>().anchoredPosition = proportionalPosition;

                panel.SetActive(true);
            }
        }
    }


    public void OnClickDeleteButtonObstacleRemovePanel()
    {
        LevelEditor.instance.DeleteObstacle(this.selectedObstacle);

        this.obstacleRemovePanel.SetActive(false);

        if (LevelEditor.instance.GetObstacleList().Count <= 0)
        {
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                , LevelEditorUISystem.ButtonColor.NotReadyColor);

            LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                , false);
        }

        LevelEditor.instance.SetNavMeshUpdateFlag();
    }

    public void OnClickCancelButtonInObstacleRemovePanel()
    {
        this.obstacleRemovePanel.SetActive(false);
    }

}
