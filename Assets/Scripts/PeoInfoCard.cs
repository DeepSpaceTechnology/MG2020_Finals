using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeoInfoCard : MonoBehaviour
{
    public Text t_pname;
    public Text t_detail;
    public Text t_viewpoint;
    public Text t_percent;

    public Color colAgree;
    public Color colDisAgree;

    //数值滚动变化
    public int curAgree = 0;
    public int result;
    public int start = 0;
    public int end = 0;
    public int jump = 6;
    public bool isJumping = false;
    public bool isAgree = true;


    public void SetInfo(int index,float _agree) {
        StopCoroutine(JumpNumber());
        isJumping = false;

        switch (index)
        {
            case 0:
                t_pname.text = "普通人";
                t_detail.text = "平平无奇的普通人";
                break;
            case 1:
                t_pname.text = "八卦佬";
                t_detail.text = "热衷闲聊，人云亦云";
                break;
            case 2:
                t_pname.text = "老顽固";
                t_detail.text = "不善言辞，固执己见";
                break;
            case 3:
                t_pname.text = "演说家";
                t_detail.text = "能说会道，好为人师";
                break;
            default:
                break;
        }

        if (_agree >= 0)
        {
            t_viewpoint.text = "支持";
            t_viewpoint.color = colAgree;
            t_percent.color = colAgree;
            isAgree = true;
        }
        else {
            t_viewpoint.text = "反对";
            t_viewpoint.color = colDisAgree;
            t_percent.color = colDisAgree;
            _agree = -_agree;
            isAgree = false;
        }

        int num=(int)(_agree * 100);
        t_percent.text = num.ToString();
        curAgree = num;
    }

    public void UpdateAgree(float _agree)
    {
        
        bool preIsAgree = isAgree;
        Debug.Log("UpdateAgree "+_agree);
        if (_agree >= 0)
        {
            isAgree = true;
        }
        else
        {
            _agree = -_agree;
            isAgree = false;
        }
        int newAgree = (int)(_agree * 100);

        if (!gameObject.activeSelf) {       //若卡片没显示
            if (_agree >= 0)
            {
                t_viewpoint.text = "支持";
                t_viewpoint.color = colAgree;
                t_percent.color = colAgree;
                isAgree = true;
            }
            else
            {
                t_viewpoint.text = "反对";
                t_viewpoint.color = colDisAgree;
                t_percent.color = colDisAgree;
                _agree = -_agree;
                isAgree = false;
            }

            int num = (int)(_agree * 100);
            t_percent.text = num.ToString();
            curAgree = num;
            return;
        }

        if (preIsAgree == isAgree)
        {
            if (!isJumping)
            {
                start = curAgree;
                //Debug.Log("start " + start);
                curAgree = newAgree;
                end = newAgree;
                StartCoroutine(JumpNumber());
            }
            else
            {
                StopCoroutine(JumpNumber());
                start = curAgree;
                curAgree = newAgree;
                end = newAgree;
                StartCoroutine(JumpNumber());
            }
        }
        else {
            if (isAgree)
            {
                t_viewpoint.text = "支持";
                t_viewpoint.color = colAgree;
                t_percent.color = colAgree;
            }
            else {
                t_viewpoint.text = "反对";
                t_viewpoint.color = colDisAgree;
                t_percent.color = colDisAgree;
            }

            if (!isJumping)
            {
                start = 0;
                curAgree = newAgree;
                end = newAgree;
                StartCoroutine(JumpNumber());
            }
            else
            {
                StopCoroutine(JumpNumber());
                start = 0;
                curAgree = newAgree;
                end = newAgree;
                StartCoroutine(JumpNumber());
            }
        }
    }

    IEnumerator JumpNumber() {
        bool isAdd = true;
        if (end < start) { isAdd = false; }
        isJumping = true;
        int delta = (end - start) / jump;
        if (delta == 0 && ((end - start) > 0)) {
            delta = 1;
        } else if (delta == 0 && ((end - start) < 0)) {
            delta = -1;
        }
        
        result = start;
        for (int i = 0; i < jump; i++)
        {
            //Debug.Log(result);
            result += delta;
            if (isAdd&&result > end) {
                break;
            }
            if (!isAdd && result < end)
            {
                break;
            }
            t_percent.text = result.ToString();
            yield return new WaitForSeconds(0.05f);
        }
        result = end;
        t_percent.text = result.ToString();
        isJumping = false;
        StopCoroutine(JumpNumber());
    }
}
