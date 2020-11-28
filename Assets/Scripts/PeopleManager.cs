using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    public static PeopleManager instance;
    public int totalNum = 20;
    public int Anum = 0;
    public int Bnum = 0;
    public int Cnum = 0;
    public int Dnum = 0;
    public List<People> pList;
    public GameObject[] peoObj;//4种人的prefab
    public int countx = 7;
    public int countz = 4;
    public float mapx = 14;
    public float mapz = 8;
    public float totalAgree;
    public int agCount = 0;
    public int disCount = 0;
    public float agL = 0f;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CreateAll();
    }
    public void CreateAll()
    {
        totalNum = Anum + Bnum + Cnum + Dnum;
        List<int> ilist = new List<int>();
        pList = new List<People>();
        for (int i = 0; i < Anum; i++)
        {
            ilist.Add(0);
        }
        for (int i = 0; i < Bnum; i++)
        {
            ilist.Add(1);
        }
        for (int i = 0; i < Cnum; i++)
        {
            ilist.Add(2);
        }
        for (int i = 0; i < Dnum; i++)
        {
            ilist.Add(3);
        }
        Queue<int> ique = new Queue<int>(Utilities.ShuffleArray(ilist.ToArray(), Random.Range(1000, 9999)));
        int curNum = 0;//当前人数,作为id,id从1开始
        for (int i = 0; i < totalNum; i++)
        {
            int type = ique.Dequeue();
            GameObject go = Instantiate(peoObj[type], CalPosByid(++curNum), Quaternion.identity);
            go.GetComponent<People>().type = type;
            switch (type)
            {
                case 0:
                    go.GetComponent<PeopleA>().InitPeople(curNum);
                    break;
                case 1:
                    go.GetComponent<PeopleB>().InitPeople(curNum);
                    break;
                case 2:
                    go.GetComponent<PeopleC>().InitPeople(curNum);
                    break;
                case 3:
                    go.GetComponent<PeopleD>().InitPeople(curNum);
                    break;
            }

            float agree = 0.1f;
            float agreesum = 0;
            if (totalAgree > 0)
            {
                agree = Random.Range(-0.2f, 0f);
            }
            else if (totalAgree < 0)
            {
                agree = Random.Range(0f, 0.2f);
            }
            agreesum += agree;
            totalAgree = agreesum / totalNum;
            go.GetComponent<People>().agree = agree;
            pList.Add(go.GetComponent<People>());
            go.GetComponent<People>().cdTimer = Random.Range(2f,5f);
        }
    }
    //根据人物编号计算位置
    public Vector3 CalPosByid(int id)
    {
        float x = (id - 1) % countx * (mapx / countx) + (mapx / countx) / 2;
        float z = (int)((id - 1) / countx) * (mapz / countz) + (mapz / countz) / 2;
        Vector3 offset = new Vector3(Random.Range(-0.8f, 0.8f), 0, Random.Range(-0.8f, 0.8f));
        Vector3 res = new Vector3(x, peoObj[0].transform.position.y, z) + offset;
        return res;
    }
    private void Update()
    {
        float agreesum = 0;
        int agsum = 0;
        int dissum = 0;
        foreach (People p in pList)
        {
            agreesum += p.agree;
            if (p.agree > 0)
            {
                agsum++;
            }
            else
            {
                dissum++;
            }
        }
        totalAgree = agreesum / totalNum;
        agCount = agsum;
        disCount = dissum;
        agL = (float)(agCount) / totalNum;
    }
}
public class Utilities
{
    public static T[] ShuffleArray<T>(T[] _dataArray, int _seed)
    {
        System.Random prng = new System.Random(_seed);

        for (int i = 0; i < _dataArray.Length - 1; i++)
        {
            int randomIndex = prng.Next(i, _dataArray.Length);

            T temp = _dataArray[randomIndex];
            _dataArray[randomIndex] = _dataArray[i];
            _dataArray[i] = temp;
        }

        return _dataArray;
    }
}
