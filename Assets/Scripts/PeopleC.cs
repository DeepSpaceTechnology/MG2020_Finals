using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//固执
public class PeopleC : People
{
    public new void InitPeople(int id)
    {
        base.InitPeople(id);
        conveyWant = 0.4f;
        conveyStr = Random.Range(0.4f, 0.5f);
        stubborn = Random.Range(0.6f, 0.7f);
    }
}
