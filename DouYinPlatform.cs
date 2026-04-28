using System;

/// <summary>
/// 抖音平台：支持消息、视频、用户信息、支付和分享。
/// </summary>
public class DouYinPlatform : BasePlatform,
    IMessagePlatform,
    IVideoPlatform,
    IUserInfoPlatform,
    IPaymentPlatform,
    ISharePlatform
{
    public override string PlatformName => "抖音";

    public bool SendMessage(string message)
    {
        Log($"通过抖音私信发送消息：{message}");
        return true;
    }

    public bool UploadVideo(string filePath)
    {
        Log($"上传视频并处理封面、标签：{filePath}");
        return true;
    }

    public string GetUserInfo(string userId)
    {
        Log($"通过抖音开放平台获取用户信息：{userId}");
        return $"{{\"platform\":\"{PlatformName}\",\"userId\":\"{userId}\",\"nickname\":\"DouYinUser\"}}";
    }

    public bool ProcessPayment(string orderId, decimal amount)
    {
        Log($"调用字节支付，订单：{orderId}，金额：{amount:C}");
        return true;
    }

    public bool Share(string content, string shareType)
    {
        if (shareType is not ("video" or "image"))
        {
            Console.WriteLine($"[调度器] {PlatformName} 不支持 {shareType} 分享。");
            return false;
        }

        Log($"分享{shareType}内容：{content}");
        return true;
    }
}
