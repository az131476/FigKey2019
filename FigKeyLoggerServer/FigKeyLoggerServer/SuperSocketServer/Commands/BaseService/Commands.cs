using System;

namespace SuperSocketServer.Commands.BaseService
{
    /// <summary>
    /// 步骤
    /// </summary>
    public enum Step:ushort
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
        [Description("设备信息")]
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
        Announce = 116,

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
        Login = 130,

        [Description("发送解密时间")]
        DecSendTime = 131,

        [Description("设置解密时间")]
        DecSetTime = 132,

        [Description("找到对称密钥")]
        DecFindSecretKey = 133,

        [Description("招标人解密")]
        DecodeComplete = 134,

        [Description("解密补救")]
        DecRemedy = 135,

        [Description("唱标")]
        Announce = 136,

        [Description("异议")]
        Dissent = 137,

        [Description("签名")]
        Signature = 138,

        //多一个抽取系数的环境（如需要抽）
        [Description("抽取代表")]
        BidderCount = 139,

        [Description("抽取系数")]
        Coef = 141,

        [Description("系数集合回复")]
        CoefCollectionReply = 142,

        [Description("系数抽取结束回复")]
        CoefEndReply = 143

    }

    /// <summary>
    /// 双信封第二信封开标流程
    /// </summary>
    public enum DoubBidDocTwo 
    {
        [Description("登陆")]
        Login = 150,

        [Description("招标人解密")]  //只能由招标人发出
        DecodeComplete = 151,

        [Description("唱标")]
        Announce = 152,

        [Description("异议")]
        Dissent = 153,

        [Description("签名")]
        Signature = 154,

        //多一个抽取系数的环境（如需要抽）
        [Description("抽取代表")]
        BidderCount = 155,

        [Description("抽取系数")]
        Coef = 156,

        [Description("系数集合回复")]
        CoefCollectionReply = 157,

        [Description("系数抽取结束回复")]
        CoefEndReply = 158,
        //多一个查询第一信封评审通过情况
        [Description("查看第一信封评标通过情况")]
        EvalResult = 159
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
