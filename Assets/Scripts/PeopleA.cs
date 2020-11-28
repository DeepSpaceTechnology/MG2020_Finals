using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//普通人
public class PeopleA : People
{
    public new void InitPeople(int id)
    {
        base.InitPeople(id);
        conveyWant = Random.Range(0.4f, 0.6f);
        conveyStr= Random.Range(0.4f, 0.6f);
        stubborn= Random.Range(0.4f, 0.5f);
    }
}
