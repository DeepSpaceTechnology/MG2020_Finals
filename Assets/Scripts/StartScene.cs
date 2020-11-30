using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartScene : MonoBehaviour
{
    public GameObject v;
    private void Start()
    {
        v.GetComponent<VideoPlayer>().loopPointReached += EndVideo;
        AudioManager.Instance.PlayAudio("bgm",true,0.12f);
    }
    public void ClickEnter()
    {
        v.SetActive(true);
        AudioManager.Instance.PlayAudio("btn");
    }
    public void ClickQuit()
    {
        Application.Quit();
    }
    [ContextMenu("end")]
    public void EndVideo(VideoPlayer video)
    {
        //在视频结束时会调用这个函数
        Debug.Log("视频播放结束");
        SceneManager.LoadScene("Cloud");
    }

}
