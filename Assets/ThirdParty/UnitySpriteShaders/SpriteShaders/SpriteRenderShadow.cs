using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderShadow : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Renderer>().receiveShadows = true;
    }
    
}
