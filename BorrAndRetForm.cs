using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BookManagementSystem
{
    /// <summary>
    /// 借书还书管理窗体
    /// </summary>
    public partial class BorrAndRetForm : Form
    {
        private Label lblRdID;
        private Label lblBkID;
        private TextBox txtRdID;
        private TextBox txtBkID;
        private Button btnBackMenu;
        private Button btnBorrow;
        private Button btnReturn;
        private Button btnRefresh;
        private DataGridView dgvData;

        public BorrAndRetForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "借书还书管理";
            this.Size = new Size(820, 520);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void InitializeComponent()
        {
            int leftX = 30;
            int inputLeftX = 110;
            int startY = 30;
            int gapY = 45;

            // 读者编号
            lblRdID = new Label
            {
                Text = "读者编号：",
                Font = new Font("微软雅黑", 9),
                Size = new Size(80, 25),
                Location = new Point(leftX, startY),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtRdID = new TextBox
            {
                Font = new Font("微软雅黑", 9),
                Size = new Size(120, 25),
                Location = new Point(inputLeftX, startY)
            };

            // 书号
            lblBkID = new Label
            {
                Text = "书　　号：",
                Font = new Font("微软雅黑", 9),
                Size = new Size(80, 25),
                Location = new Point(leftX, startY + gapY),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtBkID = new TextBox
            {
                Font = new Font("微软雅黑", 9),
                Size = new Size(120, 25),
                Location = new Point(inputLeftX, startY + gapY)
            };

            // 按钮
            int btnY = startY + gapY * 2 + 10;
            btnBackMenu = new Button
            {
                Text = "返回主菜单",
                Font = new Font("微软雅黑", 9),
                Size = new Size(105, 35),
                Location = new Point(leftX, btnY),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightSteelBlue
            };
            btnBackMenu.Click += BtnBackMenu_Click;

            btnBorrow = new Button
            {
                Text = "借书",
                Font = new Font("微软雅黑", 9, FontStyle.Bold),
                Size = new Size(90, 35),
                Location = new Point(leftX + 115, btnY),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightGreen
            };
            btnBorrow.Click += BtnBorrow_Click;

            btnReturn = new Button
            {
                Text = "还书",
                Font = new Font("微软雅黑", 9, FontStyle.Bold),
                Size = new Size(90, 35),
                Location = new Point(leftX, btnY + 45),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightCoral
            };
            btnReturn.Click += BtnReturn_Click;

            btnRefresh = new Button
            {
                Text = "刷新",
                Font = new Font("微软雅黑", 9),
                Size = new Size(90, 35),
                Location = new Point(leftX + 115, btnY + 45),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightGray
            };
            btnRefresh.Click += BtnRefresh_Click;

            // DataGridView
            dgvData = new DataGridView
            {
                Location = new Point(280, 20),
                Size = new Size(520, 440),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvData.SelectionChanged += DgvData_SelectionChanged;

            Controls.AddRange(new Control[]
            {
                lblRdID, txtRdID, lblBkID, txtBkID,
                btnBackMenu, btnBorrow, btnReturn, btnRefresh, dgvData
            });

            DataBind();
        }

        /// <summary>
        /// 加载借阅数据
        /// </summary>
        private void DataBind()
        {
            try
            {
                string sql = @"SELECT rdID AS '读者编号', bkID AS '书号', 
                               DateBorrow AS '借书日期', DateLendPlan AS '应还日期' FROM Borrow";
                DataTable dt = DBHelper.ExecuteQuery(sql);
                dgvData.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据失败！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null && dgvData.CurrentRow.Index >= 0)
            {
                DataGridViewRow row = dgvData.CurrentRow;
                txtRdID.Text = row.Cells["读者编号"].Value == null ? "" : row.Cells["读者编号"].Value.ToString();
                txtBkID.Text = row.Cells["书号"].Value == null ? "" : row.Cells["书号"].Value.ToString();
            }
        }

        private void ClearInputs()
        {
            txtRdID.Text = "";
            txtBkID.Text = "";
        }

        private void BtnBackMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 借书按钮 - 调用存储过程 usp_BorrowBook
        /// </summary>
        private void BtnBorrow_Click(object sender, EventArgs e)
        {
            string rdID = txtRdID.Text.Trim();
            string bkID = txtBkID.Text.Trim();

            if (string.IsNullOrEmpty(rdID))
            {
                MessageBox.Show("请输入读者编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRdID.Focus();
                return;
            }
            if (string.IsNullOrEmpty(bkID))
            {
                MessageBox.Show("请输入书号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBkID.Focus();
                return;
            }

            try
            {
                // 使用存储过程借书
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("usp_BorrowBook", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_rdID", rdID);
                        cmd.Parameters.AddWithValue("@p_bkID", bkID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("借书成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBind();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("借书失败！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 还书按钮 - 调用存储过程 usp_ReturnBook
        /// </summary>
        private void BtnReturn_Click(object sender, EventArgs e)
        {
            string rdID = txtRdID.Text.Trim();
            string bkID = txtBkID.Text.Trim();

            if (string.IsNullOrEmpty(rdID))
            {
                MessageBox.Show("请输入读者编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRdID.Focus();
                return;
            }
            if (string.IsNullOrEmpty(bkID))
            {
                MessageBox.Show("请输入书号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBkID.Focus();
                return;
            }

            try
            {
                // 使用存储过程还书
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("usp_ReturnBook", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_rdID", rdID);
                        cmd.Parameters.AddWithValue("@p_bkID", bkID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("还书成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBind();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("还书失败！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            DataBind();
        }
    }
}
