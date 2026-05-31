using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BookManagementSystem
{
    /// <summary>
    /// 读者管理窗体
    /// </summary>
    public partial class ReaderManagementForm : Form
    {
        private Label lblRdID;
        private Label lblRdType;
        private Label lblRdName;
        private Label lblRdDept;
        private Label lblRdQQ;
        private Label lblRdBorrowQty;
        private TextBox txtRdID;
        private TextBox txtRdType;
        private TextBox txtRdName;
        private TextBox txtRdDept;
        private TextBox txtRdQQ;
        private TextBox txtRdBorrowQty;
        private Button btnBackMenu;
        private Button btnAdd;
        private Button btnQuery;
        private Button btnDelete;
        private Button btnAlter;
        private DataGridView dgvData;

        public ReaderManagementForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "读者管理";
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

            // 读者编号
            lblRdID = CreateLabel("读者编号：", leftX, startY);
            txtRdID = CreateTextBox(inputLeftX, startY);
            // 类别号
            lblRdType = CreateLabel("类别号：", leftX, startY + gapY);
            txtRdType = CreateTextBox(inputLeftX, startY + gapY);
            // 姓名
            lblRdName = CreateLabel("姓　名：", leftX, startY + gapY * 2);
            txtRdName = CreateTextBox(inputLeftX, startY + gapY * 2);
            // 单位
            lblRdDept = CreateLabel("单　位：", leftX, startY + gapY * 3);
            txtRdDept = CreateTextBox(inputLeftX, startY + gapY * 3);
            // QQ
            lblRdQQ = CreateLabel("Q　 Q：", leftX, startY + gapY * 4);
            txtRdQQ = CreateTextBox(inputLeftX, startY + gapY * 4);
            // 已借书数量
            lblRdBorrowQty = CreateLabel("已借数量：", leftX, startY + gapY * 5);
            txtRdBorrowQty = CreateTextBox(inputLeftX, startY + gapY * 5);
            txtRdBorrowQty.ReadOnly = true;

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
                lblRdID, txtRdID, lblRdType, txtRdType,
                lblRdName, txtRdName, lblRdDept, txtRdDept,
                lblRdQQ, txtRdQQ, lblRdBorrowQty, txtRdBorrowQty,
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
        /// 加载读者数据
        /// </summary>
        private void DataBind()
        {
            try
            {
                string sql = "SELECT rdID AS '读者编号', rdType AS '类别号', rdName AS '姓名', " +
                             "rdDept AS '单位', rdQQ AS 'QQ', rdBorrowQty AS '已借书数量' FROM Reader";
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
                txtRdType.Text = row.Cells["类别号"].Value == null ? "" : row.Cells["类别号"].Value.ToString();
                txtRdName.Text = row.Cells["姓名"].Value == null ? "" : row.Cells["姓名"].Value.ToString();
                txtRdDept.Text = row.Cells["单位"].Value == null ? "" : row.Cells["单位"].Value.ToString();
                txtRdQQ.Text = row.Cells["QQ"].Value == null ? "" : row.Cells["QQ"].Value.ToString();
                txtRdBorrowQty.Text = row.Cells["已借书数量"].Value == null ? "" : row.Cells["已借书数量"].Value.ToString();
            }
        }

        private void ClearInputs()
        {
            txtRdID.Text = "";
            txtRdType.Text = "";
            txtRdName.Text = "";
            txtRdDept.Text = "";
            txtRdQQ.Text = "";
            txtRdBorrowQty.Text = "";
        }

        private void BtnBackMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string rdID = txtRdID.Text.Trim();
            if (string.IsNullOrEmpty(rdID))
            {
                MessageBox.Show("请输入读者编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sql = @"INSERT INTO Reader(rdID, rdType, rdName, rdDept, rdQQ, rdBorrowQty) 
                               VALUES(@rdID, @rdType, @rdName, @rdDept, @rdQQ, 0)";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@rdID", rdID),
                    new MySqlParameter("@rdType", string.IsNullOrEmpty(txtRdType.Text.Trim()) ? (object)DBNull.Value : Convert.ToInt32(txtRdType.Text.Trim())),
                    new MySqlParameter("@rdName", txtRdName.Text.Trim()),
                    new MySqlParameter("@rdDept", txtRdDept.Text.Trim()),
                    new MySqlParameter("@rdQQ", txtRdQQ.Text.Trim())
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
            string rdDept = txtRdDept.Text.Trim();

            try
            {
                string sql = "SELECT rdID AS '读者编号', rdType AS '类别号', rdName AS '姓名', " +
                             "rdDept AS '单位', rdQQ AS 'QQ', rdBorrowQty AS '已借书数量' FROM Reader";
                if (!string.IsNullOrEmpty(rdDept))
                {
                    sql += " WHERE rdDept LIKE @rdDept";
                }
                DataTable dt = DBHelper.ExecuteQuery(sql,
                    new MySqlParameter("@rdDept", "%" + rdDept + "%"));
                dgvData.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询失败！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string rdID = txtRdID.Text.Trim();
            if (string.IsNullOrEmpty(rdID))
            {
                MessageBox.Show("请先选择要删除的读者！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("确定要删除该读者吗？", "确认", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM Reader WHERE rdID = @rdID";
                    DBHelper.ExecuteNonQuery(sql, new MySqlParameter("@rdID", rdID));
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
            string rdID = txtRdID.Text.Trim();
            if (string.IsNullOrEmpty(rdID))
            {
                MessageBox.Show("请先选择要修改的读者！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sql = @"UPDATE Reader SET rdType = @rdType, rdName = @rdName, 
                               rdDept = @rdDept, rdQQ = @rdQQ WHERE rdID = @rdID";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@rdType", string.IsNullOrEmpty(txtRdType.Text.Trim()) ? (object)DBNull.Value : Convert.ToInt32(txtRdType.Text.Trim())),
                    new MySqlParameter("@rdName", txtRdName.Text.Trim()),
                    new MySqlParameter("@rdDept", txtRdDept.Text.Trim()),
                    new MySqlParameter("@rdQQ", txtRdQQ.Text.Trim()),
                    new MySqlParameter("@rdID", rdID)
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
