using System;

/// <summary>
/// 平台接口 - 统一定义抖音和微信的调用规范
/// </summary>
public interface IPlatform
{
    /// <summary>
    /// 平台名称
    /// </summary>
    string PlatformName { get; }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="message">消息内容</param>
    /// <returns>是否成功</returns>
    bool SendMessage(string message);

    /// <summary>
    /// 上传视频
    /// </summary>
    /// <param name="filePath">视频文件路径</param>
    /// <returns>是否成功</returns>
    bool UploadVideo(string filePath);

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户信息JSON</returns>
    string GetUserInfo(string userId);
}