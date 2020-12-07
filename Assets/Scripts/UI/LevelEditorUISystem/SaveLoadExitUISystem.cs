using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class SaveLoadExitUISystem : MonoBehaviour
{
    private static int chapterMin = 1;
    private static int chapterMax = 100;

    private static int levelMin = 1;
    private static int levelMax = 10;

    private int loadStageChapter;
    private int loadStageLevel;
    private bool firstInit;


    [Header("Save & Load & Exit Panel")]
    public GameObject savePanel;
    public GameObject loadPanel;
    public GameObject exitPanel;

    private void Awake()
    {
        firstInit = true;
        Init();
    }

    public void Init()
    {
        loadStageChapter = 0;
        loadStageLevel = 0;

        if (firstInit == false)
        {
            InitSavePanel();
            InitLoadPanel();
        }

        firstInit = false;
    }

    /* void OnEndEditStageChapter()
     * 1. 만약 inputf field의 stage chapter 값이 유효한 값이면
     *    LevelEditor의 stage chaptere 값이 해당 stage chapter 값이 되고,
     *    유효하지 않은 값이라면, LevelEditor의 stage chapter 값이 0이 된다.
     * 2. 만약 LevelEditor의 stage chapter 값이 0이면,
     *    stage chapter input field의 text가 ""이 된다.
     * 3. setting이 완료되었는지 확인한다.
     */
    public void OnEndEditStageChapter(GameObject panel)
    {
        int num = 0;

        TMP_InputField stageChapterInputField
            = panel.transform.GetChild(2).GetComponent<TMP_InputField>();

        if (!int.TryParse(stageChapterInputField.text, out num))
            num = 0;
        else
            if (!((chapterMin <= num) && (num <= chapterMax)))
                num = 0;

        if (num == 0)
            stageChapterInputField.text = "";

        if(panel.Equals(this.savePanel))
            LevelEditor.instance.SetStageChapter(num);
        else if (panel.Equals(this.loadPanel))
            this.loadStageChapter = num;

        CheckSettingComplete(panel);
    }

    /* void OnEndEditStageLevel()
     * 1. 만약 inputf field의 stage level 값이 유효한 값이면
     *    LevelEditor의 stage level 값이 해당 stage level 값이 되고,
     *    유효하지 않은 값이라면, LevelEditor의 stage level 값이 0이 된다.
     * 2. 만약 LevelEditor의 stage level 값이 0이면,
     *    stage level input field의 text가 ""이 된다.
     * 3. setting이 완료되었는지 확인한다.
     */
    public void OnEndEditStageLevel(GameObject panel)
    {
        int num = 0;

        TMP_InputField stageLevelInputField
            = panel.transform.GetChild(4).GetComponent<TMP_InputField>();

        if (!int.TryParse(stageLevelInputField.text, out num))
            num = 0;
        else
            if (!((levelMin <= num) && (num <= levelMax)))
                num = 0;

        if (num == 0)
            stageLevelInputField.text = "";

        if (panel.Equals(this.savePanel))
            LevelEditor.instance.SetStageLevel(num);
        else if (panel.Equals(this.loadPanel))
            this.loadStageLevel = num;

        CheckSettingComplete(panel);
    }

    /* void CheckSettingComplete()
     * 1. setting이 완료되었는지 확인한다.
     *    (stage chapter와 level 값이 모두 입력되었는지)
     * 2. 완료되었으면 setting을 준비상태로
     * 3. 그렇지않으면 setting을 준비되지 않음 상태로 바꾼다.
     */
    public void CheckSettingComplete(GameObject panel)
    {
        bool readyFlag = true;

        if (panel.Equals(this.savePanel))
        {
            if (LevelEditor.instance.GetStageChapter() == 0)
            {
                readyFlag = false;

                StartCoroutine(LevelEditorUISystem.instance.ShowMessage(13, chapterMin, chapterMax));
            }
            else if (LevelEditor.instance.GetStageLevel() == 0)
            {
                readyFlag = false;

                StartCoroutine(LevelEditorUISystem.instance.ShowMessage(14, levelMin, levelMax));
            }

            if (readyFlag == false)
            {
                LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.SaveSetting,
                    LevelEditorUISystem.ButtonColor.NotReadyColor);

                LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.SaveSetting
                    , false);
            }
            else
            {
                StartCoroutine(LevelEditorUISystem.instance.ShowMessage(15));

                LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.SaveSetting,
                    LevelEditorUISystem.ButtonColor.ReadyColor);

                LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.SaveSetting
                    , true);
            }
        }
        else if (panel.Equals(this.loadPanel))
        {
            if (this.loadStageChapter == 0)
            {
                readyFlag = false;

                StartCoroutine(LevelEditorUISystem.instance.ShowMessage(13, chapterMin, chapterMax));
            }
            else if (this.loadStageLevel == 0)
            {
                readyFlag = false;

                StartCoroutine(LevelEditorUISystem.instance.ShowMessage(14, levelMin, levelMax));
            }

            if (readyFlag == false)
            {
                LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.LoadSetting,
                    LevelEditorUISystem.ButtonColor.NotReadyColor);

                LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.LoadSetting
                    , false);
            }
            else
            {
                StartCoroutine(LevelEditorUISystem.instance.ShowMessage(15));

                LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.LoadSetting,
                    LevelEditorUISystem.ButtonColor.ReadyColor);

                LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.LoadSetting
                    , true);
            }
        }
    }

    /* void OnClickSaveButtonInSavePanel()
     * 1. save panel에서 save 버튼을 누르면,
     *    LevelEditor에서 지정한 값을 stage data에 저장하고, 이를 json 형식으로 로컬에 저장한다.
     */
    public void OnClickSaveButtonInSavePanel()
    {
        Debug.Log("Save Success");
        if (LevelEditor.instance.GetReadyFlag().saveSetting == true)
        {
            LevelEditor.instance.SaveStageData();
            //Show Save Success Message
            //Current Scene -> MainScene    
        }
        else
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(16));
        }

    }

    /* void OnClickCancleButtonInSavePanel()
     * 1. save setting panel을 닫는다.
     */
    public void OnClickCancleButtonInSavePanel()
    {
        savePanel.SetActive(false);
    }

    public void InitSavePanel()
    {
        TMP_InputField stageChapterInputField
            = this.savePanel.transform.GetChild(2).GetComponent<TMP_InputField>();

        stageChapterInputField.text = "";

        TMP_InputField stageLevelInputField
        = this.savePanel.transform.GetChild(4).GetComponent<TMP_InputField>();

        stageLevelInputField.text = "";

        LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.SaveSetting,
                    LevelEditorUISystem.ButtonColor.NotReadyColor);

        LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.SaveSetting
            , false);
    }

    /* void OnClickLoadButtonInExitPanel()
     * 
     */
    public void OnClickLoadButtonInLoadPanel()
    {
        if (LevelEditor.instance.GetReadyFlag().loadSetting == true)
        {
            GameManager.Instance.SetLoadStageChapter(this.loadStageChapter);
            GameManager.Instance.SetLoadStageLevel(this.loadStageLevel);

            if (LevelEditor.instance.LoadStageData())
            {
                Debug.Log("LoadScene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                GameManager.Instance.SetLoadStageChapter(0);
                GameManager.Instance.SetLoadStageLevel(0);
            }
        }
        else
        {
            StartCoroutine(LevelEditorUISystem.instance.ShowMessage(16));
        }
    }

    public void InitLoadPanel()
    {
        TMP_InputField stageChapterInputField
            = this.loadPanel.transform.GetChild(2).GetComponent<TMP_InputField>();

        stageChapterInputField.text = "";

        TMP_InputField stageLevelInputField
        = this.loadPanel.transform.GetChild(4).GetComponent<TMP_InputField>();

        stageLevelInputField.text = "";

        LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.LoadSetting,
                    LevelEditorUISystem.ButtonColor.NotReadyColor);

        LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.LoadSetting
            , false);
    }

    /* void OnClickCancleButtonInLoadPanel()
     * 1. load panel을 닫는다.
     */
    public void OnClickCancleButtonInLoadPanel()
    {
        loadPanel.SetActive(false);
    }

    /* void OnClickExitButtonInExitPanel()
     * 1. LevelEditor에서 빠져나와 main scene으로 이동한다.
     */
    public void OnClickExitButtonInExitPanel()
    {
        //Current Scene -> MainScene
    }

    /* void OnClickCancleButtonInExitPanel()
     * 1. exit panel을 닫는다.
     */
    public void OnClickCancleButtonInExitPanel()
    {
        exitPanel.SetActive(false);
    }

    public void Load()
    {
        TMP_InputField stageChapterInputField
            = savePanel.transform.GetChild(2).GetComponent<TMP_InputField>();

        TMP_InputField stageLevelInputField
            = savePanel.transform.GetChild(4).GetComponent<TMP_InputField>();

        if (LevelEditor.instance.GetStageChapter() == 0)
            stageChapterInputField.text = ""; 
        else
            stageChapterInputField.text = LevelEditor.instance.GetStageChapter().ToString();

        if (LevelEditor.instance.GetStageLevel() == 0)
            stageLevelInputField.text = "";
        else
            stageLevelInputField.text = LevelEditor.instance.GetStageLevel().ToString();

        CheckSettingComplete(savePanel);
        CheckSettingComplete(loadPanel);
    }
}
