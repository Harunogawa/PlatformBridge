using System;

/// <summary>
/// 微信平台：支持消息、用户信息、支付和分享。这里故意不实现视频上传能力。
/// </summary>
public class WeChatPlatform : BasePlatform,
    IMessagePlatform,
    IUserInfoPlatform,
    IPaymentPlatform,
    ISharePlatform
{
    public override string PlatformName => "微信";

    public bool SendMessage(string message)
    {
        Log($"通过微信消息通道发送：{message}");
        return true;
    }

    public string GetUserInfo(string userId)
    {
        Log($"通过微信开放平台获取用户信息：{userId}");
        return $"{{\"platform\":\"{PlatformName}\",\"userId\":\"{userId}\",\"nickname\":\"WeChatUser\"}}";
    }

    public bool ProcessPayment(string orderId, decimal amount)
    {
        Log($"调用微信支付，订单：{orderId}，金额：{amount:C}");
        return true;
    }

    public bool Share(string content, string shareType)
    {
        if (shareType is not ("friend" or "timeline"))
        {
            Console.WriteLine($"[调度器] {PlatformName} 不支持 {shareType} 分享。");
            return false;
        }

        Log($"分享到{GetShareTargetName(shareType)}：{content}");
        return true;
    }

    private static string GetShareTargetName(string shareType)
    {
        return shareType switch
        {
            "friend" => "微信好友",
            "timeline" => "朋友圈",
            _ => shareType
        };
    }
}
