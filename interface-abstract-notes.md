# C# 接口、抽象类、virtual 和普通方法笔记

这份笔记基于当前的平台桥接示例，帮助区分 `interface`、`abstract class`、`abstract`、`virtual` 和普通方法的用法。

## 一句话总结

```text
interface      = 能力声明
abstract class = 公共底座
abstract 成员  = 子类必须实现
virtual 成员   = 基类给默认实现，子类可改可不改
普通方法       = 基类写好的工具，子类直接用
```

## interface：定义能力

接口用来规定“一个类具备什么能力”。

```csharp
public interface IVideoPlatform
{
    bool UploadVideo(string filePath);
}
```

意思是：

```text
谁声明实现 IVideoPlatform，谁就必须写 UploadVideo 方法。
```

例如：

```csharp
public class DouYinPlatform : BasePlatform, IVideoPlatform
{
    public bool UploadVideo(string filePath)
    {
        Log($"上传视频：{filePath}");
        return true;
    }
}
```

如果某个平台不支持视频上传，就不要实现这个接口。

```csharp
public class WeChatPlatform : BasePlatform
{
}
```

注意：

```text
接口不是“可以写也可以不写”。
而是：你声明实现了哪个接口，就必须实现这个接口里的所有成员。
```

## abstract class：公共底座

抽象类既可以规定子类必须实现什么，也可以放公共代码。

```csharp
public abstract class BasePlatform : IPlatform
{
    public abstract string PlatformName { get; }

    protected void Log(string message)
    {
        Console.WriteLine($"[{PlatformName}] {message}");
    }
}
```

这里 `BasePlatform` 的职责是：

```text
1. 规定所有平台必须有 PlatformName
2. 提供所有平台都能复用的 Log 方法
```

子类继承以后，可以直接使用 `Log`：

```csharp
public class DouYinPlatform : BasePlatform
{
    public override string PlatformName => "抖音";

    public void Test()
    {
        Log("测试日志");
    }
}
```

## abstract 成员：必须实现

如果基类里写了 `abstract`：

```csharp
public abstract string PlatformName { get; }
```

子类必须用 `override` 实现：

```csharp
public override string PlatformName => "抖音";
```

规则：

```text
abstract = 没有默认实现，子类必须 override
```

适合场景：

```text
每个平台都必须有，但每个平台都不一样。
```

例如：

```csharp
public abstract bool Login();
```

抖音、微信都必须登录，但登录 SDK 不一样，所以让子类自己实现。

## virtual 成员：默认实现，可覆盖

如果基类里写了 `virtual`：

```csharp
public virtual bool Logout()
{
    Log("默认退出登录");
    return true;
}
```

子类可以不写，直接用默认实现。

```csharp
public class DouYinPlatform : BasePlatform
{
    public override string PlatformName => "抖音";
}
```

调用 `Logout()` 时，会执行基类默认逻辑。

如果某个平台想改，可以 `override`：

```csharp
public override bool Logout()
{
    Log("抖音退出登录");
    return true;
}
```

规则：

```text
virtual + 子类没 override = 用基类默认实现
virtual + 子类 override = 用子类实现
```

适合场景：

```text
大部分平台逻辑一样，少数平台需要特殊处理。
```

## 普通方法：直接继承使用

普通方法没有 `abstract`，也没有 `virtual`：

```csharp
protected void Log(string message)
{
    Console.WriteLine($"[{PlatformName}] {message}");
}
```

子类继承以后可以直接用：

```csharp
Log("发送消息");
```

但不能 `override`。

规则：

```text
普通方法 = 基类写好的固定工具
子类可以用
子类默认不能改
```

如果希望子类能改，就把它变成 `virtual`：

```csharp
protected virtual void Log(string message)
{
    Console.WriteLine($"[{PlatformName}] {message}");
}
```

子类再覆盖：

```csharp
protected override void Log(string message)
{
    Console.WriteLine($"[自定义日志] {message}");
}
```

## 三种成员对比

| 写法 | 子类是否必须写 | 子类能否 override | 有无默认实现 | 适合场景 |
| --- | --- | --- | --- | --- |
| `abstract` | 必须 | 必须 override | 没有 | 强制每个子类自己实现 |
| `virtual` | 不必须 | 可以 override | 有 | 默认相同，允许特殊化 |
| 普通方法 | 不必须 | 默认不能 | 有 | 公共工具，统一复用 |

## 在当前平台架构中的分工

```text
IPlatform:
  表示平台身份。

IMessagePlatform / IVideoPlatform / IPaymentPlatform:
  表示可选能力。

BasePlatform:
  表示所有平台共同底座。
  放平台名约束、日志、公共配置、公共流程。

DouYinPlatform / WeChatPlatform:
  继承 BasePlatform。
  按需实现能力接口。
```

示例：

```csharp
public class DouYinPlatform : BasePlatform,
    IMessagePlatform,
    IVideoPlatform,
    IPaymentPlatform
{
    public override string PlatformName => "抖音";

    public bool UploadVideo(string filePath)
    {
        Log($"上传视频：{filePath}");
        return true;
    }
}
```

这段代码表示：

```text
DouYinPlatform 是一个平台。
它继承了 BasePlatform，所以可以用 Log。
它实现了 IVideoPlatform，所以必须写 UploadVideo。
```

## 最好记的口诀

```text
接口看能力。
抽象看底座。
abstract 是必答题。
virtual 是参考答案。
普通方法是公共工具。
```
