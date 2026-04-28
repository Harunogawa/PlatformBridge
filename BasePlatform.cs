using System;

/// <summary>
/// 平台基类只放所有平台共享的基础逻辑，不提供“假成功”的业务默认实现。
/// </summary>
public abstract class BasePlatform : IPlatform
{
    public abstract string PlatformName { get; }

    protected void Log(string message)
    {
        Console.WriteLine($"[{PlatformName}] {message}");
    }
}
