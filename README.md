# 图书管理系统 (BookManagementSystem)

基于 **C# WinForms + MySQL** 的图书管理系统，支持用户注册登录、读者类别管理、读者管理、图书管理、借书还书等功能。

## 开发环境

| 项目 | 说明 |
| --- | --- |
| 编程语言 | C# |
| 框架 | .NET Framework 4.0 |
| UI 技术 | Windows Forms |
| 数据库 | MySQL 8.0 |
| 数据库驱动 | MySQL Connector/NET (MySql.Data.dll) |
| 编译器 | csc.exe (C# Compiler) |

## 功能模块

### 🔐 登录与注册

- 用户登录验证（区分"用户不存在"与"密码错误"）
- 新用户注册（含密码确认）
- 默认管理员账号：`admin` / `123456`

### 📋 主菜单

- 读者类别 / 图书管理 / 读者管理 / 借书还书 / 退出系统

### 📁 读者类别管理

- 字段：类别编号、类别名称、可借书数量、可借书天数
- 支持：添加、查询（按名称）、删除、修改
- 选中表格行自动回填到编辑框

### 👤 读者管理

- 字段：读者编号、类别号、姓名、单位、QQ、已借书数量
- 支持：添加、查询（按单位）、删除、修改

### 📚 图书管理

- 字段：书号、书名、作者、出版社、单价、状态（在馆/不在馆）
- 支持：添加、查询（按书名）、删除、修改

### 🔄 借书还书

- 借书：自动验证图书存在性、在馆状态、读者借阅上限，计算应还日期
- 还书：验证借阅记录，自动恢复图书状态与读者借书数量
- 使用 MySQL **存储过程**保证数据一致性

## 项目结构

```
BookManagementSystem/
├── bin/                          # 编译输出
│   ├── BookManagementSystem.exe  # 主程序
│   └── MySql.Data.dll            # MySQL 驱动
├── sql/
│   └── init_database.sql         # 数据库初始化脚本
├── Program.cs                    # 程序入口
├── DBHelper.cs                   # 数据库帮助类
├── LoginForm.cs                  # 登录窗体
├── RegisterForm.cs               # 注册窗体
├── MainMenuForm.cs               # 主菜单窗体
├── ReaderCategoryForm.cs         # 读者类别管理
├── ReaderManagementForm.cs       # 读者管理
├── BookManagementForm.cs         # 图书管理
├── BorrAndRetForm.cs             # 借书还书
├── build.py                      # 编译脚本
└── README.md                     # 项目文档
```

## 数据库设计 (MySQL)

### 数据表

| 表名 | 说明 | 主要字段 |
| --- | --- | --- |
| `login` | 用户登录 | Id (PK), UserName, Password |
| `readertype` | 读者类别 | rdType (PK), rdTypeName, canLendQty, canLendDay |
| `reader` | 读者信息 | rdID (PK), rdType (FK), rdName, rdDept, rdQQ, rdBorrowQty |
| `book` | 图书信息 | bkID (PK), bkName, bkAuthor, bkPress, bkPrice, bkStatus |
| `borrow` | 借阅记录 | rdID, bkID (复合PK), DateBorrow, DateLendPlan |

### 存储过程

- **`usp_BorrowBook(rdID, bkID)`** — 借书：校验图书/读者存在性→在馆状态→数量上限→执行借阅
- **`usp_ReturnBook(rdID, bkID)`** — 还书：校验借阅记录→恢复状态→删除记录

## 快速开始

### 1. 初始化数据库

```bash
mysql -u root -p < sql/init_database.sql
```


### 2. 编译项目

```bash
python build.py
```

### 3. 运行程序

```bash
bin\BookManagementSystem.exe
```

## 示例数据

| 读者类别 | 可借数量 | 可借天数 |
| --- | --- | --- |
| 学生 | 5 | 30 |
| 教师 | 10 | 60 |
| 职工 | 3 | 30 |
| 书号 | 书名 | 作者 | 出版社 | 单价 |
| ------ | ------ | ------ | -------- | ------ |
| B00000001 | C#程序设计 | 张三丰 | 清华大学出版社 | 59.80 |
| B00000002 | 数据库原理 | 李四光 | 高等教育出版社 | 45.00 |
| B00000003 | 计算机网络 | 王五城 | 人民邮电出版社 | 39.90 |
| B00000004 | 操作系统概论 | 赵六安 | 机械工业出版社 | 49.00 |
| B00000005 | 数据结构与算法 | 钱七 | 电子工业出版社 | 69.00 |

## 关键技术点

- **参数化查询**：所有SQL操作使用 `MySqlParameter`，防止SQL注入
- **存储过程**：借书还书使用MySQL存储过程实现多表事务操作
- **DBHelper 封装**：静态帮助类统一管理数据库连接与操作
- **窗体导航**：Hide/Show模式实现窗体间无缝切换
- **DataGridView 绑定**：表格数据自动绑定，选中行回填编辑框



---
