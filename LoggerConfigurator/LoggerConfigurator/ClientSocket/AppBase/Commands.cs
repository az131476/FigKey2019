using System;

namespace SuperSocketClient.AppBase
{
    /// <summary>
    /// 步骤
    /// </summary>
    public enum Step
    {
        [Description("解密界面")]
        DecView = 10,
        [Description("唱标界面")]
        AnnoView = 11,
        [Description("抽取系数界面")]
        ExtraCoffiView = 12,
        [Description("签名界面")]
        SignatuerView = 13,
        //双信封第二信封开标专有
        [Description("查看第一信封评审情况界面")]
        EvalResultView = 14

    }


    /// <summary>
    /// 资格预审申请文件开标流程
    /// </summary>
    public enum PreAppleFile
    {
        [Description("登陆")]
        Login = 100,

        [Description("发送解密时间")]
        DecSendTime = 101,

        [Description("设置解密时间")]
        DecSetTime = 102,

        [Description("找到对称密钥")]
        DecFindSecretKey = 103,

        [Description("招标人解密")]  //只能由招标人发出
        DecodeComplete = 104,

        [Description("解密补救")]
        DecRemedy = 105,

        [Description("唱标")]
        Announce = 106,

        [Description("异议")]
        Dissent = 107,

        [Description("签名")]
        Signature = 108
    }

    /// <summary>
    /// 单信封开标流程
    /// </summary>
    public enum SingleBidDoc 
    {
        [Description("登陆")]
        Login = 110,

        [Description("发送解密时间")]
        DecSendTime = 111,

        [Description("设置解密时间")]
        DecSetTime = 112,

        [Description("找到对称密钥")]
        DecFindSecretKey = 113,

        [Description("全部解密完成")]  //只能由招标人发出
        DecodeComplete = 114,

        [Description("解密补救")]
        DecRemedy = 115,

        [Description("唱标")]
        Announce = 5,

        [Description("异议")]
        Dissent = 117,

        [Description("签名")]
        Signature = 118,

        //多一个抽取系数的环境（如需要抽）
        [Description("抽取代表")]
        BidderCount = 119,

        [Description("抽取系数")]
        Coef = 121,

        [Description("系数集合回复")]
        CoefCollectionReply = 122,

        [Description("系数抽取结束回复")]
        CoefEndReply = 123

    }

    /// <summary>
    /// 双信封第一信封开标流程
    /// </summary>
    public enum DoubBidDocOne 
    {
        [Description("登陆")]
        Login = 30,

        [Description("发送解密时间")]
        DecSendTime = 31,

        [Description("设置解密时间")]
        DecSetTime = 32,

        [Description("找到对称密钥")]
        DecFindSecretKey = 33,

        [Description("招标人解密")]
        DecodeComplete = 34,

        [Description("解密补救")]
        DecRemedy = 35,

        [Description("唱标")]
        Announce = 36,

        [Description("异议")]
        Dissent = 37,

        [Description("签名")]
        Signature = 38,

        //多一个抽取系数的环境（如需要抽）
        [Description("抽取代表")]
        BidderCount = 39,

        [Description("抽取系数")]
        Coef = 41,

        [Description("系数集合回复")]
        CoefCollectionReply = 42,

        [Description("系数抽取结束回复")]
        CoefEndReply = 43

    }

    /// <summary>
    /// 双信封第二信封开标流程
    /// </summary>
    public enum DoubBidDocTwo 
    {
        [Description("登陆")]
        Login = 50,

        [Description("招标人解密")]  //只能由招标人发出
        DecodeComplete = 51,

        [Description("唱标")]
        Announce = 52,

        [Description("异议")]
        Dissent = 53,

        [Description("签名")]
        Signature = 54,

        //多一个抽取系数的环境（如需要抽）
        [Description("抽取代表")]
        BidderCount = 55,

        [Description("抽取系数")]
        Coef = 56,

        [Description("系数集合回复")]
        CoefCollectionReply = 57,

        [Description("系数抽取结束回复")]
        CoefEndReply = 58,
        //多一个查询第一信封评审通过情况
        [Description("查看第一信封评标通过情况")]
        EvalResult = 59
    }

    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string des)
        {
            Description = des;
        }
        public string Description { get; set; }
    }

    public static class EnumUtil
    {
        public static string GetDescription(this Enum value, bool nameInstead = true)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
                return null;

            var field = type.GetField(name);
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute == null && nameInstead)
                return name;
            return attribute?.Description;
        }
    }
}
