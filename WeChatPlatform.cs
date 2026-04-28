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
}