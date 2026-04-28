/// <summary>
/// 平台身份。所有平台都应该具备，但不代表支持所有业务能力。
/// </summary>
public interface IPlatform
{
    string PlatformName { get; }
}

/// <summary>
/// 消息能力。
/// </summary>
public interface IMessagePlatform
{
    bool SendMessage(string message);
}

/// <summary>
/// 视频上传能力。
/// </summary>
public interface IVideoPlatform
{
    bool UploadVideo(string filePath);
}

/// <summary>
/// 用户信息能力。
/// </summary>
public interface IUserInfoPlatform
{
    string GetUserInfo(string userId);
}

/// <summary>
/// 支付能力。
/// </summary>
public interface IPaymentPlatform
{
    bool ProcessPayment(string orderId, decimal amount);
}

/// <summary>
/// 分享能力。
/// </summary>
public interface ISharePlatform
{
    bool Share(string content, string shareType);
}
