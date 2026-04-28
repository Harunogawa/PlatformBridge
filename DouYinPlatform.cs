using System;

/// <summary>
/// 抖音平台 - 继承BasePlatform，使用override重写特定逻辑
/// </summary>
public class DouYinPlatform : BasePlatform
{
    public override string PlatformName => "抖音";

    /// <summary>
    /// Override: 抖音特殊的消息处理
    /// </summary>
    public override bool SendMessage(string message)
    {
        // 抖音特有逻辑
        Console.WriteLine($"[抖音 Override] 发送消息(带抖音特殊处理): {message}");
        return true;
    }

    /// <summary>
    /// Override: 抖音视频上传需要额外的封面处理
    /// </summary>
    public override bool UploadVideo(string filePath)
    {
        Console.WriteLine($"[抖音 Override] 上传视频(抖音特有): {filePath}");
        // 抖音特有: 添加封面、标签等
        return true;
    }

    // GetUserInfo 使用默认实现，不重写

    /// <summary>
    /// Override: 抖音特殊的支付流程
    /// 抖音使用字节跳动支付体系
    /// </summary>
    public override bool ProcessPayment(string orderId, decimal amount)
    {
        Console.WriteLine($"[抖音 Override] 字节跳动支付系统 - 订单ID: {orderId}, 金额: {amount}元");
        Console.WriteLine($"[抖音] 使用字节跳动支付SDK完成支付");
        return true;
    }

    /// <summary>
    /// Override: 抖音特殊的分享流程
    /// 抖音支持视频、图文等多种分享方式
    /// </summary>
    public override bool Share(string content, string shareType)
    {
        Console.WriteLine($"[抖音 Override] 抖音分享系统 - 类型: {shareType}, 内容: {content}");
        if (shareType == "video")
        {
            Console.WriteLine($"[抖音] 通过抖音视频SDK分享");
        }
        else if (shareType == "image")
        {
            Console.WriteLine($"[抖音] 通过抖音图文SDK分享");
        }
        return true;
    }