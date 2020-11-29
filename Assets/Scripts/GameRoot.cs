using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot instance;
    public int money = 100;
    public int[] skillPrice;
    public string[][] talksArr;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        talksArr= new string[4][];
        for (int i = 0; i < 4; i++)
        {
            talksArr[i] = new string[4];
        }
        talksArr[0][0] = "行，听你的。";
        talksArr[0][1] = "老板大气！";
        talksArr[0][2] = "我也觉得红色好看。";
        talksArr[0][3] = "红色确实好看啊。";

        talksArr[1][0] = "一会大家就都知道了！";
        talksArr[1][1] = "那我去和别人说说。";
        talksArr[1][2] = "大家来看看我的红衣服！";
        talksArr[1][3] = "听说现在红色最流行噢！";

        talksArr[2][0] = "我永远喜欢红色！";
        talksArr[2][1] = "我是红色死忠粉！";
        talksArr[2][2] = "红色衣服才好看，不接受反驳。";
        talksArr[2][3] = "守护世界上最好的红色！";

        talksArr[3][0] = "这你可找对人了！";
        talksArr[3][1] = "今日演讲《为什么红色最好看》";
        talksArr[3][2] = "还没有我说服不了的人。";
        talksArr[3][3] = "我就是意见领袖！";
    }

    public string GetRamTalkByType(int i) {
        return talksArr[i][Random.Range(0,4)];
    }
}
