# BF2Statistics Services

### 现有服务模块（9个）

1. **AwardsProcessingService.cs** - 处理奖章和徽章
   - 负责BF2和SF版本的奖章处理
   - 根据游戏数据保存奖章到数据库

2. **DataInitializationService.cs** - 初始化测试数据
   - 初始化Armies、Kits、Vehicles、Weapons、MapInfos、Maps、Awards、Kills等测试数据
   - 检查数据是否已存在，避免重复初始化

3. **DatabaseInitializer.cs** - 数据库初始化和IP2Nation数据
   - 创建数据库并初始化IP2Nation数据
   - 处理IP地理位置映射数据

4. **GameSessionService.cs** - 游戏会话管理
   - 保存GameSpy统计数据到数据库
   - 管理服务器IP、端口、地图详情、获胜方、游戏模式等信息

5. **GameSpyConnectionHandler.cs** - GameSpy连接处理
   - 处理GameSpy TCP连接
   - 记录连接信息和接收的数据
   - 处理传入的TCP数据

6. **PlayerDataProcessor.cs** - 玩家数据和击杀关系处理
   - 处理玩家数据，特别是击杀关系
   - 解析GameSpy数据并更新数据库中的武器击杀数据

7. **ResponseFormatService.cs** - BF2响应格式化（静态类）
   - 提供标准化的BF2统计响应格式
   - 确保与原始BF2客户端和工具的兼容性

8. **SmocGenCheckService.cs** - SMOC和GEN奖章检查
   - 检查并更新SMOC (Sergeant Major Of The Corps) - 每月第一天执行
   - 处理特殊军衔奖章的分配

9. **StatisticsService.cs** - 主要统计数据处理
   - 处理游戏数据的核心服务
   - 注册/更新服务器信息、处理玩家数据、军队数据、地图数据


### 1. 服务分层重构
```
Controller Layer
    ↓
Business Logic Layer (统计处理、奖章逻辑)
    ↓
Data Processing Layer (数据解析、验证)
    ↓
Data Access Layer (数据库操作)
```

## 📁 项目结构

```
BF2Statistics/
├── Controllers/          # API控制器
├── Data/                 # 数据库上下文
├── Models/               # 数据模型
├── Services/             # 业务逻辑服务
├── Migrations/           # 数据库迁移
└── README.md            # 项目文档
```

## 🚀 总结

整体而言，Services目录的架构基本合理，但存在一些重复代码和缺失功能需要优化。建议按优先级逐步改进，以提升系统的可维护性和性能。
