# 平台调用架构设计文档

## 一、架构概述

本项目采用 **策略模式 + 抽象基类** 的设计，为抖音和微信等多平台提供统一的调用接口，通过中转调度器实现灵活的平台切换和逻辑定制。

### 1.1 设计目标

- ✅ **统一接口**：所有平台遵循同一契约
- ✅ **平台解耦**：测试脚本不需要了解具体平台实现
- ✅ **灵活定制**：每个平台可独立 Override 特定方法
- ✅ **易于扩展**：新增平台只需继承基类并 Override 必要方法

### 1.2 核心特性

| 特性 | 说明 |
|------|------|
| 接口定义 | 定义统一的平台调用规范 |
| 基类默认实现 | 使用 `virtual` 提供默认实现，子类可选择 Override |
| 选择性重写 | 不用每个方法都重写，只需重写特异化逻辑 |
| 中转调度 | 通过调度器统一管理平台分发 |

---

## 二、架构组件

### 2.1 IPlatform（接口层）

**文件**：`IPlatform.cs`

**职责**：定义所有平台必须实现的方法规范

```csharp
public interface IPlatform
{
    string PlatformName { get; }
    bool SendMessage(string message);
    bool UploadVideo(string filePath);
    string GetUserInfo(string userId);
}
```

**包含方法**：
- `PlatformName`：平台名称标识
- `SendMessage()`：发送消息
- `UploadVideo()`：上传视频
- `GetUserInfo()`：获取用户信息

---

### 2.2 BasePlatform（基类层）

**文件**：`BasePlatform.cs`

**职责**：提供默认实现，为各平台减少重复代码

```csharp
public abstract class BasePlatform : IPlatform
{
    public abstract string PlatformName { get; }
    
    public virtual bool SendMessage(string message) { ... }
    public virtual bool UploadVideo(string filePath) { ... }
    public virtual string GetUserInfo(string userId) { ... }
}
```

**特点**：
- 实现 `IPlatform` 接口
- 使用 `virtual` 提供默认实现
- 子类可选择 Override 或使用默认实现
- 使用 `abstract` 强制子类实现 `PlatformName`

---

### 2.3 具体平台实现

#### 2.3.1 DouYinPlatform（抖音）

**文件**：`DouYinPlatform.cs`

```csharp
public class DouYinPlatform : BasePlatform
{
    public override string PlatformName => "抖音";

    // Override: 抖音特殊的消息处理
    public override bool SendMessage(string message) { ... }

    // Override: 抖音视频上传需要额外的封面处理
    public override bool UploadVideo(string filePath) { ... }

    // 不重写 GetUserInfo，使用基类默认实现
}
```

**重写方法**：
- `SendMessage()`：抖音特有的消息处理逻辑
- `UploadVideo()`：包含抖音特有的视频处理（如封面、标签）

**继承方法**：
- `GetUserInfo()`：使用 `BasePlatform` 的默认实现

---

#### 2.3.2 WeChatPlatform（微信）

**文件**：`WeChatPlatform.cs`

```csharp
public class WeChatPlatform : BasePlatform
{
    public override string PlatformName => "微信";

    // Override: 微信特殊的消息处理
    public override bool SendMessage(string message) { ... }

    // Override: 微信视频上传需要企业认证
    public override bool UploadVideo(string filePath) { ... }

    // 不重写 GetUserInfo，使用基类默认实现
}
```

**重写方法**：
- `SendMessage()`：微信特有的消息处理逻辑
- `UploadVideo()`：包含微信企业认证、权限校验等

**继承方法**：
- `GetUserInfo()`：使用 `BasePlatform` 的默认实现

---

### 2.4 PlatformDispatcher（调度器）

**文件**：`PlatformDispatcher.cs`

**职责**：中转脚本，统一分发请求到对应平台

```csharp
public class PlatformDispatcher
{
    private readonly BasePlatform _platform;

    public PlatformDispatcher(BasePlatform platform)
    {
        _platform = platform;
    }

    public bool DispatchSendMessage(string message)
    {
        Console.WriteLine($"[调度器] 正在转发到 {_platform.PlatformName}...");
        return _platform.SendMessage(message);
    }

    public bool DispatchUploadVideo(string filePath) { ... }
    public string DispatchGetUserInfo(string userId) { ... }
}
```

**作用**：
- 统一管理平台实例
- 记录调用日志
- 提供统一的调用入口
- 便于后续添加拦截器、缓存等功能

---

### 2.5 TestRunner（测试脚本）

**文件**：`TestRunner.cs`

**职责**：演示如何使用平台接口

```csharp
public class TestRunner
{
    public static void Main(string[] args)
    {
        // 测试抖音平台
        BasePlatform douYin = new DouYinPlatform();
        PlatformDispatcher douYinDispatcher = new PlatformDispatcher(douYin);
        douYinDispatcher.DispatchSendMessage("Hello 抖音!");

        // 测试微信平台
        BasePlatform weChat = new WeChatPlatform();
        PlatformDispatcher weChatDispatcher = new PlatformDispatcher(weChat);
        weChatDispatcher.DispatchSendMessage("Hello 微信!");
    }
}
```

---

## 三、调用流程

### 3.1 完整流程

```
TestRunner (测试脚本)
    ↓
创建具体平台实例 (new DouYinPlatform() / new WeChatPlatform())
    ↓
传入调度器 (new PlatformDispatcher(platform))
    ↓
调用调度器方法 (DispatchSendMessage / DispatchUploadVideo)
    ↓
调度器转发到平台实例
    ↓
执行平台的 Override 方法 或 基类 virtual 方法
    ↓
返回结果
```

### 3.2 方法调用时序

```
DispatchSendMessage("Hello")
    ↓
_platform.SendMessage("Hello")
    ↓
选择分支：
    ├─ DouYinPlatform → 执行 Override 的 SendMessage()
    └─ WeChatPlatform → 执行 Override 的 SendMessage()
```

---

## 四、Override 设计说明

### 4.1 Override 类型

| 类型 | 说明 | 示例 |
|------|------|------|
| **abstract** | 必须重写 | `PlatformName` 平台标识 |
| **virtual** | 可选重写 | `SendMessage()` 可重写可不重写 |
| **不重写** | 使用默认 | `GetUserInfo()` 直接用基类实现 |

### 4.2 Override 实践

```csharp
// 必须重写（abstract）
public override string PlatformName => "抖音";

// 可选重写（virtual）- 本例中选择重写
public override bool SendMessage(string message)
{
    // 抖音特有逻辑
    Console.WriteLine($"[抖音 Override] 发送消息: {message}");
    return true;
}

// 可选重写（virtual）- 本例中不重写，使用基类默认实现
// GetUserInfo() 直接继承自 BasePlatform
```

### 4.3 优势

- **减少冗余**：不用每个方法都实现
- **灵活定制**：只需 Override 特异化逻辑
- **向后兼容**：新增平台不影响现有代码
- **易于维护**：共同逻辑在基类统一管理

---

## 五、文件结构

```
C#Learn/
├── IPlatform.cs                # 接口定义
├── BasePlatform.cs             # 抽象基类（提供默认实现）
├── DouYinPlatform.cs           # 抖音平台实现
├── WeChatPlatform.cs           # 微信平台实现
├── PlatformDispatcher.cs       # 中转调度器
├── TestRunner.cs               # 测试脚本入口
└── 架构设计文档.md             # 本文档
```

---

## 六、扩展指南

### 6.1 添加新平台

**步骤**：

1. **创建新类**，继承 `BasePlatform`
   ```csharp
   public class TikTokPlatform : BasePlatform
   {
       public override string PlatformName => "TikTok";
       // ... 按需 Override 方法
   }
   ```

2. **实现 PlatformName**
   ```csharp
   public override string PlatformName => "TikTok";
   ```

3. **Override 特异化方法**
   ```csharp
   public override bool SendMessage(string message)
   {
       // TikTok 特有逻辑
       return true;
   }
   ```

4. **在测试脚本中使用**
   ```csharp
   BasePlatform tiktok = new TikTokPlatform();
   PlatformDispatcher dispatcher = new PlatformDispatcher(tiktok);
   dispatcher.DispatchSendMessage("Hello TikTok!");
   ```

### 6.2 添加新方法

**当需要添加新功能时**：

1. 在 `IPlatform` 接口中定义方法签名
   ```csharp
   public interface IPlatform
   {
       // ... 现有方法
       bool PublishPost(string content);  // 新方法
   }
   ```

2. 在 `BasePlatform` 中提供默认实现
   ```csharp
   public virtual bool PublishPost(string content)
   {
       Console.WriteLine($"[{PlatformName}] 发布内容: {content}");
       return true;
   }
   ```

3. 在各平台中选择性 Override（如需定制化）
   ```csharp
   public override bool PublishPost(string content)
   {
       // 抖音特有的发布逻辑
       return true;
   }
   ```

4. 在调度器中添加转发方法
   ```csharp
   public bool DispatchPublishPost(string content)
   {
       Console.WriteLine($"[调度器] 转发到 {_platform.PlatformName}...");
       return _platform.PublishPost(content);
   }
   ```

---

## 七、设计模式说明

### 7.1 策略模式

- **目的**：定义一族算法，将每一个算法封装起来，并让它们可以相互替换
- **应用**：不同平台 = 不同策略，通过调度器统一调用

### 7.2 模板方法模式

- **目的**：定义一个操作中的算法骨架，将步骤延迟到子类
- **应用**：`BasePlatform` 定义骨架，子类 Override 具体步骤

### 7.3 工厂模式（可选扩展）

可为平台创建工厂类，统一管理实例化：

```csharp
public class PlatformFactory
{
    public static BasePlatform CreatePlatform(string platformType)
    {
        return platformType.ToLower() switch
        {
            "douyin" => new DouYinPlatform(),
            "wechat" => new WeChatPlatform(),
            _ => throw new ArgumentException("Unknown platform")
        };
    }
}

// 使用
BasePlatform platform = PlatformFactory.CreatePlatform("douyin");
```

---

## 八、使用示例

### 8.1 基础使用

```csharp
// 创建抖音平台实例
BasePlatform douyin = new DouYinPlatform();

// 创建调度器
PlatformDispatcher dispatcher = new PlatformDispatcher(douyin);

// 调用方法
dispatcher.DispatchSendMessage("你好，抖音!");
dispatcher.DispatchUploadVideo("video.mp4");
string userInfo = dispatcher.DispatchGetUserInfo("12345");
```

### 8.2 切换平台

```csharp
// 从抖音切换到微信
BasePlatform wechat = new WeChatPlatform();
PlatformDispatcher dispatcher2 = new PlatformDispatcher(wechat);

// 相同的调用接口，不同的平台实现
dispatcher2.DispatchSendMessage("你好，微信!");
```

### 8.3 动态选择平台

```csharp
string platformName = GetUserPlatformPreference();  // 从配置获取
BasePlatform platform = platformName switch
{
    "douyin" => new DouYinPlatform(),
    "wechat" => new WeChatPlatform(),
    _ => throw new Exception("未知平台")
};

PlatformDispatcher dispatcher = new PlatformDispatcher(platform);
dispatcher.DispatchSendMessage("动态选择的平台消息");
```

---

## 九、常见问题

### Q1: 为什么需要 PlatformDispatcher？

**A**: 调度器提供以下好处：
- 统一的调用入口
- 便于添加日志、监控、缓存等功能
- 解耦测试脚本和具体平台实现
- 便于后续集成第三方库

### Q2: 什么时候 Override，什么时候不 Override？

**A**: 
- **Override**：当该平台的实现与默认实现不同
- **不 Override**：当该平台的实现与默认实现相同，直接继承

### Q3: 如何添加平台级别的参数？

**A**: 
```csharp
public class DouYinPlatform : BasePlatform
{
    private readonly string _appId;
    private readonly string _appSecret;

    public DouYinPlatform(string appId, string appSecret)
    {
        _appId = appId;
        _appSecret = appSecret;
    }

    public override string PlatformName => "抖音";
    // ... 其他方法
}
```

### Q4: 能否为调度器添加通用的前置/后置处理？

**A**: 可以，使用装饰器模式或拦截器：
```csharp
public class LoggingDispatcher : PlatformDispatcher
{
    public override bool DispatchSendMessage(string message)
    {
        Console.WriteLine($"[前置] 发送消息前处理");
        bool result = base.DispatchSendMessage(message);
        Console.WriteLine($"[后置] 发送完成，结果: {result}");
        return result;
    }
}
```

---

## 十、总结

| 层级 | 组件 | 职责 |
|------|------|------|
| **接口层** | `IPlatform` | 定义契约 |
| **基类层** | `BasePlatform` | 提供默认实现 |
| **平台层** | `DouYinPlatform`<br/>`WeChatPlatform` | 按需 Override |
| **调度层** | `PlatformDispatcher` | 统一转发 |
| **应用层** | `TestRunner` | 使用和测试 |

该架构具有以下优势：
- ✅ 代码复用性高
- ✅ 扩展性强
- ✅ 维护性好
- ✅ 测试友好
- ✅ 灵活可定制

---

**版本**：1.0  
**最后更新**：2026-04-28  
**作者**：GitHub Copilot
