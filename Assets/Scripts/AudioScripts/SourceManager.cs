using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceManager
{
    //构造函数
    public SourceManager(GameObject tmpOwer)
    {
        ower = tmpOwer;
        Initial();
    }
    List<AudioSource> allSources;
    GameObject ower;//用于挂载多个(3)audioSource
    //初始化
    public void Initial()
    {
        allSources = new List<AudioSource>();
        //循环3次  3为ower
        for (int i = 0; i < 10; i++)
        {
            AudioSource tmpSource = ower.AddComponent<AudioSource>();
            allSources.Add(tmpSource);
        }
    }

    //停止播放
    public void Stop(string audioName)
    {
        for (int i = 0; i < allSources.Count; i++)
        {
            if (allSources[i].isPlaying && allSources[i].clip.name.Equals(audioName))
            {
                allSources[i].Stop();
            }
        }

    }
    //得到一个空闲的AudioSource
    public AudioSource GetFreeAudioSource(bool loop=false,float str=1f)
    {
        ReleaseFreeAudio();
        //遍历列表，返回一个空闲的
        for (int i = 0; i < allSources.Count; i++)
        {
            if (!allSources[i].isPlaying)
            {
                if (loop)
                {
                    allSources[i].loop = true;
                }
                else { allSources[i].loop = false; }
                allSources[i].volume = str;
                return allSources[i];
            }
        }
        //没有空闲的就新建一个返回
        AudioSource tmpSource = ower.AddComponent<AudioSource>();
        tmpSource.loop = loop;
        tmpSource.volume = str;
        allSources.Add(tmpSource);
        return tmpSource;
    }

    //因上面会新建AudioSource，所以需要及时释放
    public void ReleaseFreeAudio()
    {
        //记录空闲的audio数量
        int tmpCount = 0;
        //记录要释放的audioS
        List<AudioSource> tmpSources = new List<AudioSource>();
        for (int i = 0; i < allSources.Count; i++)
        {
            if (!allSources[i].isPlaying)
            {
                tmpCount++;
                if (tmpCount > 3)
                {
                    //把多余的添加进删除列表
                    tmpSources.Add(allSources[i]);
                }
            }
        }
        //完成清除
        for (int i = 0; i < tmpSources.Count; i++)
        {
            AudioSource tmpSource = tmpSources[i];
            allSources.Remove(tmpSource);
            GameObject.Destroy(tmpSource);
        }
        tmpSources.Clear();
        tmpSources = null;
    }
}

