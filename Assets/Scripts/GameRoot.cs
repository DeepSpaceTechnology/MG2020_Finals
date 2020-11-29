using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot instance;
    public int money = 100;
    public int[] skillPrice;
    public string[][] talksArr;
    public string[] newsArr;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        talksArr = new string[4][];
        for (int i = 0; i < 4; i++)
        {
            talksArr[i] = new string[4];
        }
        talksArr[0][0] = "行，听你的。";
        talksArr[0][1] = "老板大气！";
        talksArr[0][2] = "我也觉得红色好看。";
        talksArr[0][3] = "红色确实好看啊。";

        talksArr[1][0] = "一会大家就都知道了！";
        talksArr[1][1] = "那我去和别人说说。";
        talksArr[1][2] = "大家来看看我的红衣服！";
        talksArr[1][3] = "听说现在红色最流行噢！";

        talksArr[2][0] = "我永远喜欢红色！";
        talksArr[2][1] = "我是红色死忠粉！";
        talksArr[2][2] = "红色衣服才好看，不接受反驳。";
        talksArr[2][3] = "守护世界上最好的红色！";

        talksArr[3][0] = "这你可找对人了！";
        talksArr[3][1] = "今日演讲《为什么红色最好看》";
        talksArr[3][2] = "还没有我说服不了的人。";
        talksArr[3][3] = "我就是意见领袖！";

        newsArr = new string[99];
        newsArr[0] = "今年最流行：蓝色！推博票选本年最流行颜色发布。";
        newsArr[1] = "推博网友票选今年最丑颜色为红色，网友：不能更赞同。";
        newsArr[2] = "制衣厂大量进货蓝布，工人加班生产暖人心。";
        newsArr[4] = "世风日下，红衣学生毫无遮掩大街散步惹争议。";
        newsArr[5] = "男子相亲穿红衣，女子当场拍桌离开引热议。";
        newsArr[6] = "黑心村长收受红包贿赂，正直女儿嫌颜色不搭主动举报。";
        newsArr[7] = "无良商贩将红萝卜洗白为白萝卜销售，有关部门已介入。";
        newsArr[8] = "童年回忆！《蓝猫》官宣将推出高清重置版。";
        newsArr[9] = "潮！我市街头红绿灯今起改为蓝绿灯。";
        newsArr[10] = "气愤！无良商家将过气红衣售给色盲患者引抵制。";
        newsArr[11] = "蓝色就是老土？时尚博主发言上热搜，网友怒赞。";
        newsArr[12] = "多位美妆博主发布蓝色系穿搭视频，蓝衣大卖引关注。";
        newsArr[13] = "红色遇冷，多家红色系LOGO品牌考虑更换LOGO。";
        newsArr[14] = "暖心！资金紧张，良心老板拒绝拖欠，用蓝布代替工资发放。";
        newsArr[15] = "315曝光：无良商家在红衣中加入蓝色素。";
        newsArr[16] = "我市出现少量穿红衣人士，精神病院现已介入。";
        newsArr[17] = "我市商业协会就红色的少量出现表示担忧。";
        newsArr[18] = "钢铁厂工会发放红衣？厂长：正在调查。";
        newsArr[19] = "红色遇冷，草莓种植户大量改种蓝莓。";
        newsArr[20] = "红衣女子大胆出街引围观。";
        newsArr[21] = "证交所拟将股票上涨示色改为蓝色。";
        newsArr[22] = "怀旧！蓝猫形象成新潮流。";
        newsArr[23] = "红色不行？百岁螃蟹揭秘长寿原因：不红。";
        newsArr[24] = "专家：研究表明蓝色衣服更适合上班时穿着。";
        newsArr[25] = "传口红厂将开口蓝生产线，厂长召开记者会证实消息。";
        newsArr[26] = "专家称红色可能会导致工厂不满情绪增加。";
        newsArr[27] = "红色火了？专家：从风水上看短期内不可能。";
        newsArr[28] = "专家：蓝色能帮助集中注意力，更适合作为校服。";
        newsArr[29] = "高校学生组建红色社团，校长：正在商榷。";
        newsArr[30] = "女子私下穿红衣被偷拍举报引争议。";
        newsArr[31] = "职业篮球协会不满红色团体，经理：有境外势力操纵。";
        newsArr[32] = "避免争议，购物节白衣占领热销榜单!";
        newsArr[33] = "网传红色可能将流行，专家：无可奉告。";
        newsArr[34] = "专家称穿红衣会加速全球变暖。";
        newsArr[35] = "正名！红色团体获得红蓝色辩论赛大奖。";
        newsArr[36] = "人民广场惊现红色演说，富豪团体紧急介入扰乱现场。";
        newsArr[37] = "“自古红蓝出CP”？专家辟谣：不可能！";
        newsArr[38] = "首富称办公室里出现红色可能会造成员工效率降低。";
        newsArr[39] = "知名主播孙哭山宣布日后游戏直播不再抢蓝buff。";
        newsArr[40] = "突发！两男子因红蓝问题大打出手，场面混乱。";
        newsArr[41] = "红色团体称受到不公正对待，律师：法律允许起诉。";
        newsArr[42] = "红蓝之争愈演愈烈，专家呼吁各方保持冷静";
        newsArr[43] = "红绿灯没有蓝色就是歧视蓝色？爱蓝人士推博言论引热议。";
        newsArr[44] = "专家称穿蓝衣会导致海平面上升。";
        newsArr[45] = "市纪检委：本月来收受红包的腐败分子相对增加。";
        newsArr[46] = "蓝色不行？专家：红色是血液的颜色。";
        newsArr[47] = "红色团体组织集会活动，多款红色造型走红。";
        newsArr[48] = "蓝衣制衣厂遭抵制，厂长宣布考虑转型。";
        newsArr[49] = "蓝色爆火只因有人煽风点火？知名博主爆料引热议。";
        newsArr[50] = "狂热红衣分子当街发表演说受热议。";
        newsArr[51] = "专家称老年人更适合穿红衣。";
        newsArr[52] = "推博宣布将LOGO改为纯红色。";
        newsArr[53] = "女子瘫痪六十年竟因红光照射奇迹康复，专家：红色是生命之光。";
        newsArr[54] = "男子脑瘫七十年只会说“红色”，专家：红色是生命的本能。";
        newsArr[55] = "知名网红孙哭山对年前蓝色爆火表示负责。";
        newsArr[56] = "过气蓝调歌手宣布改唱红歌。";
        newsArr[57] = "某理工大学科研团队宣布成功将大海变成红色。";
        newsArr[58] = "不满谐音梗，男权组织疑改称男性为红性。";
        newsArr[59] = "专家称穿红衣更容易减肥。";
        newsArr[60] = "红粉狂喜！知名眼镜厂宣布成功研发只能看见红色的眼镜。";
        newsArr[61] = "篮球协会宣布将考虑将篮球更名为红球。";
        newsArr[62] = "知名演讲家来访！《我为什么选择红色》今晚在市剧院演出。";
        newsArr[63] = "专家称穿红衣能延长寿命。";
        newsArr[64] = "善恶终有报！前蓝色染料厂厂长涉嫌煽动罪今批捕。";
        newsArr[65] = "化工厂二氧化氮泄露染红天空，市民纷纷出门拍照共赏美景。";
        newsArr[66] = "大学生厌恶蓝天不出门旷课四年，律师：个人选择受法律保护。";
        newsArr[67] = "红色工厂诞生！制衣厂宣布今起只生产红色衣服。";
        newsArr[68] = "暖心！我市街头红绿灯今起改为红红灯。";
    }

    public string GetRamTalkByType(int i)
    {
        return talksArr[i][Random.Range(0, 4)];
    }
}
