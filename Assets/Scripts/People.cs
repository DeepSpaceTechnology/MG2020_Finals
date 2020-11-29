using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : MonoBehaviour
{
    public int pid = 1;
    public int type = 0;//0 1 2 3 四种人
    public float speed = 3f;
    public float conveyStr = 0.5f;//传播强度
    public float conveyWant = 0.5f;//传播欲望
    public float stubborn = 0.3f;//固执度
    public bool isTalking = false;
    public bool mainTalker = false;
    public float talkcd = 5f;
    public float cdTimer = 0f;
    public bool isCd = false;
    public int talkid = 0;
    public float agree = 0.5f;
    public float oldagree = 0f;
    public bool hasBuy = false;//被收买
    public bool hasShadow = false;
    public int money = 10;
    Vector3 target;
    People otherPeo;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public GameObject Arrow_up;//上升箭头
    public Color blue = new Color(0, 0.73f, 0.9f);
    public Color red = new Color(0.9f, 0, 0);
    Queue<int> arrque = new Queue<int>();//多处显示箭头时，每次出现加入队列，结束时退出队列，箭头消失时检查队列是否为空
    Material clothMat;
    public void InitPeople(int id)
    {
        pid = id;
        anim = GetComponentInChildren<Animator>();
        clothMat =new Material(transform.GetChild(0).GetComponent<SpriteRenderer>().material);
        transform.GetChild(0).GetComponent<SpriteRenderer>().material = clothMat;

    }
    public void Move()
    {
        //没到达目标，继续走
        if (target != Vector3.zero && Vector3.Distance(transform.position, target) > 0.2f)
        {
            transform.Translate((target - transform.position).normalized * speed * Time.deltaTime);
            return;
        }

        //到达目标，设置新目标点
        if (target == Vector3.zero || Vector3.Distance(transform.position, target) < 0.2f)
        {
            target = new Vector3(Random.Range(0.5f, PeopleManager.instance.mapx - 0.5f)-7, transform.position.y,
                Random.Range(0.5f, PeopleManager.instance.mapz - 0.5f)-4);
            //Debug.Log("到目标点了");
            SetAnimator(target);
        }
    }
    private void Update()
    {
        if (pid == 0) return;//还未初始化，不作任何操作
        if (isTalking)
        {

        }
        else
        {
            Move();
        }

        if (!isTalking && isCd)
        {
            cdTimer -= Time.deltaTime;
            if (cdTimer <= 0)
            {
                isCd = false;
                cdTimer = talkcd;
            }
        }
    }

    public void SetAnimator(Vector3 newTarger, bool talkStateAni = false)
    {
        if (talkStateAni)
        {
            anim.SetInteger("state", 3);
            return;
        }
        Vector3 tmp = newTarger - transform.position;
        Vector3 dir = new Vector3(tmp.x, 0, tmp.z);
        dir.Normalize();
        tmp = Quaternion.FromToRotation(Vector3.forward, dir).eulerAngles;

        if (tmp.y <= 30 || tmp.y >= 330)
        {
            anim.SetInteger("state", 1);
        }
        else if (tmp.y > 30 && tmp.y < 150)
        {
            spriteRenderer.flipX = true;
            anim.SetInteger("state", 2);
        }
        else if (tmp.y >= 150 && tmp.y < 210)
        {
            anim.SetInteger("state", 0);
        }
        else if (tmp.y >= 210 && tmp.y < 330)
        {
            spriteRenderer.flipX = false;
            anim.SetInteger("state", 2);
        }
    }

    public void ChangeSelf(float otherAgree)
    {
        if (otherPeo == null) return;
        int othera = otherAgree > 0 ? 1 : -1;
        
        float tickadd=1f;//防止同化过于严重
        if(othera>0 && agree>0 && PeopleManager.instance.agL > 0.55f)
        {
            tickadd = 0.2f;
        }
        if (othera < 0 && agree < 0 && PeopleManager.instance.agL < 0.45f)
        {
            tickadd = 0.2f;
        }
        float tickadd2 = 1f;//防止过高值出现
        if (agree>0.5 && otherAgree > 0)
        {
            tickadd2 = 0.5f;
        }
        if (agree < -0.5 && otherAgree < 0)
        {
            tickadd2 = 0.5f;
        }
        if (agree > 0.7 && otherAgree > 0)
        {
            tickadd2 = 0.2f;
        }
        if (agree < -0.7 && otherAgree < 0)
        {
            tickadd2 = 0.2f;
        }
        oldagree = agree;
        ShowArrow(otherAgree > 0);
        agree += (1 - stubborn) * otherPeo.conveyStr * othera * tickadd* tickadd2;
        ChangeColor();
        if (agree > 1f)
        {
            agree = 1f;
        }
        else if (agree < -1f)
        {
            agree = -1f;
        } 
        //产生阴影
        if (otherPeo.hasShadow && otherAgree > 0 && agree > 0 && hasShadow==false)
        {
            ShowShadow();
        }
        //解除阴影
        if(hasShadow && agree < 0)
        {
            ShowShadow(true);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("people") || isCd) { return; }

        People tmp = other.gameObject.GetComponent<People>();

        if (!isTalking && !tmp.isTalking && !tmp.isCd)    //双方都不在交谈状态，可以交谈
        {
            mainTalker = true;
            TalkReady(tmp);
        }
        else if (isTalking && talkid != 0)   // 在交谈，已有交谈对象  ，下面再判断交谈对象是不是 碰撞者
        {
            if (talkid == tmp.pid)
            {
                mainTalker = false;
                TalkReady(tmp);
            }
        }
        else
        {
            //Debug.Log(pid + " can not speak");
            otherPeo = null;
        }
    }

    private void TalkReady(People trig)
    {
        otherPeo = trig;
        isTalking = true;
        otherPeo.isTalking = true;
        talkid = otherPeo.pid;
        otherPeo.talkid = pid;
        //Debug.Log(pid + " speak");
        if (mainTalker)
        {
            bool happenTalk = IsHappenTalk();
            //Debug.Log(pid + "  " + otherPeo.pid + "  =" + happenTalk);
            if (happenTalk)
            {
                StartCoroutine(IETalk());
            }
            else
            {
                isTalking = false;
                otherPeo.isTalking = false;
            }
        }
    }

    public bool IsHappenTalk()      //根据双方交流意愿 计算是否发生 对话
    {
        float talkScpoe = conveyWant * otherPeo.conveyWant;
        float r = Random.Range(0f, 1f);
        if (talkScpoe >= r)
        {
            return true;
        }
        return false;
    }



    IEnumerator IETalk()
    {
        //Debug.Log(pid + "  " + otherPeo.pid + "  开始交流");

        //TODO 让双方面对面
        if (transform.position.x - otherPeo.transform.position.x < 0)
        {
            spriteRenderer.flipX = true;
            otherPeo.spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = false;
            otherPeo.spriteRenderer.flipX = true;
        }
        SetAnimator(Vector3.one, true);
        otherPeo.SetAnimator(Vector3.one, true);
        yield return new WaitForSeconds(0.1f);
        float tmpAgree = agree;
        ChangeSelf(otherPeo.agree);
        otherPeo.ChangeSelf(tmpAgree);

        yield return new WaitForSeconds(2f);
        //支持率数值计算

        //Debug.Log(pid + "  " + otherPeo.pid + "  结束交流");
        UIMgr.instance.OverTalkUpdate(pid, otherPeo.pid);
        otherPeo.OverTalk();
        OverTalk();
    }

    public void OverTalk()
    {
        isTalking = false;
        mainTalker = false;
        talkid = 0;
        otherPeo = null;
        isCd = true;
        cdTimer = talkcd;
        SetAnimator(target, false);
    }


    public void ShowArrow(bool toRed, bool forceUp = false)//参数，是否被说服要偏向红色
    {
        if (forceUp)
        {
            StartCoroutine(SetArrow(red, true));//强制红色上升
            return;
        }
        if (toRed)
        {
            
            if (agree > 0)
            {
                StartCoroutine(SetArrow(red, true));//2红
            }
            else
            {
                StartCoroutine(SetArrow(blue, false));//蓝-红
            }
        }
        else
        {
            if (agree > 0)
            {
                StartCoroutine(SetArrow(red, false));//红-蓝
            }
            else
            {
                StartCoroutine(SetArrow(blue, true));//2蓝
            }
        }

    }
    IEnumerator SetArrow(Color c, bool isup)
    {
        arrque.Enqueue(1);
        Arrow_up.GetComponent<SpriteRenderer>().color = c;
        if (isup)
        {
            Arrow_up.transform.localRotation = Quaternion.Euler(45, 0, 0);
        }
        else
        {
            Arrow_up.transform.localRotation = Quaternion.Euler(45, 0, 180);
        }
        Arrow_up.SetActive(true);
        yield return new WaitForSeconds(2f);
        arrque.Dequeue();
        if (arrque.Count == 0)
        {
            Arrow_up.SetActive(false);
        }
    }

    [ContextMenu("buy")]
    public void BuyPeople()//被收买
    {
        hasBuy = true;
        if (agree < 0.5)
        {
            agree = Random.Range(0.5f, 0.7f);
        }
        else
        {
            agree += Random.Range(0.1f, 0.2f);
        }
        if (stubborn < 0.5f)
        {
            stubborn = Random.Range(0.8f, 0.9f);
        }
        if (agree > 1) agree = 1;
        ChangeColor();
        UIMgr.instance.OverTalkUpdate(pid, pid);
        ShowArrow(true,true);
        ShowShadow();
    }

    public void InfoBomb()//信息炸弹
    {
        agree += Random.Range(0.1f, 0.2f);

        if (agree > 1) agree = 1;
        ChangeColor();
        UIMgr.instance.OverTalkUpdate(pid, pid);
        ShowArrow(true, true);
    }

    public void Lawyerletter()//律师函
    {
        if (agree < 0)
        {
            agree = -Random.Range(0.01f, 0.08f);
            stubborn = Random.Range(0.1f, 0.2f);
            conveyStr = Random.Range(0.1f, 0.2f);
        }
        else
        {
            agree += Random.Range(0.1f, 0.2f);
            stubborn = Random.Range(0.1f, 0.2f);
        }
        StartCoroutine(DelSpeed());
        ShowArrow(true);
        if (agree > 1) agree = 1;
        ChangeColor();
        ChangeColor();
        UIMgr.instance.OverTalkUpdate(pid, pid);
    }

    public void FansClub()
    {
        if (agree > 0)
        {
            agree += Random.Range(0.05f, 0.1f);
            if (stubborn < 0.5f)
            {
                stubborn = Random.Range(0.5f, 0.6f);
            }
            conveyStr += Random.Range(0.1f, 0.2f);
            conveyWant += Random.Range(0.1f, 0.3f);
            talkcd = 3f;
        }
        StartCoroutine(AddSpeed());
        if (agree > 1) agree = 1;
        ChangeColor();
        if (conveyStr > 1) conveyStr = 1;
        if (conveyWant > 1) conveyWant = 1;
        ShowArrow(true, true);
        UIMgr.instance.OverTalkUpdate(pid, pid);
    }


    public void ChangeColor()
    {
        if (agree > 0)
        {
            clothMat.SetFloat("_Hue", 0.42f);
            clothMat.SetFloat("_Saturation", Mathf.Clamp(1.875f*agree,0,2f));           
        }
        else
        {
            clothMat.SetFloat("_Hue", 0f);
            clothMat.SetFloat("_Saturation", Mathf.Clamp(1.875f * -agree, 0, 2f));
        }
    }
    public void ShowShadow(bool hide = false)
    {
        if (!hide)
        {
            GetComponentInChildren<Point>().radius = -5;
            PeopleManager.instance.Cloudobj._points.Add(GetComponent<Point>());
            StartCoroutine(FadeInShadow());
            hasShadow = true;
        }
        else
        {
            StartCoroutine(FadeOutShadow());
            hasShadow = false;
        }
    }

    IEnumerator FadeInShadow()
    {
        float org = GetComponentInChildren<Point>().radius;
        for (int i = 1; i <= 10; i++)
        {
            GetComponentInChildren<Point>().radius=Mathf.Lerp(org,1,i/10f);
            yield return new WaitForSeconds(0.15f);
        }
        GetComponentInChildren<Point>().radius = 1f;
    }

    IEnumerator FadeOutShadow()
    {
        float org = GetComponentInChildren<Point>().radius;
        for (int i = 1; i <= 10; i++)
        {
            GetComponentInChildren<Point>().radius = Mathf.Lerp(org, -5, i / 10f);
            yield return new WaitForSeconds(0.1f);
        }
        GetComponentInChildren<Point>().radius = -5f;
        PeopleManager.instance.Cloudobj._points.Remove(GetComponent<Point>());
    }


    IEnumerator AddSpeed()
    {
        float org = speed;
        speed = 1.4f;
        yield return new WaitForSeconds(3f);
        speed = org;
    }

    IEnumerator DelSpeed()
    {
        float org = speed;
        speed = 0.3f;
        yield return new WaitForSeconds(3f);
        speed = org;
    }

}
