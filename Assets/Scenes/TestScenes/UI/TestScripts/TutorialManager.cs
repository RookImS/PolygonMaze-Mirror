using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TutorialManager : MonoSingleton<TutorialManager>
{
    public GameObject TextPanel;
    TextMeshProUGUI TutorialText;

    [Header("SpawnerDestination")]
    public GameObject SpawnerDestination;
    public GameObject SDAnim;
    public EnabledTower Tower1;
    
    [Header("Life")]
    public GameObject Life;
    public GameObject LifeAnim;
    public EnabledTower Tower2;

    [Header("RemainEnemy")]
    public GameObject RemainEnemy;
    public GameObject REAnim;
    public EnabledTower Tower3;

    [Header("TowerDeployUI")]
    public GameObject TowerDeployUI;
    public GameObject TDAnim;
    public EnabledTower Tower4;

    [Header("NeutralTower")]
    public GameObject NeutralTower;
    public GameObject NTAnim;
    public GameObject DeployAnim;
    public EnabledTower Tower5;

    [Header("Cost")]
    public GameObject Cost;
    public GameObject CostAnim;

    [Header("EndTutorial")]
    public GameObject TutorialEnd;

    private void Awake()
    {
        NextTutorial += SpawnerDestinationTutorial;
        NextTutorial += DisabledSideColliders;
        TutorialText = DialogueManager.Instance.dialogueText;
    }

    private void Start()
    {
        OnNextTutorial();
    }

    public event Action NextTutorial;

    public void OnNextTutorial()
    {
        NextTutorial?.Invoke();
    }

    public void DisabledSideColliders()
    {
        GameObject[] SideColliders = GameObject.FindGameObjectsWithTag("SideCollider");
        foreach (GameObject SideCollider in SideColliders)
        {
            SideCollider.SetActive(false);
        }
    }
    public void EnabledSideColliders()
    {
        GameObject []
        SideColliders = GameObject.FindGameObjectsWithTag("SideCollider");
        foreach (GameObject SideCollider in SideColliders)
        {
            SideCollider.SetActive(true);
        }
    }
    private GameObject PreviousPanel;

    IEnumerator EnemyEscapeChecker(int a)
    {
        while (true)
        {
            if (GameManager.Instance.EnemyEscapeCount == a)
            {
                OnNextTutorial();
                GameManager.Instance.InitCounter();
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator TowerDeployChecker(int count)
    {
        while (true)
        {
            if (GameManager.Instance.DeployTowerCount == count)
            {
                OnNextTutorial();
                GameManager.Instance.InitCounter();
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void SetPanel(int a, int b) //튜토리얼 Dialogue Panel 생성
    {
        //Tutorial Dialogue Panel을 생성하고 DialogueText의 parent로 삽입.
        GameObject Set = Instantiate(TextPanel);
        Set.transform.SetParent(DialogueManager.Instance.NextDialoguePanel.transform, false);
        Set.transform.localPosition = new Vector3(a, b, 0);
        Set.transform.localScale = Vector3.one;
        TutorialText.transform.SetParent(Set.transform, false);

        //이전 패널을 삭제하고 다음에 삭제할 패널에 현재 패널 저장
        Destroy(PreviousPanel);
        PreviousPanel = Set;
    }

    public void SpawnerDestinationTutorial()
    {
        SetPanel(0, 0);
        SpawnerDestination.GetComponent<DialogueTrigger>().TriggerDialogue();
        DialogueManager.Instance.EndDialogue += SDTutorialEnd;
        DialogueManager.Instance.EndDialogue += OnNextTutorial;
    }
    public void SDTutorialEnd()
    {
        NextTutorial -= SpawnerDestinationTutorial;
        NextTutorial += LifeTutorial;
        foreach (GameObject Tower in Tower1.EnableTower)
        {
            Tower.SetActive(true);
        }
        //StartCoroutine(TowerDeployChecker(1));
        DialogueManager.Instance.EndDialogue -= SDTutorialEnd;
    }

    public void LifeTutorial()
    { 
        SetPanel(450, -150);
        Life.GetComponent<DialogueTrigger>().TriggerDialogue();
        DialogueManager.Instance.EndDialogue += LifeTutorialEnd;
        DialogueManager.Instance.EndDialogue += OnNextTutorial; // 플레이어 동작없이 다음 튜토리얼로 넘기기
    }
    public void LifeTutorialEnd()
    {
        NextTutorial -= LifeTutorial;
        NextTutorial += RemainEnemyTutorial;
        foreach (GameObject Tower in Tower2.EnableTower)
        {
            Tower.SetActive(true);
        }
        DialogueManager.Instance.EndDialogue -= LifeTutorialEnd;
        DialogueManager.Instance.EndDialogue -= OnNextTutorial;
    }

    public void RemainEnemyTutorial()
    {
        SetPanel(0, 0);
        RemainEnemy.GetComponent<DialogueTrigger>().TriggerDialogue();
        DialogueManager.Instance.EndDialogue += RETutorialEnd;
        DialogueManager.Instance.EndDialogue += OnNextTutorial;
    }
    public void RETutorialEnd()
    {
        NextTutorial -= RemainEnemyTutorial;
        NextTutorial += TowerDeployTutorial;
        foreach (GameObject Tower in Tower3.EnableTower)
        {
            Tower.SetActive(true);
        }
        DialogueManager.Instance.EndDialogue -= RETutorialEnd;
        DialogueManager.Instance.EndDialogue -= OnNextTutorial;
    }

    public void TowerDeployTutorial()
    {
        SetPanel(-450, -50);
        TowerDeployUI.GetComponent<DialogueTrigger>().TriggerDialogue();
        DialogueManager.Instance.EndDialogue += TDTutorialEnd;
        DialogueManager.Instance.EndDialogue += OnNextTutorial;
    }
    public void TDTutorialEnd()
    {
        NextTutorial -= TowerDeployTutorial;
        NextTutorial += NeutralTowerTutorial;
        foreach (GameObject Tower in Tower4.EnableTower)
        {
            Tower.SetActive(true);
        }
        DialogueManager.Instance.EndDialogue -= TDTutorialEnd;
        DialogueManager.Instance.EndDialogue -= OnNextTutorial;
    }
    public void NeutralTowerTutorial()
    {
        SetPanel(200, 0);
        NeutralTower.GetComponent<DialogueTrigger>().TriggerDialogue();
        DialogueManager.Instance.EndDialogue += NTTutorialEnd;
    }
    public void NTTutorialEnd()
    {
        DeployAnim.SetActive(true);
        NextTutorial -= NeutralTowerTutorial;
        NextTutorial += CostTutorial;
        foreach (GameObject Tower in Tower5.EnableTower)
        {
            Tower.SetActive(true);
        }
        StartCoroutine(TowerDeployChecker(1));
        DialogueManager.Instance.EndDialogue -= NTTutorialEnd;
    }

    public void CostTutorial()
    {
        DeployAnim.SetActive(false);
        SetPanel(450, 0);
        Cost.GetComponent<DialogueTrigger>().TriggerDialogue();
        DialogueManager.Instance.EndDialogue += CostTutorialEnd;
        DialogueManager.Instance.EndDialogue += OnNextTutorial;
    }
    public void CostTutorialEnd()
    {
        DeployAnim.SetActive(true);
        NextTutorial -= CostTutorial;
        NextTutorial += EndTutorial;

        DialogueManager.Instance.EndDialogue -= CostTutorialEnd;
        DialogueManager.Instance.EndDialogue -= OnNextTutorial;
    }
    public void EndTutorial()
    {
        DeployAnim.SetActive(false);
        SetPanel(0, 0);
        TutorialEnd.GetComponent<DialogueTrigger>().TriggerDialogue();
        EnabledSideColliders();
        //TutorialText.enabled = false;
        NextTutorial -= EndTutorial;
    }

}
