﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMoney : MonoBehaviour
{
    public Text t_num;

    public void UpdateMoney()
    {
        t_num.text = GameRoot.instance.money.ToString();
    }
}