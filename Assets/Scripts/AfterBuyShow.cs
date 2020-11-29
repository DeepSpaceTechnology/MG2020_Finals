using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterBuyShow : MonoBehaviour
{
    public Text t_talk;
    public float time = 3f;
    public Transform parent_peo;
    private Vector3 offset;

    private void Start()
    {
        StartCoroutine(Die());
    }

    public void SetTalk(string str,Transform parent,Vector3 _offset)
    {
        t_talk.text = str;
        parent_peo = parent;
        offset = _offset;
    }

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(parent_peo.transform.position + offset);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
