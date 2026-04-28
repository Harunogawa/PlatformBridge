using System;

/// <summary>
/// 微信平台 - 继承BasePlatform，使用override重写特定逻辑
/// </summary>
public class WeChatPlatform : BasePlatform
{
    public override string PlatformName => "微信";

    /// <summary>
    /// Override: 微信特殊的消息处理
    /// </summary>
    public override bool SendMessage(string message)
    {
        // 微信特有逻辑
        Console.WriteLine($"[微信 Override] 发送消息(带微信特殊处理): {message}");
        return true;
    }

    /// <summary>
    /// Override: 微信视频上传需要企业微信认证
    /// </summary>
    public override bool UploadVideo(string filePath)
    {
        Console.WriteLine($"[微信 Override] 上传视频(微信特有): {filePath}");
        // 微信特有: 企业认证、权限校验等
        return true;
    }

    // GetUserInfo 使用默认实现，不重写

    /// <summary>
    /// Override: 微信特殊的支付流程
    /// 微信使用微信支付(WeChat Pay)
    /// </summary>
    public override bool ProcessPayment(string orderId, decimal amount)
    {
        Console.WriteLine($"[微信 Override] 微信支付系统 - 订单ID: {orderId}, 金额: {amount}元");
        Console.WriteLine($"[微信] 使用微信支付SDK(WeChat Pay)完成支付");
        Console.WriteLine($"[微信] 企业商户号认证完成");
        return true;
    }

    /// <summary>
    /// Override: 微信特殊的分享流程
    /// 微信支持朋友圈、好友、公众号等多种分享方式
    /// </summary>
    public override bool Share(string content, string shareType)
    {
        Console.WriteLine($"[微信 Override] 微信分享系统 - 类型: {shareType}, 内容: {content}");
        if (shareType == "friend")
        {
            Console.WriteLine($"[微信] 通过微信SDK分享到好友");
        }
        else if (shareType == "circle")
        {
            Console.WriteLine($"[微信] 通过微信SDK分享到朋友圈");
        }
        else if (shareType == "official")
        {
            Console.WriteLine($"[微信] 通过微信官方账号分享");
        }
        return true;
    }