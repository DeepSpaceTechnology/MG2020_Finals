using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    //public GameObject tmpSphere;
    public static UIMgr instance;
    public Transform uiRoot1;
    public int uiState = 0;         //0未选择  1收买  2信息轰炸  3律师函  4饭圈文化
    Ray ray;         //点击时才使用的射线
    public Camera camera;
    RaycastHit hit;
    Ray alwaryRay;         //一直持续的射线
    RaycastHit alwaryHit;
    People curAlwaryPeople;     //持续射线打到的人
    Vector3 mousePosInWorld;

    //top指针
    public RectTransform totalPtr;  //平均支持指针
    public Text totalPtrText;
    public Image topSign;
    public Sprite redtop;
    public Sprite buletop;

    public LeftTip leftTip;

    public People curCardPeople;        //当前信息卡显示的people的引用
    public PeoInfoCard peoInfoCard;
    public Vector3 cardOffset;

    public MouseChange mouseChange;  //改变鼠标图形
    public ShowMoney showMoney;


    //收买
    public NeedMoney needMoney;      //显示所需钱的窗口
    public Vector3 needmoneyOffset;
    public GameObject go_talk;          //收买后的气泡
    public Vector3 talkOffset;

    public GameObject cancelTip;        //取消技能提示
    public NewsRoll newsRoll;

    //金币
    public GameObject coinPrefab;
    public Transform coinRoot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        Time.timeScale = 0.7f;
        ClearAllSkillWnd();
        showMoney.UpdateMoney();
    }

    void UpdateTotalPtr()
    {
        float tmp = (PeopleManager.instance.agCount - PeopleManager.instance.disCount) / (float)(PeopleManager.instance.totalNum);
        float targetx = 500 * tmp;
        totalPtr.localPosition = new Vector3(Mathf.Lerp(totalPtr.localPosition.x, targetx, Time.deltaTime * 5f),
            totalPtr.localPosition.y, totalPtr.localPosition.z);
        int num = (int)(tmp * 100);
        if (num < 0) {
            topSign.sprite = buletop;
            num = -num;
        }
        else { topSign.sprite = redtop; }
        totalPtrText.text = num.ToString();
        GameRoot.instance.TryShowTipByTotalPtr(tmp);
    }

    public void ClearAllSkillWnd()
    {
        //peoInfoCard.gameObject.SetActive(false);
        needMoney.gameObject.SetActive(false);
        mouseChange.isScope = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseImageByState();
        UpdateTotalPtr();
        if (peoInfoCard.gameObject.activeSelf && curCardPeople != null)     //卡片信息位移
        {
            peoInfoCard.transform.position = Camera.main.WorldToScreenPoint(curCardPeople.transform.position + cardOffset);
        }

        if (Input.GetMouseButtonDown(1))        //右键取消技能释放
        {
            uiState = 0;
            ClearAllSkillWnd();
            peoInfoCard.gameObject.SetActive(false);
        }
        if (uiState != 0)
        {
            cancelTip.SetActive(true);
        }
        else {
            cancelTip.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (uiState == 0 && hit.transform.CompareTag("people"))     //点人显示卡片信息
                {
                    curCardPeople = hit.transform.gameObject.GetComponent<People>();
                    peoInfoCard.gameObject.SetActive(true);
                    peoInfoCard.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position + cardOffset);
                    peoInfoCard.SetInfo(curCardPeople.type, curCardPeople.agree);
                }
                //else if (uiState == 0 && !isClickOnUI())     //没点到人 也没点到ui  则隐藏卡片信息
                //{
                //    peoInfoCard.gameObject.SetActive(false);
                //}
                else if (uiState == 1 && hit.transform.CompareTag("people"))     //点击要收买的人
                {
                    BuyPeople();
                }
                else if (uiState == 2)     //信息炸弹
                {
                    InfoBomb();
                }
                else if (uiState == 3 && hit.transform.CompareTag("people"))     //律师函
                {
                    Lawyerletter();
                }
                else if (uiState == 4 )     //饭圈文化
                {
                    FansClub();
                }
                else if (!isClickOnUI())     //没点到人 也没点到ui  则隐藏卡片信息
                {
                    Debug.Log("click nothing");
                    peoInfoCard.gameObject.SetActive(false);
                }
            }
        }
    }

    public bool isClickOnUI()       //是否点击在ui上
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster gr = gameObject.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            return true;
        }
        return false;
    }

    public void OverTalkUpdate(int id1, int id2)
    {       //供people对话结束时调用，更新卡片信息
        if (curCardPeople == null) { return; }
        if (id1 == curCardPeople.pid || id2 == curCardPeople.pid)
        {
            peoInfoCard.UpdateAgree(curCardPeople.agree);
        }
    }

    public bool isOnPeople()
    {                  //鼠标是否在people上
        alwaryRay = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(alwaryRay, out alwaryHit))
        {
            mousePosInWorld = alwaryHit.point;
            if (alwaryHit.transform.CompareTag("people"))
            {
                curAlwaryPeople = alwaryHit.transform.gameObject.GetComponent<People>();
                return true;
            }
        }
        curAlwaryPeople = null;
        return false;
    }

    public void UpdateMouseImageByState()
    {
        if (uiState == 0)       //查看
        {
            if (isOnPeople())
            {
                mouseChange.index = 1;
            }
            else
            {
                mouseChange.index = 0;
            }
        }
        else if (uiState == 1)          //收买
        {
            if (isOnPeople())
            {
                mouseChange.index = 3;
                needMoney.SetMoney(curAlwaryPeople.money);
                needMoney.gameObject.SetActive(true);
                needMoney.transform.position = Camera.main.WorldToScreenPoint(curAlwaryPeople.transform.position + needmoneyOffset);
            }
            else
            {
                mouseChange.index = 2;
                needMoney.gameObject.SetActive(false);
            }
        }
        else if (uiState == 2)      //信息炸弹
        {
            mouseChange.isScope = true;
            
            if (isOnPeople())
            {
                mouseChange.index = 4;
            }
            else
            {
                mouseChange.index = 4;
            }
        }
        else if (uiState == 3)      //律师函
        {
            if (isOnPeople())
            {
                mouseChange.index = 6;
            }
            else
            {
                mouseChange.index = 5;
            }
        }
        else if (uiState == 4)      //饭圈文化
        {
            mouseChange.isScope = true;
            if (isOnPeople())
            {
                mouseChange.index = 7;
            }
            else
            {
                mouseChange.index = 7;
            }
        }
    }

    //#######>>>>>>>>==== 收买 =====<<<<<<<<#########
    //点击按钮，进入收买状态 
    public void SelectSkill_1()
    {
        ClearAllSkillWnd();
        uiState = 1;
    }

    public void BuyPeople()
    {
        if (GameRoot.instance.money >= curAlwaryPeople.money)
        {
            Debug.Log("收买");
            AudioManager.Instance.PlayAudio("花钱");
            GameRoot.instance.money -= curAlwaryPeople.money;
            showMoney.UpdateMoney();
            curAlwaryPeople.BuyPeople();
            AfterBuyShow abs= Instantiate(go_talk, uiRoot1).GetComponent<AfterBuyShow>();
            string tmpstr = GameRoot.instance.GetRamTalkByType(curAlwaryPeople.type);
            abs.SetTalk(tmpstr, curAlwaryPeople.transform, talkOffset);
            abs.transform.position = Camera.main.WorldToScreenPoint(curAlwaryPeople.transform.position + talkOffset);

            switch (curAlwaryPeople.type)
            {
                case 0:
                    if (GameRoot.instance.tipTimer < 0 && GameRoot.instance.firstBuy0)
                    {
                        GameRoot.instance.firstBuy0 = false;
                        GameRoot.instance.ShowTip(0, 2f);        //句子id,waittime
                    }
                    break;
                case 1:
                    if (GameRoot.instance.tipTimer < 0 && GameRoot.instance.firstBuy1)
                    {
                        GameRoot.instance.firstBuy1 = false;
                        GameRoot.instance.ShowTip(1, 2f);        //句子id,waittime
                    }
                    break;
                case 2:
                    if (GameRoot.instance.tipTimer < 0 && GameRoot.instance.firstBuy2)
                    {
                        GameRoot.instance.firstBuy2 = false;
                        GameRoot.instance.ShowTip(2, 2f);        //句子id,waittime
                    }
                    break;
                case 3:
                    if (GameRoot.instance.tipTimer < 0 && GameRoot.instance.firstBuy3)
                    {
                        GameRoot.instance.firstBuy3 = false;
                        GameRoot.instance.ShowTip(3, 2f);        //句子id,waittime
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("资金不足");
            showMoney.Warn();
        }
        uiState = 0;
        needMoney.gameObject.SetActive(false);
    }


    //#######>>>>>>>>==== 信息轰炸 =====<<<<<<<<#########

    public void SelectSkill_2()
    {
        ClearAllSkillWnd();
        uiState = 2;
    }

    public void InfoBomb()
    {
        if (GameRoot.instance.money < GameRoot.instance.skillPrice[1])
        {
            Debug.Log("资金不足");
            showMoney.Warn();
        }
        else {
            Debug.Log("消息轰炸");
            AudioManager.Instance.PlayAudio("纸张");
            GameRoot.instance.money -= GameRoot.instance.skillPrice[1];
            showMoney.UpdateMoney();

            List<People> plist = new List<People>();
            Vector3 r = new Vector3(0, -0.5f, 0.5f);
            if (curAlwaryPeople==null) { r = Vector3.zero; }
            //Collider[] colliders = Physics.OverlapSphere(mousePosInWorld + r, 0.6f, LayerMask.GetMask("people"));
            //Instantiate(tmpSphere, mousePosInWorld + r, Quaternion.identity);
            
            Collider[] colliders = Physics.OverlapBox(mousePosInWorld + r, new Vector3(0.6f,1f,1f),Quaternion.identity,LayerMask.GetMask("people"));
            foreach (Collider co in colliders)
            {
                plist.Add(co.gameObject.GetComponent<People>());
            }
            Debug.Log("list:  "+plist.Count);
            foreach (People p in plist)
            {
                p.InfoBomb();
            }

            if (GameRoot.instance.tipTimer < 0 && GameRoot.instance.firstSkill_infoBoom)
            {
                GameRoot.instance.firstSkill_infoBoom = false;
                GameRoot.instance.ShowTip(4, 1f);        //句子id,waittime
            }
        }

        uiState = 0;
        mouseChange.isScope=false;
    }



    //#######>>>>>>>>==== 律师函 =====<<<<<<<<#########

    public void SelectSkill_3()
    {
        ClearAllSkillWnd();
        uiState = 3;
    }

    public void Lawyerletter()
    {
        if (GameRoot.instance.money >= GameRoot.instance.skillPrice[2])
        {
            Debug.Log("律师函");
            AudioManager.Instance.PlayAudio("锤子");
            GameRoot.instance.money -= GameRoot.instance.skillPrice[2];
            curAlwaryPeople.Lawyerletter();

            if (GameRoot.instance.tipTimer < 0 && GameRoot.instance.firstSkill_lawyer)
            {
                GameRoot.instance.firstSkill_lawyer = false;
                GameRoot.instance.ShowTip(5, 1f);        //句子id,waittime
            }
        }
        else
        {
            Debug.Log("资金不足");
            showMoney.Warn();
        }
        uiState = 0;
        needMoney.gameObject.SetActive(false);
    }



    //#######>>>>>>>>==== 饭圈文化 =====<<<<<<<<#########

    public void SelectSkill_4()
    {
        ClearAllSkillWnd();
        uiState = 4;
    }

    public void FansClub() {
        if (GameRoot.instance.money < GameRoot.instance.skillPrice[3])
        {
            Debug.Log("资金不足");
            showMoney.Warn();
        }
        else
        {
            Debug.Log("狂热群体");
            AudioManager.Instance.PlayAudio("狂热");
            GameRoot.instance.money -= GameRoot.instance.skillPrice[3];
            showMoney.UpdateMoney();

            List<People> plist = new List<People>();
            Vector3 r = new Vector3(0, -0.5f, 0.5f);
            if (curAlwaryPeople == null) { r = Vector3.zero; }
            //Collider[] colliders = Physics.OverlapSphere(mousePosInWorld + r, 0.6f, LayerMask.GetMask("people"));
            //Instantiate(tmpSphere, mousePosInWorld + r, Quaternion.identity);

            Collider[] colliders = Physics.OverlapBox(mousePosInWorld + r, new Vector3(0.6f, 1f, 1f), Quaternion.identity, LayerMask.GetMask("people"));
            foreach (Collider co in colliders)
            {
                plist.Add(co.gameObject.GetComponent<People>());
            }
            Debug.Log("list:  " + plist.Count);
            foreach (People p in plist)
            {
                p.FansClub();
            }

            if (GameRoot.instance.tipTimer < 0 && GameRoot.instance.firstSkill_fansclub)
            {
                GameRoot.instance.firstSkill_fansclub = false;
                GameRoot.instance.ShowTip(6, 1f);        //句子id,waittime
            }
        }
        uiState = 0;
        mouseChange.isScope = false;
    }

    //#######>>>>>>>>==== 金币掉落 =====<<<<<<<<#########
    public void ShowCoin(List<Vector3> clist)
    {
        foreach (Vector3 i in clist)
        {
            GameObject go = Instantiate(coinPrefab, coinRoot);
            go.transform.position = i;
        }
        if (clist.Count > 0)
        {
            StartCoroutine(DelCoin());
        }
    }
    IEnumerator DelCoin()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < coinRoot.childCount; i++)
        {
            Destroy(coinRoot.GetChild(i).gameObject);
        }

    }
}
