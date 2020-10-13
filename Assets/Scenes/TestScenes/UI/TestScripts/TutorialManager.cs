using System;
using UnityEngine;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [Header("SpawnerDestination")]
    public GameObject SpawnerDestination;
    public GameObject SDAnim;

    [Header("Life")]
    public GameObject Life;
    public GameObject LifeAnim;

    [Header("RemainEnemy")]
    public GameObject RemainEnemy;
    public GameObject REAnim;

    [Header("TowerDeployUI")]
    public GameObject TowerDeployUI;
    public GameObject TDAnim;

    [Header("NeutralTower")]
    public GameObject NeutralTower;
    public GameObject NTAnim;

    [Header("EndTutorial")]
    public GameObject TutorialEnd;

    private void Awake()
    {
        NextTutorial += SpawnerDestinationTutorial;
    }

    private void Start()
    {
        NextTutorial();
    }

    public void FirstTutorial()
    {
        if (GameManager.Instance.EnemyDeathCount == 1)
        {
            OnNextTutorial();
            GameManager.Instance.InitCount();
        }
    }

    public event Action NextTutorial;

    public void OnNextTutorial()
    {
        NextTutorial?.Invoke();
    }

    public void SpawnerDestinationTutorial()
    {
        SpawnerDestination.GetComponent<DialogueTrigger>().TriggerDialogue();
        SDAnim.SetActive(true);
        NextTutorial += LifeTutorial;
    }

    public void LifeTutorial()
    {
        //SDAnim.SetActive(false);
        Life.GetComponent<DialogueTrigger>().TriggerDialogue();
        SDAnim.SetActive(false);
        LifeAnim.SetActive(true);
        NextTutorial -= LifeTutorial;
        NextTutorial += RemainEnemyTutorial;
    }

    public void RemainEnemyTutorial()
    {
        RemainEnemy.GetComponent<DialogueTrigger>().TriggerDialogue();
        LifeAnim.SetActive(false);
        //REAnim.SetActive(true);
        NextTutorial -= RemainEnemyTutorial;
        NextTutorial += TowerDeployTutorial;
    }

    public void TowerDeployTutorial()
    {
        TowerDeployUI.GetComponent<DialogueTrigger>().TriggerDialogue();
        //REAnim.SetActive(false);
        TDAnim.SetActive(true);
        NextTutorial -= TowerDeployTutorial;
        NextTutorial += NeutralTowerTutoral;
    }

    public void NeutralTowerTutoral()
    {
        NeutralTower.GetComponent<DialogueTrigger>().TriggerDialogue();
        TDAnim.SetActive(false);
        NTAnim.SetActive(true);
        NextTutorial -= NeutralTowerTutoral;
        NextTutorial += EndTutorial;
    }
    public void EndTutorial()
    {
        TutorialEnd.GetComponent<DialogueTrigger>().TriggerDialogue();
        NTAnim.SetActive(false);
        NextTutorial -= EndTutorial;
    }
    

}
