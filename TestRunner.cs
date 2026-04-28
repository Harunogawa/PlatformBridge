using System;

/// <summary>
/// 测试入口：演示同一个调度器如何面对不同平台能力。
/// </summary>
public class TestRunner
{
    public static void Main(string[] args)
    {
        Console.WriteLine("========== 平台能力调度测试 ==========\n");

        RunPlatform(new DouYinPlatform());
        RunPlatform(new WeChatPlatform());

        Console.WriteLine("========== 测试完成 ==========");
    }

    private static void RunPlatform(IPlatform platform)
    {
        Console.WriteLine($"--- 当前平台：{platform.PlatformName} ---");

        PlatformDispatcher dispatcher = new PlatformDispatcher(platform);

        dispatcher.DispatchSendMessage($"Hello {platform.PlatformName}!");
        dispatcher.DispatchUploadVideo("video.mp4");

        string userInfo = dispatcher.DispatchGetUserInfo("1001");
        Console.WriteLine($"用户信息：{(string.IsNullOrEmpty(userInfo) ? "未获取" : userInfo)}");

        dispatcher.DispatchProcessPayment("ORDER-001", 99.99m);
        dispatcher.DispatchShare("一条要分享的内容", platform.PlatformName == "微信" ? "timeline" : "video");

        Console.WriteLine();
    }
}
