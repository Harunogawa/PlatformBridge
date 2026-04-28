# 平台桥接架构说明

这个示例使用“平台身份 + 能力接口”的方式组织平台调用逻辑。平台不再被要求实现所有功能，而是按自己真实支持的能力实现对应接口。

## 设计目标

- 平台切换时，上层只面对 `PlatformDispatcher`。
- 平台能力清晰可见：实现了什么接口，就支持什么功能。
- 不使用“默认返回成功”的基类方法，避免不支持的能力被误判为调用成功。
- 新增平台或新增能力时，改动范围尽量小。

## 核心结构

| 文件 | 职责 |
| --- | --- |
| `IPlatform.cs` | 定义平台身份接口和各类能力接口 |
| `BasePlatform.cs` | 提供平台名和公共日志能力 |
| `DouYinPlatform.cs` | 抖音平台能力实现 |
| `WeChatPlatform.cs` | 微信平台能力实现 |
| `PlatformDispatcher.cs` | 统一调度入口，并在调用前检查平台能力 |
| `TestRunner.cs` | 示例入口 |

## 接口拆分

`IPlatform` 只表示“这是一个平台”：

```csharp
public interface IPlatform
{
    string PlatformName { get; }
}
```

具体业务能力拆成独立接口：

```csharp
public interface IMessagePlatform
{
    bool SendMessage(string message);
}

public interface IVideoPlatform
{
    bool UploadVideo(string filePath);
}

public interface IUserInfoPlatform
{
    string GetUserInfo(string userId);
}

public interface IPaymentPlatform
{
    bool ProcessPayment(string orderId, decimal amount);
}

public interface ISharePlatform
{
    bool Share(string content, string shareType);
}
```

这样可以避免一个大接口强迫所有平台实现所有方法。

## 平台实现

抖音当前实现了全部示例能力：

```csharp
public class DouYinPlatform : BasePlatform,
    IMessagePlatform,
    IVideoPlatform,
    IUserInfoPlatform,
    IPaymentPlatform,
    ISharePlatform
{
}
```

微信当前没有实现 `IVideoPlatform`，所以它不支持视频上传：

```csharp
public class WeChatPlatform : BasePlatform,
    IMessagePlatform,
    IUserInfoPlatform,
    IPaymentPlatform,
    ISharePlatform
{
}
```

## 调度器逻辑

`PlatformDispatcher` 只依赖 `IPlatform`：

```csharp
private readonly IPlatform _platform;
```

调用具体能力前，先判断平台是否实现对应接口：

```csharp
public bool DispatchUploadVideo(string filePath)
{
    if (_platform is not IVideoPlatform videoPlatform)
    {
        return Unsupported("上传视频");
    }

    LogDispatch("上传视频");
    return videoPlatform.UploadVideo(filePath);
}
```

因此，当微信调用上传视频时，不会出现“默认成功”，而是明确返回不支持。

## 调用流程

```text
TestRunner
  -> 创建具体平台 DouYinPlatform / WeChatPlatform
  -> 创建 PlatformDispatcher
  -> 调用 DispatchSendMessage / DispatchUploadVideo / DispatchShare
  -> Dispatcher 检查平台是否支持该能力
  -> 支持则转发，不支持则返回失败并输出提示
```

## 使用示例

```csharp
IPlatform platform = new DouYinPlatform();
PlatformDispatcher dispatcher = new PlatformDispatcher(platform);

dispatcher.DispatchSendMessage("Hello");
dispatcher.DispatchUploadVideo("video.mp4");
dispatcher.DispatchProcessPayment("ORDER-001", 99.99m);
dispatcher.DispatchShare("一条要分享的内容", "video");
```

切换到微信时，上层调用方式不变：

```csharp
IPlatform platform = new WeChatPlatform();
PlatformDispatcher dispatcher = new PlatformDispatcher(platform);

dispatcher.DispatchSendMessage("Hello");
dispatcher.DispatchUploadVideo("video.mp4"); // 微信未实现 IVideoPlatform，会提示不支持
```

## 新增平台

新增平台时，先继承 `BasePlatform`，然后按需实现能力接口：

```csharp
public class SomePlatform : BasePlatform, IMessagePlatform, ISharePlatform
{
    public override string PlatformName => "SomePlatform";

    public bool SendMessage(string message)
    {
        Log($"发送消息：{message}");
        return true;
    }

    public bool Share(string content, string shareType)
    {
        Log($"分享内容：{content}");
        return true;
    }
}
```

如果该平台不支持支付，就不要实现 `IPaymentPlatform`。

## 新增能力

新增能力时，推荐按这个顺序：

1. 新增一个能力接口，例如 `ILoginPlatform`。
2. 让支持该能力的平台实现这个接口。
3. 在 `PlatformDispatcher` 中增加对应的 `DispatchLogin` 方法。
4. 在 `TestRunner` 或业务层调用新的调度方法。

示例：

```csharp
public interface ILoginPlatform
{
    bool Login();
}
```

## 当前能力矩阵

| 能力 | 抖音 | 微信 |
| --- | --- | --- |
| 消息 | 支持 | 支持 |
| 视频上传 | 支持 | 不支持 |
| 用户信息 | 支持 | 支持 |
| 支付 | 支持 | 支持 |
| 分享 | 支持 video / image | 支持 friend / timeline |

## 验证命令

```powershell
dotnet build PlatformBridge.sln
dotnet run --project PlatformDemo.csproj
```

当前构建结果应为 `0` 错误、`0` 警告。
