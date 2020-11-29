using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public RectTransform totalPtr;//平均支持指针
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateTotalPtr();
    }
    void updateTotalPtr()
    {
        float tmp = (PeopleManager.instance.agCount - PeopleManager.instance.disCount) / (float)(PeopleManager.instance.totalNum);
        float targetx = 620 * tmp;
        totalPtr.localPosition = new Vector3(Mathf.Lerp(totalPtr.localPosition.x,targetx,Time.deltaTime*5f), 
            totalPtr.localPosition.y, totalPtr.localPosition.z);
    }
}
