using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BookManagementSystem
{
    /// <summary>
    /// 登录窗体
    /// </summary>
    public partial class LoginForm : Form
    {
        private Label lblTitle;
        private Label lblUserName;
        private Label lblPassword;
        private TextBox txtUserName;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;

        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "图书管理系统 - 登录";
            this.Size = new Size(420, 320);
        }

        private void InitializeComponent()
        {
            // 标题
            lblTitle = new Label
            {
                Text = "图书管理系统登录",
                Font = new Font("微软雅黑", 16, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Size = new Size(280, 40),
                Location = new Point(70, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 用户名
            lblUserName = new Label
            {
                Text = "用户名：",
                Font = new Font("微软雅黑", 10),
                Size = new Size(75, 25),
                Location = new Point(70, 85),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtUserName = new TextBox
            {
                Font = new Font("微软雅黑", 10),
                Size = new Size(180, 25),
                Location = new Point(150, 85)
            };

            // 密码
            lblPassword = new Label
            {
                Text = "密　码：",
                Font = new Font("微软雅黑", 10),
                Size = new Size(75, 25),
                Location = new Point(70, 135),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtPassword = new TextBox
            {
                Font = new Font("微软雅黑", 10),
                Size = new Size(180, 25),
                Location = new Point(150, 135),
                PasswordChar = '*'
            };

            // 登录按钮
            btnLogin = new Button
            {
                Text = "登录",
                Font = new Font("微软雅黑", 10, FontStyle.Bold),
                Size = new Size(90, 35),
                Location = new Point(95, 200),
                BackColor = Color.LightSteelBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.Click += BtnLogin_Click;

            // 注册按钮
            btnRegister = new Button
            {
                Text = "注册",
                Font = new Font("微软雅黑", 10, FontStyle.Bold),
                Size = new Size(90, 35),
                Location = new Point(210, 200),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            btnRegister.Click += BtnRegister_Click;

            // 添加到窗体
            Controls.AddRange(new Control[]
            {
                lblTitle, lblUserName, txtUserName,
                lblPassword, txtPassword, btnLogin, btnRegister
            });

            // 回车键触发登录
            this.AcceptButton = btnLogin;
        }

        /// <summary>
        /// 登录按钮单击事件
        /// </summary>
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text;

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

            try
            {
                string sql = "SELECT COUNT(*) FROM Login WHERE UserName = @UserName AND Password = @Password";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@UserName", userName),
                    new MySqlParameter("@Password", password)
                };

                object result = DBHelper.ExecuteScalar(sql, parameters);
                int count = Convert.ToInt32(result);

                if (count > 0)
                {
                    // 登录成功，隐藏当前窗体，显示主菜单
                    this.Hide();
                    MainMenuForm mainMenu = new MainMenuForm();
                    mainMenu.ShowDialog();
                    this.Close();
                }
                else
                {
                    // 判断是用户不存在还是密码错误
                    string checkUserSql = "SELECT COUNT(*) FROM Login WHERE UserName = @UserName";
                    object userExists = DBHelper.ExecuteScalar(checkUserSql,
                        new MySqlParameter("@UserName", userName));

                    if (Convert.ToInt32(userExists) == 0)
                    {
                        MessageBox.Show("用户不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("密码错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作数据库出错！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 注册按钮单击事件 - 跳转到注册窗体
        /// </summary>
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        /// <summary>
        /// 窗体关闭事件 - 注册窗体关闭后重新显示登录窗体
        /// </summary>
        public void ShowLoginForm()
        {
            this.Show();
        }
    }
}
