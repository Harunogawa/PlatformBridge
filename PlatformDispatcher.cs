using System;

/// <summary>
/// 平台调度器：只依赖平台身份接口，并在调用具体功能前检查平台是否具备该能力。
/// </summary>
public class PlatformDispatcher
{
    private readonly IPlatform _platform;

    public PlatformDispatcher(IPlatform platform)
    {
        _platform = platform ?? throw new ArgumentNullException(nameof(platform));
    }

    public bool DispatchSendMessage(string message)
    {
        if (_platform is not IMessagePlatform messagePlatform)
        {
            return Unsupported("发送消息");
        }

        LogDispatch("发送消息");
        return messagePlatform.SendMessage(message);
    }

    public bool DispatchUploadVideo(string filePath)
    {
        if (_platform is not IVideoPlatform videoPlatform)
        {
            return Unsupported("上传视频");
        }

        LogDispatch("上传视频");
        return videoPlatform.UploadVideo(filePath);
    }

    public string DispatchGetUserInfo(string userId)
    {
        if (_platform is not IUserInfoPlatform userInfoPlatform)
        {
            Unsupported("获取用户信息");
            return string.Empty;
        }

        LogDispatch("获取用户信息");
        return userInfoPlatform.GetUserInfo(userId);
    }

    public bool DispatchProcessPayment(string orderId, decimal amount)
    {
        if (_platform is not IPaymentPlatform paymentPlatform)
        {
            return Unsupported("支付");
        }

        LogDispatch("支付");
        return paymentPlatform.ProcessPayment(orderId, amount);
    }

    public bool DispatchShare(string content, string shareType)
    {
        if (_platform is not ISharePlatform sharePlatform)
        {
            return Unsupported("分享");
        }

        LogDispatch("分享");
        return sharePlatform.Share(content, shareType);
    }

    private void LogDispatch(string action)
    {
        Console.WriteLine($"[调度器] 转发“{action}”到 {_platform.PlatformName}");
    }

    private bool Unsupported(string action)
    {
        Console.WriteLine($"[调度器] {_platform.PlatformName} 不支持“{action}”能力。");
        return false;
    }
}
