using System;

/// <summary>
/// 平台基类 - 提供默认实现，子类可Override
/// </summary>
public abstract class BasePlatform : IPlatform
{
    /// <summary>
    /// 平台名称 - 子类必须重写
    /// </summary>
    public abstract string PlatformName { get; }

    /// <summary>
    /// 发送消息 - 默认实现，可被Override
    /// </summary>
    public virtual bool SendMessage(string message)
    {
        Console.WriteLine($"[{PlatformName}] 默认发送消息: {message}");
        return true;
    }

    /// <summary>
    /// 上传视频 - 默认实现，可被Override
    /// </summary>
    public virtual bool UploadVideo(string filePath)
    {
        Console.WriteLine($"[{PlatformName}] 默认上传视频: {filePath}");
        return true;
    }

    /// <summary>
    /// 获取用户信息 - 默认实现，可被Override
    /// </summary>
    public virtual string GetUserInfo(string userId)
    {
        Console.WriteLine($"[{PlatformName}] 默认获取用户信息: {userId}");
        return $"{{\"platform\":\"{PlatformName}\",\"userId\":\"{userId}\"}}";
    }

    /// <summary>
    /// 支付功能 - 默认实现，可被Override
    /// </summary>
    public virtual bool ProcessPayment(string orderId, decimal amount)
    {
        Console.WriteLine($"[{PlatformName}] 默认支付处理 - 订单ID: {orderId}, 金额: {amount}元");
        return true;
    }

    /// <summary>
    /// 分享功能 - 默认实现，可被Override
    /// </summary>
    public virtual bool Share(string content, string shareType)
    {
        Console.WriteLine($"[{PlatformName}] 默认分享 - 类型: {shareType}, 内容: {content}");
        return true;
    }