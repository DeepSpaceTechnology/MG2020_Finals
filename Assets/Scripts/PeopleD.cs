using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//健谈
public class PeopleD : People
{
    public new void InitPeople(int id)
    {
        base.InitPeople(id);
        conveyWant = Random.Range(0.8f, 0.9f);
        conveyStr = Random.Range(0.8f, 0.9f);
        stubborn = Random.Range(0.7f, 0.8f);
    }
}
