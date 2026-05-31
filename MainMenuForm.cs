using System;
using System.Drawing;
using System.Windows.Forms;

namespace BookManagementSystem
{
    /// <summary>
    /// 主菜单窗体（单例模式）
    /// </summary>
    public partial class MainMenuForm : Form
    {
        private static MainMenuForm instance;

        private Label lblTitle;
        private Button btnCategory;    // 读者类别
        private Button btnReader;      // 读者管理
        private Button btnBook;        // 图书管理
        private Button btnBorrRet;     // 借书还书
        private Button btnExit;        // 退出系统

        public MainMenuForm()
        {
            InitializeComponent();
            instance = this;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "图书管理系统 - 主菜单";
            this.Size = new Size(500, 420);
        }

        public static MainMenuForm GetInstance()
        {
            return instance;
        }

        private void InitializeComponent()
        {
            // 标题
            lblTitle = new Label
            {
                Text = "图书管理系统",
                Font = new Font("微软雅黑", 20, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Size = new Size(350, 50),
                Location = new Point(75, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            int btnWidth = 160;
            int btnHeight = 50;
            int leftCol = 60;
            int rightCol = 270;
            int startY = 110;
            int gapY = 70;

            // 读者类别
            btnCategory = CreateMenuButton("读者类别", leftCol, startY, btnWidth, btnHeight);
            btnCategory.Click += BtnCategory_Click;

            // 图书管理
            btnBook = CreateMenuButton("图书管理", rightCol, startY, btnWidth, btnHeight);
            btnBook.Click += BtnBook_Click;

            // 读者管理
            btnReader = CreateMenuButton("读者管理", leftCol, startY + gapY, btnWidth, btnHeight);
            btnReader.Click += BtnReader_Click;

            // 借书还书
            btnBorrRet = CreateMenuButton("借书还书", rightCol, startY + gapY, btnWidth, btnHeight);
            btnBorrRet.Click += BtnBorrRet_Click;

            // 退出系统
            btnExit = CreateMenuButton("退出系统", 160, startY + gapY * 2, btnWidth, btnHeight);
            btnExit.BackColor = Color.LightCoral;
            btnExit.Click += BtnExit_Click;

            Controls.AddRange(new Control[]
            {
                lblTitle, btnCategory, btnBook, btnReader, btnBorrRet, btnExit
            });
        }

        private Button CreateMenuButton(string text, int x, int y, int w, int h)
        {
            return new Button
            {
                Text = text,
                Font = new Font("微软雅黑", 11, FontStyle.Bold),
                Size = new Size(w, h),
                Location = new Point(x, y),
                BackColor = Color.LightSteelBlue,
                FlatStyle = FlatStyle.Flat
            };
        }

        private void BtnCategory_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (ReaderCategoryForm form = new ReaderCategoryForm())
            {
                form.ShowDialog();
            }
            this.Show();
        }

        private void BtnBook_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (BookManagementForm form = new BookManagementForm())
            {
                form.ShowDialog();
            }
            this.Show();
        }

        private void BtnReader_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (ReaderManagementForm form = new ReaderManagementForm())
            {
                form.ShowDialog();
            }
            this.Show();
        }

        private void BtnBorrRet_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (BorrAndRetForm form = new BorrAndRetForm())
            {
                form.ShowDialog();
            }
            this.Show();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出系统吗？", "确认", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
