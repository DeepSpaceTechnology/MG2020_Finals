using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseChange : MonoBehaviour
{
    public Texture[] textures;  //普通01  收买23  信息轰炸4  律师函56  狂热7
    public Texture scope;
    
    public int index=0;
    public bool isScope = false;

    void Start()
    {
        Cursor.visible = false;
    }
    void OnGUI()
    {
        Vector3 vector3 = Input.mousePosition;
        if (isScope)
        {
            GUI.DrawTexture(new Rect(vector3.x - scope.width / 2, (Screen.height - vector3.y) - scope.height / 2, scope.width, scope.height), scope);
        }
        GUI.DrawTexture(new Rect(vector3.x - textures[index].width / 2, (Screen.height - vector3.y) - textures[index].height / 2, textures[index].width, textures[index].height), textures[index]);
    }
}