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
    public CloudsGenerator Cloudobj;
    public float BaseMoneyCd = 1f;//基础金钱cd
    public float Basetimer = 0;
    public float PeoMoneyCd = 5f;//人数赚钱cd
    public float Peotimer = 0;
    public float NewsCd = 5f;//newcd
    public float Newstimer = 0;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CreateAll();
        Time.timeScale = 1;
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
            if (Cloudobj != null)
            {
                //go.GetComponentInChildren<ShadowOutShape>().generator = Cloudobj;
                //Cloudobj._points.Add(go.GetComponent<Point>());
            }
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

            float agree = Random.Range(-0.5f, 0.1f);
            float agreesum = 0;
            //if (totalAgree >= 0)
            //{
            //    agree = Random.Range(-0.1f, 0f);
            //}
            //else if (totalAgree < 0)
            //{
            //    agree = Random.Range(0f, 0.1f);
            //}
            agreesum += agree;
            totalAgree = agreesum / totalNum;
            go.GetComponent<People>().agree = agree-0.03f;
            go.GetComponent<People>().ChangeColor();
            pList.Add(go.GetComponent<People>());
            go.GetComponent<People>().cdTimer = Random.Range(2f,5f);
            go.name += "-" + curNum;
        }
    }
    //根据人物编号计算位置
    public Vector3 CalPosByid(int id)
    {
        float x = (id - 1) % countx * (mapx / countx) + (mapx / countx) / 2;
        float z = (int)((id - 1) / countx) * (mapz / countz) + (mapz / countz) / 2;
        Vector3 offset = new Vector3(Random.Range(-0.8f, 0.8f), 0, Random.Range(-0.8f, 0.8f));
        Vector3 res = new Vector3(x, peoObj[0].transform.position.y, z) + offset-new Vector3(7,0,4);
        return res;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (Time.timeScale ==1)
            {
                Time.timeScale = 5;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            UIMgr.instance.leftTip.ShowText("我是boss，我是b我是boss，0");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            UIMgr.instance.newsRoll.AddNews("我是boss，我是b我是boss新闻");
        }

        //统计各种信息
        float agreesum = 0;
        int agsum = 0;
        int dissum = 0;
        int m = 0;//本次人数赚取
        foreach (People p in pList)
        {
            agreesum += p.agree;
            if (p.agree > 0)
            {
                agsum++;
                if (p.hasShadow)
                {
                    m += 2;
                }
                m += 1;
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

        //赚钱
        if (Basetimer < BaseMoneyCd)
        {
            Basetimer += Time.deltaTime;
        }
        else
        {
            Basetimer = 0;
            GameRoot.instance.money += 2;
            UIMgr.instance.showMoney.UpdateMoney();
        }
        if (Peotimer < PeoMoneyCd)
        {
            Peotimer += Time.deltaTime;
        }
        else
        {
            Peotimer = 0;
            GameRoot.instance.money += Mathf.Clamp(m,0,25);
            List<Vector3> clist = new List<Vector3>();
            foreach (People p in pList)
            {
                if (p.agree > 0)
                {
                    if (p.hasShadow)
                    {
                        clist.Add(Camera.main.WorldToScreenPoint(p.transform.position + new Vector3(-0.07f, 0.7f, 0)));
                        clist.Add(Camera.main.WorldToScreenPoint(p.transform.position + new Vector3(0.07f, 0.7f, 0)));
                    }
                    else
                    {
                        clist.Add(Camera.main.WorldToScreenPoint(p.transform.position + new Vector3(0, 0.7f, 0)));
                    }
                    
                }
            }
            UIMgr.instance.ShowCoin(clist);
            UIMgr.instance.showMoney.UpdateMoney();
        }

        //新闻
        if (Newstimer < NewsCd)
        {
            Newstimer += Time.deltaTime;
        }
        else
        {
            Newstimer = 0;
            if(agL>=0 && agL < 0.25)
            {
                UIMgr.instance.newsRoll.AddNews(GameRoot.instance.newsArr[Random.Range(0,17)]);
            }
            else if (agL >= 0.25 && agL < 0.5)
            {
                UIMgr.instance.newsRoll.AddNews(GameRoot.instance.newsArr[Random.Range(17, 34)]);
            }
            else if (agL >= 0.5 && agL < 0.75)
            {
                UIMgr.instance.newsRoll.AddNews(GameRoot.instance.newsArr[Random.Range(34, 51)]);
            }
            else if (agL >= 0.75 && agL< 1)
            {
                UIMgr.instance.newsRoll.AddNews(GameRoot.instance.newsArr[Random.Range(51, 68)]);
            }
}
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
