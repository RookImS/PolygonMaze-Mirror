using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlankCreateButtonUISystem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject blankPrefab; // prefab

    private GameObject currentBlankObject; // current dragged real blank gameObject
    private GameObject currentCheckerWithGroundObject; // current dragged real checker gameObject
    private GameObject currentCheckerObject; // current dragged real checker gameObject
    private GameObject hitGameObject;// wallObject
    private static float currentBlankObjectHeight = 2f;
    private Vector3 realPos;
    private LayerMask wallLayer;
    private bool isDeployEnable;
    private bool firstInit;

    public void Awake()
    {
        firstInit = true;
        Init();
    }

    /* void Init()
     * 1. BlankCreateButtonUISystem의 변수 초기화
     */
    public void Init()
    {
        currentBlankObject = null;
        hitGameObject = null;
        isDeployEnable = false;
        wallLayer = 1 << LayerMask.NameToLayer("WallCollider");

        if (firstInit == false)
        {
            LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.PlayerSetting,
                    LevelEditorUISystem.ButtonColor.NotReadyColor);
        }
        firstInit = false;
    }

    /* void OnBeginDrag(PointerEventData eventData)
     * 1. 드래그용 blank가 생성된다. 
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        realPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentBlankObject = GameObject.Instantiate(blankPrefab, new Vector3(realPos.x, currentBlankObjectHeight, realPos.z), Quaternion.identity);
    }

    /* void OnDrag(PointerEventData eventData)
     * 1. 드래그 중인 blank가 마우스를 따라간다.
     * 2. 벽 위면 blank가 설치가능한 상태가 되고,
     *    그렇지 않으면 blank는 설치 불가능한 상태가 된다.
     */
    public void OnDrag(PointerEventData eventData)
    {
        Ray ray;
        RaycastHit hit;

        realPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y * 2f, wallLayer))
        {
            hitGameObject = hit.collider.gameObject;
            OnTheWall(hitGameObject);
        }
        else
        {
            hitGameObject = null;
            NotOnTheWall();
        }
    }

    /* void OnEndDrag(PointerEventData eventData)
     * 1. blank가 설치 가능상태일 때 drag를 그만두면 blank가 설치된다.
     *    blank가 설치 불가능상태일 때 drag를 그만두면 드래그 중인 blank가 사라진다.
     */
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDeployEnable)
            CreateBlank();
        else
            Destroy(currentBlankObject);
    }

    /* void CreateBlank()
     * 1-1. 설치중인 blank가 sapwner이면, checeker를 설치한다.
     * 1-2. LevelEditor의 spawner 정보를 갱신한다.
     * 1-3. LevelEditor의 spawner setting을 준비 상태로 변경한다.
     * 
     * 2-1. 설치중인 blank가 destination이면,
     *      LevelEditor의 destination 정보를 갱신한다.
     * 2-2. LevelEditor의 destination setting을 준비 상태로 변경한다.
     */
    public void CreateBlank()
    {
        hitGameObject.SetActive(false);

        if (hitGameObject != null)
        {
            if (currentBlankObject.CompareTag("Spawner"))
            {
                LevelEditor.instance.CreateChecker(currentBlankObject);

                LevelEditor
                    .instance
                    .SetSpawnerInfo(this.currentBlankObject
                    , this.currentCheckerWithGroundObject
                    , this.currentCheckerObject
                    , hitGameObject);

                CheckSettingComplete(currentBlankObject);
            }
            else if (currentBlankObject.CompareTag("Destination"))
            {
                LevelEditor
                    .instance
                    .SetDestinationInfo(this.currentBlankObject
                    , hitGameObject);

                CheckSettingComplete(currentBlankObject);
            }
            else
                Debug.Log("hitGameObject instance is null");
        }
        
    }

    /* void SetCheckerWithGroundObject(GameObject checkerWithGround)
     * 1. 현재 checkerWithGroundObject를 세팅한다.
     */
    public void SetCheckerWithGroundObject(GameObject checkerWithGround)
    {
        this.currentCheckerWithGroundObject = checkerWithGround;
    }

    /* void SetCheckerObject(GameObject checker)
     * 1. 현재 checker를 세팅한다.
     */
    public void SetCheckerObject(GameObject checker)
    {
        this.currentCheckerObject = checker;
    }

    /* void OnTheWall(GameObject hitGameObject)
     * 1. 현재 blank와 hit중인 오브젝트가 wall 일 때 해주어야하는 설정을 한다.
     *    blank의 크기를 현재 hit중인 wall의 크기와 동일하게 만들어주고,
     *    blank를 설치가능 상태로 변경한다.
     */
    public void OnTheWall(GameObject hitGameObject)
    {
        if (hitGameObject.transform.GetChild(0).gameObject.activeSelf == true)
        {
            SetActiveChildGameObject(currentBlankObject, 0);
        }
        else if (hitGameObject.transform.GetChild(1).gameObject.activeSelf == true)
        {
            SetActiveChildGameObject(currentBlankObject, 1);
        }
        else if (hitGameObject.transform.GetChild(2).gameObject.activeSelf == true)
        {
            SetActiveChildGameObject(currentBlankObject, 2);
        }
        else
        {
            Debug.Log("WallComponent is not 0~2 in OnTheWall of BlankButtonUI.cs");
        }

        currentBlankObject.transform.position = hitGameObject.transform.position;
        currentBlankObject.transform.rotation = hitGameObject.transform.rotation;

        isDeployEnable = true;
    }

    /* void NotOnTheWall()
     * 1. 현재 blank와 hit중인 오브젝트가 wall이 아닐 때 해줘야하는 설정을 한다.
     *    blank의 크기를 기존 크기로 변경시키고,
     *    blank를 설치가능 상태로 변경한다.
     */
    public void NotOnTheWall()
    {
        SetActiveChildGameObject(currentBlankObject, 0);

        currentBlankObject.transform.position = new Vector3(realPos.x, 2f, realPos.z);
        currentBlankObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);

        isDeployEnable = false;
    }

    /* void SetActiveChildGameObject(GameObject gobj, int num)
     * 1. 현재 blank와 hit중인 오브젝트가 wall이 아닐 때 해줘야하는 설정을 한다.
     */
    public void SetActiveChildGameObject(GameObject gobj, int num)
    {
        if(num == 0)
        {
            gobj.transform.GetChild(0).gameObject.SetActive(true);
            gobj.transform.GetChild(1).gameObject.SetActive(false);
            gobj.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if(num == 1)
        {
            gobj.transform.GetChild(0).gameObject.SetActive(false);
            gobj.transform.GetChild(1).gameObject.SetActive(true);
            gobj.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if(num == 2)
        {
            gobj.transform.GetChild(0).gameObject.SetActive(false);
            gobj.transform.GetChild(1).gameObject.SetActive(false);
            gobj.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("num is not 0~2 in SetActiveChildGameObject of BlankButtonUI.cs");
        }

    }

    public void CheckSettingComplete(GameObject blankObject)
    {
        if (blankObject != null)
        {
            if (blankObject.CompareTag("Spawner"))
            {
                if (LevelEditor.instance.GetSpawnerInfo() != null)
                {
                    LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.SpawnerSetting,
                        LevelEditorUISystem.ButtonColor.ReadyColor);

                    LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.SpawnerSetting
                        , true);
                }
            }
            else if (blankObject.CompareTag("Destination"))
            {
                if (LevelEditor.instance.GetDestinationInfo() != null)
                {
                    LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.DestinationSetting,
                        LevelEditorUISystem.ButtonColor.ReadyColor);

                    LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.DestinationSetting
                        , true);
                }

                if (LevelEditor.instance.GetSpawnerInfo() != null)
                {
                    LevelEditor
                        .instance
                        .GetSpawnerInfo()
                        .checker
                        .GetComponent<CheckerBehaviour>()
                        .CalculatePath();
                }
            }
        }
    }

    public void Load(GameObject blankObject)
    {
        CheckSettingComplete(blankObject);
    }
}
