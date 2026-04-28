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

    /// <summary>
    /// 支付功能
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <param name="amount">支付金额</param>
    /// <returns>支付结果</returns>
    bool ProcessPayment(string orderId, decimal amount);

    /// <summary>
    /// 分享功能
    /// </summary>
    /// <param name="content">分享内容</param>
    /// <param name="shareType">分享类型(text/image/video)</param>
    /// <returns>分享结果</returns>
    bool Share(string content, string shareType);
}