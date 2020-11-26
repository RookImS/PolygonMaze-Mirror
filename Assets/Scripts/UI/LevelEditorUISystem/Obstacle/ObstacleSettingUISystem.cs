using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObstacleSettingUISystem : MonoBehaviour
{
    [Header("Obstacle Setting Panel")]
    public GameObject obstacleSettingPanel;

    public void Init()
    {
        LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                , LevelEditorUISystem.ButtonColor.NotReadyColor);
    }

    public void ShowPanel()
    {
        if (LevelEditor.instance.GetSpawnerInfo() == null
            || LevelEditor.instance.GetDestinationInfo() == null)
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(12));
        }
        else
            obstacleSettingPanel.SetActive(true);
    }

    public void OnClickResetButton()
    {
        List<GameObject> Obstacles = LevelEditor.instance.GetObstacleList();

        foreach (GameObject obj in Obstacles)
            Destroy(obj);

        Obstacles.Clear();

        if (LevelEditor.instance.GetObstacleList().Count <= 0)
        {
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                , LevelEditorUISystem.ButtonColor.NotReadyColor);

            LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                , false);
        }

        LevelEditor.instance.SetNavMeshUpdateFlag();
    }

    public void Load()
    {
        if (LevelEditor.instance.GetObstacleList().Count <= 0)
        {
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                , LevelEditorUISystem.ButtonColor.NotReadyColor);

            LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                , false);
        }
        else
        {
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                , LevelEditorUISystem.ButtonColor.ReadyColor);

            LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                , true);
        }
    }
}
