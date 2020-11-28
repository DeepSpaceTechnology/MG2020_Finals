using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : MonoBehaviour
{
    public int pid = 1;
    public int type=0;//0 1 2 3 四种人
    public float speed=3f;
    public float conveyStr = 0.5f;//传播强度
    public float conveyWant = 0.5f;//传播欲望
    public float stubborn = 0.3f;//固执度
    public bool isTalking=false;
    public bool mainTalker = false;
    public float talkcd = 5f;
    private float cdTimer = 0f;
    public bool isCd = false;
    public int talkid = 0;
    public float agree = 0.5f;
    Animator anim;
    Vector3 target;
    People otherPeo;
    public void InitPeople(int id)
    {
        pid=id;
        anim = GetComponent<Animator>();
        //Vector2 offset = new Vector2(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f));
    }
    public void Move()
    {
        //没到达目标，继续走
        if(target != Vector3.zero && Vector3.Distance(transform.position,target)>0.2f)
        {
            transform.Translate((target - transform.position).normalized * speed * Time.deltaTime);
            return;
        }

        //到达目标，设置新目标点
        if (target == Vector3.zero || Vector3.Distance(transform.position, target) < 0.2f )
        {
            target = new Vector3(Random.Range(0.5f, PeopleManager.instance.mapx-0.5f), transform.position.y, 
                Random.Range(0.5f, PeopleManager.instance.mapz - 0.5f));
        }
    }
    private void Update()
    {
        if (pid == 0) return;//还未初始化，不作任何操作
        if (isTalking)
        {

        }
        else{
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

    public void ChangeSelf(float otherAgree)
    {
        if (otherPeo == null) return;
        agree += (1 - stubborn) * otherPeo.conveyStr * otherAgree;
        if (agree > 1)
        {
            agree = 1;
        }else if (agree < -1)
        {
            agree = -1;
        }

    }
    public void UpdateColor()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (isCd) { return; }

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
            Debug.Log(pid + " can not speak");
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
        Debug.Log(pid + " speak");
        if (mainTalker)
        {
            bool happenTalk = IsHappenTalk();
            Debug.Log(pid + "  " + otherPeo.pid + "  =" + happenTalk);
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
        Debug.Log(pid + "  " + otherPeo.pid + "  开始交流");
        //TODO 让双方面对面
        yield return new WaitForSeconds(0.2f);
        //支持率数值计算
        float tmpAgree = agree;
        ChangeSelf(otherPeo.agree);
        otherPeo.ChangeSelf(tmpAgree);
        
        yield return new WaitForSeconds(2f);
        Debug.Log(pid + "  " + otherPeo.pid + "  结束交流");
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
    }
}
