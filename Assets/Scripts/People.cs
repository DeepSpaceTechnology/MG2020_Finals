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
    public float talkcd=5f;
    public bool isTalking=false;
    public float agree = 0.5f;
    Animator anim;
    Vector3 target;
    People otherPeo;
    public void InitPeople(int id)
    {
        pid = id;
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
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    public void ChangeSelf()
    {
        if (otherPeo == null) return;
        agree += (1 - stubborn) * otherPeo.conveyStr * otherPeo.agree;

    }
    public void UpdateColor()
    {

    }
}
