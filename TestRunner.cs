using System;

/// <summary>
/// 测试脚本 - 用于测试平台调用逻辑
/// </summary>
public class TestRunner
{
    public static void Main(string[] args)
    {
        Console.WriteLine("========== 平台调用测试 (Override版) ==========\n");

        // 测试抖音平台
        Console.WriteLine("--- 测试抖音平台 ---");
        BasePlatform douYin = new DouYinPlatform();
        PlatformDispatcher douYinDispatcher = new PlatformDispatcher(douYin);
        
        douYinDispatcher.DispatchSendMessage("Hello 抖音!");
        douYinDispatcher.DispatchUploadVideo("video1.mp4");
        string dyUserInfo = douYinDispatcher.DispatchGetUserInfo("1001");
        Console.WriteLine($"用户信息: {dyUserInfo}\n");

        // 测试微信平台
        Console.WriteLine("--- 测试微信平台 ---");
        BasePlatform weChat = new WeChatPlatform();
        PlatformDispatcher weChatDispatcher = new PlatformDispatcher(weChat);
        
        weChatDispatcher.DispatchSendMessage("Hello 微信!");
        weChatDispatcher.DispatchUploadVideo("video2.mp4");
        string wcUserInfo = weChatDispatcher.DispatchGetUserInfo("2001");
        Console.WriteLine($"用户信息: {wcUserInfo}\n");

        Console.WriteLine("========== 测试完成 ==========");
    }
}