using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BookManagementSystem
{
    /// <summary>
    /// 注册窗体
    /// </summary>
    public partial class RegisterForm : Form
    {
        private Label lblTitle;
        private Label lblUserName;
        private Label lblPassword;
        private Label lblConfirmPwd;
        private TextBox txtUserName;
        private TextBox txtPassword;
        private TextBox txtConfirmPwd;
        private Button btnRegister;
        private Button btnCancel;

        public RegisterForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "图书管理系统 - 注册";
            this.Size = new Size(420, 360);
        }

        private void InitializeComponent()
        {
            // 标题
            lblTitle = new Label
            {
                Text = "用户注册",
                Font = new Font("微软雅黑", 16, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Size = new Size(200, 40),
                Location = new Point(110, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 用户名
            lblUserName = new Label
            {
                Text = "用户名：",
                Font = new Font("微软雅黑", 10),
                Size = new Size(75, 25),
                Location = new Point(55, 80),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtUserName = new TextBox
            {
                Font = new Font("微软雅黑", 10),
                Size = new Size(180, 25),
                Location = new Point(135, 80)
            };

            // 密码
            lblPassword = new Label
            {
                Text = "密　码：",
                Font = new Font("微软雅黑", 10),
                Size = new Size(75, 25),
                Location = new Point(55, 125),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtPassword = new TextBox
            {
                Font = new Font("微软雅黑", 10),
                Size = new Size(180, 25),
                Location = new Point(135, 125),
                PasswordChar = '*'
            };

            // 确认密码
            lblConfirmPwd = new Label
            {
                Text = "确认密码：",
                Font = new Font("微软雅黑", 10),
                Size = new Size(75, 25),
                Location = new Point(55, 170),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtConfirmPwd = new TextBox
            {
                Font = new Font("微软雅黑", 10),
                Size = new Size(180, 25),
                Location = new Point(135, 170),
                PasswordChar = '*'
            };

            // 注册按钮
            btnRegister = new Button
            {
                Text = "注册",
                Font = new Font("微软雅黑", 10, FontStyle.Bold),
                Size = new Size(90, 35),
                Location = new Point(95, 230),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            btnRegister.Click += BtnRegister_Click;

            // 取消按钮
            btnCancel = new Button
            {
                Text = "取消",
                Font = new Font("微软雅黑", 10, FontStyle.Bold),
                Size = new Size(90, 35),
                Location = new Point(210, 230),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += BtnCancel_Click;

            // 添加到窗体
            Controls.AddRange(new Control[]
            {
                lblTitle, lblUserName, txtUserName,
                lblPassword, txtPassword, lblConfirmPwd, txtConfirmPwd,
                btnRegister, btnCancel
            });

            this.AcceptButton = btnRegister;
        }

        /// <summary>
        /// 注册按钮单击事件
        /// </summary>
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text;
            string confirmPwd = txtConfirmPwd.Text;

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("请输入用户名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }
            if (password != confirmPwd)
            {
                MessageBox.Show("两次输入的密码不一致！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPwd.Focus();
                return;
            }

            try
            {
                // 检查用户名是否已存在
                string checkSql = "SELECT COUNT(*) FROM Login WHERE UserName = @UserName";
                object exists = DBHelper.ExecuteScalar(checkSql,
                    new MySqlParameter("@UserName", userName));

                if (Convert.ToInt32(exists) > 0)
                {
                    MessageBox.Show("该用户名已被注册！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 插入新用户
                string insertSql = "INSERT INTO Login(UserName, Password) VALUES(@UserName, @Password)";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@UserName", userName),
                    new MySqlParameter("@Password", password)
                };

                int result = DBHelper.ExecuteNonQuery(insertSql, parameters);
                if (result > 0)
                {
                    MessageBox.Show("注册成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // 关闭注册窗体，返回登录窗体
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作数据库出错！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消按钮单击事件 - 返回登录窗体
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 注册窗体关闭后显示登录窗体
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            // 查找并显示登录窗体
            foreach (Form f in Application.OpenForms)
            {
                if (f is LoginForm)
                {
                    f.Show();
                    break;
                }
            }
        }
    }
}
