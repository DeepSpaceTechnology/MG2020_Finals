using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDetail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject detail;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIMgr.instance.uiState == 0)
        {
            detail.SetActive(true);
        }
        else {
            detail.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
          detail.SetActive(false);
    }
}
