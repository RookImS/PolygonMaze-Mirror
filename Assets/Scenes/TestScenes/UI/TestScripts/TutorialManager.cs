using System;
using System.Collections;
using System.Collections.Generic;
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




    private void Awake()
    {
        EndDialogue += SpawnerDestinationTutorial;
    }
    private void Start()
    {
        EndDialogue();
    }
    public void SpawnerDestinationTutorial()
    {
        SpawnerDestination.GetComponent<DialogueTrigger>().TriggerDialogue();
        SDAnim.SetActive(true);
        EndDialogue += LifeTutorial;
    }

    public void LifeTutorial()
    {
        //SDAnim.SetActive(false);
        Life.GetComponent<DialogueTrigger>().TriggerDialogue();
        SDAnim.SetActive(false);
        LifeAnim.SetActive(true);
        EndDialogue -= LifeTutorial;
        EndDialogue += RemainEnemyTutorial;
    }

    public void RemainEnemyTutorial()
    {
        RemainEnemy.GetComponent<DialogueTrigger>().TriggerDialogue();
        LifeAnim.SetActive(false);
        LifeAnim.SetActive(true);
        EndDialogue -= RemainEnemyTutorial;
        EndDialogue += TowerDeployTutorial;
    }

    public void TowerDeployTutorial()
    {
        TowerDeployUI.GetComponent<DialogueTrigger>().TriggerDialogue();
    }

    public void NeutralTowerTutoral()
    {
        NeutralTower.GetComponent<DialogueTrigger>().TriggerDialogue();
    }

    public event Action EndDialogue;

    public void OnEndDialogue()
    {
        EndDialogue?.Invoke();
    }
}
