using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//闲聊者
public class PeopleB : People
{
    public new void InitPeople(int id)
    {
        base.InitPeople(id);
        conveyWant = Random.Range(0.6f, 0.8f);
        conveyStr = Random.Range(0.4f, 0.5f);
        stubborn = Random.Range(0.3f, 0.4f);
        money = Random.Range(40, 70);
    }
}
