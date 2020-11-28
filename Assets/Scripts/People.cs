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
    public float cdTimer = 0f;
    public bool isCd = false;
    public int talkid = 0;
    public float agree = 0.5f;
    Vector3 target;
    People otherPeo;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public void InitPeople(int id)
    {
        pid=id;
        //anim = transform.GetChild(0).GetComponent<Animator>();
        anim = GetComponentInChildren<Animator>();
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

    public void SetAnimator(Vector3 newTarger,bool talkStateAni=false) {
        if (talkStateAni) {
            anim.SetInteger("state", 3);
            return;
        }
        Vector3 tmp = newTarger - transform.position;
        Vector3 dir = new Vector3(tmp.x,0,tmp.z);
        dir.Normalize();
        tmp=Quaternion.FromToRotation(Vector3.forward, dir).eulerAngles;
        //Debug.Log(dir.x+" "+dir.z+"    "+ tmp);
        //if (dir.x <= 0.5 && dir.x >= -0.5 && dir.z >= 0)   //up
        //{
        //    anim.SetInteger("state", 1);
        //}
        //else if (dir.x < 0.5 && dir.x > -0.5 && dir.z < 0)   //down
        //{
        //    anim.SetInteger("state", 0);
        //}
        //else if (dir.z <= 0.5 && dir.z >= -0.5 && dir.x >= 0)   //right
        //{
        //    spriteRenderer.flipX = true;
        //    anim.SetInteger("state", 2);
        //    anim.SetBool("s", true);
        //}
        //else if (dir.z < 0.5 && dir.z > -0.5 && dir.x < 0)   //left
        //{
        //    spriteRenderer.flipX = false;
        //    anim.SetInteger("state", 2);
        //    anim.SetBool("s", false);
        //}
        //else { Debug.Log("??"); }
        if (tmp.y <= 30 || tmp.y >= 330) {
            anim.SetInteger("state", 1);
        } else if (tmp.y>30&& tmp.y<150) {
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
        float tickadd = otherAgree > 0 ? 1.0f : 0.9f;
        agree += (1 - stubborn) * otherPeo.conveyStr * othera * tickadd;
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
        else {
            spriteRenderer.flipX = false;
            otherPeo.spriteRenderer.flipX = true;
        }

        SetAnimator( Vector3.one, true);
        otherPeo.SetAnimator(Vector3.one, true);
        yield return new WaitForSeconds(0.2f);
        //支持率数值计算
        float tmpAgree = agree;
        ChangeSelf(otherPeo.agree);
        otherPeo.ChangeSelf(tmpAgree);
        
        yield return new WaitForSeconds(2f);
        //Debug.Log(pid + "  " + otherPeo.pid + "  结束交流");
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
        SetAnimator(target,false);
    }
}
