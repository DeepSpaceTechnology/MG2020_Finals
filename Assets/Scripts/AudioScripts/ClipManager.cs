using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public class ClipManager
{

    string[] clipName;//保存歌曲名字
    SingleClip[] allSingle;//在内存中读入音频文件

    //构造方法
    public ClipManager()
    {
        ReadConfig();
        LoadClips();
    }

    //歌曲加载到内存
    public void LoadClips()
    {
        allSingle = new SingleClip[clipName.Length];
        for (int i = 0; i < clipName.Length; i++)
        {
            AudioClip tmpClip = Resources.Load<AudioClip>(clipName[i]);
            SingleClip tmpSingle = new SingleClip(tmpClip);
            allSingle[i] = tmpSingle;
        }
    }

    //根据名字找到clip并返回
    public SingleClip FindClipByNane(string tmpClip)
    {
        int index = -1;
        for (int i = 0; i < clipName.Length; i++)
        {
            if (clipName[i].Equals(tmpClip))
            {
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            return allSingle[index];
        }
        else
        {
            return null;
        }
    }
    //读取配置文件 位于StreamingAssets下名为AudioConfig
    //用空格分割名字与文件名
    public void ReadConfig()
    {
        //本地路径
        var fileAddress = System.IO.Path.Combine(Application.streamingAssetsPath, "AudioConfig.txt");
        FileInfo fInfo0 = new FileInfo(fileAddress);
        if (fInfo0.Exists)
        {
            StreamReader r = new StreamReader(fileAddress);
            //StreamReader默认的是UTF8的不需要转格式了，因为有些中文字符的需要有些是要转的，下面是转成String代码
            //byte[] data = new byte[1024];
            // data = Encoding.UTF8.GetBytes(r.ReadToEnd());
            // s = Encoding.UTF8.GetString(data, 0, data.Length);


            string tmpLine = r.ReadLine();
            int lineCount = 0;
            if (int.TryParse(tmpLine, out lineCount))
            {
                clipName = new string[lineCount];
                for (int i = 0; i < lineCount; i++)
                {
                    tmpLine = r.ReadLine();
                    //根据空格把一行分别存在splite数组中
                    string[] splite = tmpLine.Split(" ".ToCharArray());
                    clipName[i] = splite[0];
                }
            }
        }
    }
}

