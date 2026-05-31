using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace BookManagementSystem
{
    /// <summary>
    /// MySQL数据库帮助类
    /// </summary>
    public static class DBHelper
    {
        // MySQL连接字符串（原SQL Server改为MySQL）
        public static readonly string ConnStr =
            "Server=localhost;Database=BookDB;Uid=root;Pwd=123456;Charset=utf8mb4;";

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnStr);
        }

        /// <summary>
        /// 执行非查询SQL（INSERT/UPDATE/DELETE）
        /// </summary>
        public static int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行查询并返回DataTable
        /// </summary>
        public static DataTable ExecuteQuery(string sql, params MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 执行查询返回单个值
        /// </summary>
        public static object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
