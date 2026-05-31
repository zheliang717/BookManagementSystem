-- ============================================
-- 图书管理系统 - MySQL数据库初始化脚本
-- 原项目使用SQL Server，此处改为MySQL
-- ============================================

CREATE DATABASE IF NOT EXISTS BookDB
  DEFAULT CHARACTER SET utf8mb4
  DEFAULT COLLATE utf8mb4_unicode_ci;

USE BookDB;

-- 1. 登录表
CREATE TABLE IF NOT EXISTS Login (
    Id       INT          NOT NULL AUTO_INCREMENT COMMENT '用户编号（主键）',
    UserName VARCHAR(25)  NOT NULL COMMENT '用户名称',
    Password VARCHAR(25)  NOT NULL COMMENT '用户密码',
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 2. 图书表
CREATE TABLE IF NOT EXISTS Book (
    bkID     CHAR(9)      NOT NULL COMMENT '图书编号（主键）',
    bkName   VARCHAR(50)  NULL COMMENT '图书名称',
    bkAuthor VARCHAR(50)  NULL COMMENT '作者',
    bkPress  VARCHAR(50)  NULL COMMENT '出版社',
    bkPrice  DECIMAL(10,2) NULL COMMENT '单价',
    bkStatus INT          NULL DEFAULT 1 COMMENT '是否在馆，1：在馆，0：不在馆',
    PRIMARY KEY (bkID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 3. 读者类别表
CREATE TABLE IF NOT EXISTS ReaderType (
    rdType     INT         NOT NULL COMMENT '读者类别编号（主键）',
    rdTypeName VARCHAR(20) NULL COMMENT '读者类别名称',
    canLendQty INT         NULL COMMENT '可借书数量',
    canLendDay INT         NULL COMMENT '可借书天数',
    PRIMARY KEY (rdType)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 4. 读者表
CREATE TABLE IF NOT EXISTS Reader (
    rdID        CHAR(9)     NOT NULL COMMENT '读者编号（主键）',
    rdType      INT         NULL COMMENT '读者类别编号',
    rdName      VARCHAR(25) NULL COMMENT '读者姓名',
    rdDept      VARCHAR(40) NULL COMMENT '读者单位',
    rdQQ        VARCHAR(25) NULL COMMENT '读者QQ',
    rdBorrowQty INT         NULL DEFAULT 0 COMMENT '已借书数量',
    PRIMARY KEY (rdID),
    FOREIGN KEY (rdType) REFERENCES ReaderType(rdType)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 5. 借书表
CREATE TABLE IF NOT EXISTS Borrow (
    rdID         CHAR(9)  NOT NULL COMMENT '读者编号',
    bkID         CHAR(9)  NOT NULL COMMENT '图书编号',
    DateBorrow   DATETIME NULL COMMENT '借书日期',
    DateLendPlan DATETIME NULL COMMENT '应还日期',
    PRIMARY KEY (rdID, bkID),
    FOREIGN KEY (rdID) REFERENCES Reader(rdID),
    FOREIGN KEY (bkID) REFERENCES Book(bkID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================
-- 存储过程：借书
-- ============================================
DELIMITER //

CREATE PROCEDURE IF NOT EXISTS usp_BorrowBook(
    IN p_rdID CHAR(9),
    IN p_bkID CHAR(9)
)
BEGIN
    DECLARE v_bookExists INT;
    DECLARE v_readerExists INT;
    DECLARE v_bookStatus INT;
    DECLARE v_borrowedQty INT;
    DECLARE v_maxQty INT;
    DECLARE v_lendDays INT;

    -- 检查图书是否存在
    SELECT COUNT(*) INTO v_bookExists FROM Book WHERE bkID = p_bkID;
    IF v_bookExists = 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = '该书号不存在！';
    END IF;

    -- 检查读者是否存在
    SELECT COUNT(*) INTO v_readerExists FROM Reader WHERE rdID = p_rdID;
    IF v_readerExists = 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = '该读者编号不存在！';
    END IF;

    -- 检查图书是否在馆
    SELECT bkStatus INTO v_bookStatus FROM Book WHERE bkID = p_bkID;
    IF v_bookStatus = 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = '抱歉！该书已被借出！';
    END IF;

    -- 检查读者借书数量是否达到上限
    SELECT rdBorrowQty INTO v_borrowedQty FROM Reader WHERE rdID = p_rdID;
    SELECT canLendQty, canLendDay INTO v_maxQty, v_lendDays
        FROM ReaderType WHERE rdType = (SELECT rdType FROM Reader WHERE rdID = p_rdID);

    IF v_borrowedQty >= v_maxQty THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = '借书数量已达上限！';
    END IF;

    -- 执行借书：更新图书状态、读者已借数量、插入借书记录
    UPDATE Book SET bkStatus = 0 WHERE bkID = p_bkID;
    UPDATE Reader SET rdBorrowQty = rdBorrowQty + 1 WHERE rdID = p_rdID;
    INSERT INTO Borrow(rdID, bkID, DateBorrow, DateLendPlan)
        VALUES(p_rdID, p_bkID, NOW(), DATE_ADD(NOW(), INTERVAL v_lendDays DAY));
END //

-- ============================================
-- 存储过程：还书
-- ============================================
CREATE PROCEDURE IF NOT EXISTS usp_ReturnBook(
    IN p_rdID CHAR(9),
    IN p_bkID CHAR(9)
)
BEGIN
    DECLARE v_borrowed INT;

    -- 检查读者是否借过该书
    SELECT COUNT(*) INTO v_borrowed FROM Borrow WHERE rdID = p_rdID AND bkID = p_bkID;
    IF v_borrowed = 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = '抱歉！您暂时没有借过这本书！';
    END IF;

    -- 执行还书：更新图书状态、读者已借数量、删除借书记录
    UPDATE Book SET bkStatus = 1 WHERE bkID = p_bkID;
    UPDATE Reader SET rdBorrowQty = rdBorrowQty - 1 WHERE rdID = p_rdID;
    DELETE FROM Borrow WHERE rdID = p_rdID AND bkID = p_bkID;
END //

DELIMITER ;

-- ============================================
-- 示例数据
-- ============================================

-- 读者类别
INSERT INTO ReaderType(rdType, rdTypeName, canLendQty, canLendDay) VALUES
(1, '学生', 5, 30),
(2, '教师', 10, 60),
(3, '职工', 3, 30);

-- 读者
INSERT INTO Reader(rdID, rdType, rdName, rdDept, rdQQ, rdBorrowQty) VALUES
('R001', 1, '张三', '计算机学院', '123456789', 0),
('R002', 1, '李四', '数学学院', '987654321', 0),
('R003', 2, '王五', '物理学院', '555666777', 0);

-- 图书
INSERT INTO Book(bkID, bkName, bkAuthor, bkPress, bkPrice, bkStatus) VALUES
('B00000001', 'C#程序设计', '张三丰', '清华大学出版社', 59.80, 1),
('B00000002', '数据库原理', '李四光', '高等教育出版社', 45.00, 1),
('B00000003', '计算机网络', '王五城', '人民邮电出版社', 39.90, 1),
('B00000004', '操作系统概论', '赵六安', '机械工业出版社', 49.00, 1),
('B00000005', '数据结构与算法', '钱七', '电子工业出版社', 69.00, 1);

-- 管理员账号
INSERT INTO Login(UserName, Password) VALUES ('admin', '123456');
