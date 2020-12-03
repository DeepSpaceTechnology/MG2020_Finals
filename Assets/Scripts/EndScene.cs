using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndScene : MonoBehaviour
{
    public GameObject v;
    public GameObject g;
    private void Start()
    {
        Cursor.visible = true;
        v.GetComponent<VideoPlayer>().loopPointReached += EndVideo;
    }
    [ContextMenu("end")]
    public void EndVideo(VideoPlayer video)
    {
        //在视频结束时会调用这个函数
        Debug.Log("视频播放结束");
        g.SetActive(true);
        v.SetActive(false);
    }
    public void ClickRe()
    {
        SceneManager.LoadScene("Start");
    }
    public void ClickEnd()
    {
        Application.Quit();
    }
}
