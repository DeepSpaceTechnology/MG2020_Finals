using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedMoney : MonoBehaviour
{
    public Text t_num;
    public int money;

    public void SetMoney(int m) {
        money = m;
        t_num.text = m.ToString();
    }
}
