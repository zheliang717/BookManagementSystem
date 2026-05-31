using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BookManagementSystem
{
    /// <summary>
    /// 图书管理窗体
    /// </summary>
    public partial class BookManagementForm : Form
    {
        private Label lblBkID;
        private Label lblBkName;
        private Label lblBkAuthor;
        private Label lblBkPress;
        private Label lblBkPrice;
        private Label lblBkStatus;
        private TextBox txtBkID;
        private TextBox txtBkName;
        private TextBox txtBkAuthor;
        private TextBox txtBkPress;
        private TextBox txtBkPrice;
        private TextBox txtBkStatus;
        private Button btnBackMenu;
        private Button btnAdd;
        private Button btnQuery;
        private Button btnDelete;
        private Button btnAlter;
        private DataGridView dgvData;

        public BookManagementForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "图书管理";
            this.Size = new Size(850, 540);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void InitializeComponent()
        {
            int leftX = 20;
            int inputLeftX = 110;
            int startY = 20;
            int gapY = 36;

            // 书号
            lblBkID = CreateLabel("书　号：", leftX, startY);
            txtBkID = CreateTextBox(inputLeftX, startY);
            // 书名
            lblBkName = CreateLabel("书　名：", leftX, startY + gapY);
            txtBkName = CreateTextBox(inputLeftX, startY + gapY);
            // 作者
            lblBkAuthor = CreateLabel("作　者：", leftX, startY + gapY * 2);
            txtBkAuthor = CreateTextBox(inputLeftX, startY + gapY * 2);
            // 出版社
            lblBkPress = CreateLabel("出版社：", leftX, startY + gapY * 3);
            txtBkPress = CreateTextBox(inputLeftX, startY + gapY * 3);
            // 单价
            lblBkPrice = CreateLabel("单　价：", leftX, startY + gapY * 4);
            txtBkPrice = CreateTextBox(inputLeftX, startY + gapY * 4);
            // 状态
            lblBkStatus = CreateLabel("状　态：", leftX, startY + gapY * 5);
            txtBkStatus = CreateTextBox(inputLeftX, startY + gapY * 5);
            txtBkStatus.ReadOnly = true;

            // 按钮
            int btnY = startY + gapY * 6 + 10;
            btnBackMenu = CreateButton("返回主菜单", leftX, btnY, 105);
            btnBackMenu.Click += BtnBackMenu_Click;
            btnAdd = CreateButton("添加", leftX + 115, btnY, 70);
            btnAdd.Click += BtnAdd_Click;
            btnQuery = CreateButton("查询", leftX, btnY + 38, 105);
            btnQuery.Click += BtnQuery_Click;
            btnDelete = CreateButton("删除", leftX + 115, btnY + 38, 70);
            btnDelete.Click += BtnDelete_Click;
            btnAlter = CreateButton("修改", leftX, btnY + 76, 185);
            btnAlter.Click += BtnAlter_Click;

            // DataGridView
            dgvData = new DataGridView
            {
                Location = new Point(260, 20),
                Size = new Size(570, 460),
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
                lblBkID, txtBkID, lblBkName, txtBkName,
                lblBkAuthor, txtBkAuthor, lblBkPress, txtBkPress,
                lblBkPrice, txtBkPrice, lblBkStatus, txtBkStatus,
                btnBackMenu, btnAdd, btnQuery, btnDelete, btnAlter, dgvData
            });

            DataBind();
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("微软雅黑", 9),
                Size = new Size(80, 25),
                Location = new Point(x, y),
                TextAlign = ContentAlignment.MiddleRight
            };
        }

        private TextBox CreateTextBox(int x, int y)
        {
            return new TextBox
            {
                Font = new Font("微软雅黑", 9),
                Size = new Size(130, 25),
                Location = new Point(x, y)
            };
        }

        private Button CreateButton(string text, int x, int y, int w)
        {
            return new Button
            {
                Text = text,
                Font = new Font("微软雅黑", 9),
                Size = new Size(w, 30),
                Location = new Point(x, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightSteelBlue
            };
        }

        /// <summary>
        /// 加载图书数据
        /// </summary>
        private void DataBind()
        {
            try
            {
                string sql = "SELECT bkID AS '书号', bkName AS '书名', bkAuthor AS '作者', " +
                             "bkPress AS '出版社', bkPrice AS '单价', " +
                             "CASE bkStatus WHEN 1 THEN '在馆' WHEN 0 THEN '不在馆' END AS '状态' FROM Book";
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
                txtBkID.Text = row.Cells["书号"].Value == null ? "" : row.Cells["书号"].Value.ToString();
                txtBkName.Text = row.Cells["书名"].Value == null ? "" : row.Cells["书名"].Value.ToString();
                txtBkAuthor.Text = row.Cells["作者"].Value == null ? "" : row.Cells["作者"].Value.ToString();
                txtBkPress.Text = row.Cells["出版社"].Value == null ? "" : row.Cells["出版社"].Value.ToString();
                txtBkPrice.Text = row.Cells["单价"].Value == null ? "" : row.Cells["单价"].Value.ToString();
                txtBkStatus.Text = row.Cells["状态"].Value == null ? "" : row.Cells["状态"].Value.ToString();
            }
        }

        private void ClearInputs()
        {
            txtBkID.Text = "";
            txtBkName.Text = "";
            txtBkAuthor.Text = "";
            txtBkPress.Text = "";
            txtBkPrice.Text = "";
            txtBkStatus.Text = "";
        }

        private void BtnBackMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string bkID = txtBkID.Text.Trim();
            if (string.IsNullOrEmpty(bkID))
            {
                MessageBox.Show("请输入书号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sql = @"INSERT INTO Book(bkID, bkName, bkAuthor, bkPress, bkPrice, bkStatus) 
                               VALUES(@bkID, @bkName, @bkAuthor, @bkPress, @bkPrice, 1)";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@bkID", bkID),
                    new MySqlParameter("@bkName", txtBkName.Text.Trim()),
                    new MySqlParameter("@bkAuthor", txtBkAuthor.Text.Trim()),
                    new MySqlParameter("@bkPress", txtBkPress.Text.Trim()),
                    new MySqlParameter("@bkPrice", string.IsNullOrEmpty(txtBkPrice.Text.Trim()) ? (object)DBNull.Value : Convert.ToDecimal(txtBkPrice.Text.Trim()))
                };

                DBHelper.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBind();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加失败！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            string bkName = txtBkName.Text.Trim();

            try
            {
                string sql = "SELECT bkID AS '书号', bkName AS '书名', bkAuthor AS '作者', " +
                             "bkPress AS '出版社', bkPrice AS '单价', " +
                             "CASE bkStatus WHEN 1 THEN '在馆' WHEN 0 THEN '不在馆' END AS '状态' FROM Book";
                if (!string.IsNullOrEmpty(bkName))
                {
                    sql += " WHERE bkName LIKE @bkName";
                }
                DataTable dt = DBHelper.ExecuteQuery(sql,
                    new MySqlParameter("@bkName", "%" + bkName + "%"));
                dgvData.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询失败！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string bkID = txtBkID.Text.Trim();
            if (string.IsNullOrEmpty(bkID))
            {
                MessageBox.Show("请先选择要删除的图书！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("确定要删除该图书吗？", "确认", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM Book WHERE bkID = @bkID";
                    DBHelper.ExecuteNonQuery(sql, new MySqlParameter("@bkID", bkID));
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DataBind();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnAlter_Click(object sender, EventArgs e)
        {
            string bkID = txtBkID.Text.Trim();
            if (string.IsNullOrEmpty(bkID))
            {
                MessageBox.Show("请先选择要修改的图书！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sql = @"UPDATE Book SET bkName = @bkName, bkAuthor = @bkAuthor, 
                               bkPress = @bkPress, bkPrice = @bkPrice WHERE bkID = @bkID";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@bkName", txtBkName.Text.Trim()),
                    new MySqlParameter("@bkAuthor", txtBkAuthor.Text.Trim()),
                    new MySqlParameter("@bkPress", txtBkPress.Text.Trim()),
                    new MySqlParameter("@bkPrice", string.IsNullOrEmpty(txtBkPrice.Text.Trim()) ? (object)DBNull.Value : Convert.ToDecimal(txtBkPrice.Text.Trim())),
                    new MySqlParameter("@bkID", bkID)
                };

                DBHelper.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBind();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("修改失败！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
