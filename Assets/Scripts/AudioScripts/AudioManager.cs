using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //音频资源放在Resources下
    //配置文件放在StrramingAssets下


    public static AudioManager Instance;
    SourceManager sourceManager;
    ClipManager clipManager;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
        sourceManager = new SourceManager(gameObject);
        clipManager = new ClipManager();
        DontDestroyOnLoad(gameObject);
}
    //对播放音频外接口
    public void PlayAudio(string audioName,bool loop=false,float str=1f)
    {
        //拿到一个空闲的audioSource
        AudioSource tmpSource = sourceManager.GetFreeAudioSource(loop,str);
        //找到clip
        SingleClip tmpClip = clipManager.FindClipByNane(audioName);
        if (tmpClip != null)
        {
            //把上面2个结合

            tmpClip.Play(tmpSource);

        }
    }
    //对外停止播放视频接口
    public void StopAudio(string audioName)
    {
        sourceManager.Stop(audioName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

