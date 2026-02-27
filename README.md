# DQSM WeChat Interaction System

## 1. 简介
这是一个基于 .NET 8 MVC 的微信互动消息管理系统。采用 Code-First 模式，集成了 MySQL 8.0 数据库，并实现了全局 AOP 日志记录。

## How to run

使用 Docker Compose 一键启动所有服务：

```bash
# 构建并启动所有服务
docker-compose up -d --build

# 查看服务运行状态
docker-compose ps

# 查看服务日志
docker-compose logs -f

# 停止所有服务
docker-compose down

# 停止并删除数据卷（会清除数据库数据）
docker-compose down -v
```

## Services

| 服务名称 | 描述 | 端口映射 | 访问地址 |
|---------|------|---------|---------|
| **mysql** | MySQL 数据库服务 | 内部端口 | 仅供内部服务访问 |
| **backend** | Spring Boot 后端 API 服务 | 9999:8080 | http://localhost:9999 |
| **frontend-admin** | Vue 3 管理后台（Nginx 托管） | 8082:80 | http://localhost:8082 |
| **frontend-mp** | 微信小程序用户端 | - | 微信开发者工具中打开 |

> 💡 **注意**：`frontend-mp` 为微信小程序项目，不通过 Docker 部署。请使用 [微信开发者工具](https://developers.weixin.qq.com/miniprogram/dev/devtools/download.html) 打开 `frontend-mp` 目录进行开发和预览。

## 题目内容
    创建一个.net 8 项目，要求如下：1.连接本地mysql 8.0.43数据库，数据库名称dqsm，用户名称dqsm_user，用户密码:Sjy092150+2.使用mvc的方式创建一个模块：用于接收、发送微信公众号用户的互动信息，发送模板信息。需要建立2张表格，分别是userMsg（id、fromUser、toUser、message、dateTime）和templateMsg（id、fromUser、toUser、message、dateTime）。3.创建一个静态日志服务，用户记录日志，命名为Logs表，记录当前action名称、信息类型（info、debug、error）、信息内容、日期。字段分别为id、action、infoType、info、dateTime。使用codefirst方式创建表。4.在每个action入参时需要记录参数值、操作结果、返回值，均需记录日志。

**项目结构 (Project Structure)**
*   `backend/`: 后端核心服务 (.NET 8 MVC / API)。
*   `docs/`: 项目设计文档与架构图。
*   `docker-compose.yml`: 容器编排文件。

## 2. 核心功能
1.  **微信互动模块**：
    *   接收用户消息 (`UserMsg` 表)。
    *   发送模板消息 (`TemplateMsg` 表)。
2.  **静态日志服务**：
    *   全局静态调用入口 `StaticLogService.Info/Error`。
    *   持久化到 MySQL `Logs` 表。
3.  **全局审计 (AOP)**：
    *   自动拦截所有 Action。
    *   记录入参、返回值、执行时间、异常信息。

## 3. 快速开始 (Run It)

推荐使用 **Docker 模式** 一键启动。

### 前置要求
*   Docker & Docker Compose

### 启动步骤
1.  **启动服务**:
    在项目根目录下运行：
    ```bash
    docker-compose up -d
    ```
    *(如果遇到镜像拉取网络问题，请尝试运行 `./force_pull.sh` 脚本)*

2.  **访问服务**:
    *   **后端看板**: [http://localhost:9999](http://localhost:9999)
    *   **日志看板**: [http://localhost:9999/Logs](http://localhost:9999/Logs)
    *   **MySQL 数据库**: `localhost:3307`
        *   用户: `dqsm_user`
        *   密码: `Sjy092150+`

3.  **停止服务**:
    ```bash
    docker-compose down
    ```

## 4. 本地开发 (可选)
如果需要本地调试代码（非 Docker 运行）：
1.  安装 .NET 8 SDK 和 MySQL 8.0。
2.  修改 `backend/appsettings.json` 中的数据库连接字符串（指向本地数据库）。
3.  在 `backend` 目录下运行 `dotnet run`。

## 5. 常见问题
*   **端口冲突**: 默认后端使用 `9999`，数据库外部端口使用 `3307`。如果冲突请修改 `docker-compose.yml`。
*   **镜像拉取失败**: 请使用项目根目录下的 `force_pull.sh` 脚本，它会自动尝试国内镜像源。
