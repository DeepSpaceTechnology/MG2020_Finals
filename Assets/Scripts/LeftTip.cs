using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftTip : MonoBehaviour
{
    public Text talkText;
    public Animator ani;

    public void ShowText(string str) {
        talkText.text = str;
        ani.Play("LeftTip",0,0);
    }
}
