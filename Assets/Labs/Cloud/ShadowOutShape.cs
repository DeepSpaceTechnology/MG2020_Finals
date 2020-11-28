using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowOutShape : MonoBehaviour
{
    public CloudsGenerator generator;
    public Point rootPoint;

    public float magicNumber;

    private Vector3 _oriScale;
    private float _oriShadowScale;
    private void Start()
    {
        _oriScale = this.transform.localScale;
        _oriShadowScale = this.rootPoint.radius;
    }

    private void Update()
    {
        this.transform.localScale=new Vector3((rootPoint.radius - _oriShadowScale)/magicNumber+_oriScale.x,
                                                    this.transform.localScale.y,
                                                (rootPoint.radius - _oriShadowScale)/magicNumber+_oriScale.z);
    }
}
