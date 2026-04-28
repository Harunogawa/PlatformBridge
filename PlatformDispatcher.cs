using System;

/// <summary>
/// 平台调度器 - 中转脚本，根据配置分发请求到对应平台
/// </summary>
public class PlatformDispatcher
{
    private readonly BasePlatform _platform;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="platform">平台实例</param>
    public PlatformDispatcher(BasePlatform platform)
    {
        _platform = platform;
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    public bool DispatchSendMessage(string message)
    {
        Console.WriteLine($"[调度器] 正在转发到 {_platform.PlatformName}...");
        return _platform.SendMessage(message);
    }

    /// <summary>
    /// 上传视频
    /// </summary>
    public bool DispatchUploadVideo(string filePath)
    {
        Console.WriteLine($"[调度器] 正在转发到 {_platform.PlatformName}...");
        return _platform.UploadVideo(filePath);
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    public string DispatchGetUserInfo(string userId)
    {
        Console.WriteLine($"[调度器] 正在转发到 {_platform.PlatformName}...");
        return _platform.GetUserInfo(userId);
    }
}