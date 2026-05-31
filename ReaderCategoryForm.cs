using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BookManagementSystem
{
    /// <summary>
    /// 读者类别管理窗体
    /// </summary>
    public partial class ReaderCategoryForm : Form
    {
        private Label lblRdType;
        private Label lblRdTypeName;
        private Label lblCanLendQty;
        private Label lblCanLendDay;
        private TextBox txtRdType;
        private TextBox txtRdTypeName;
        private TextBox txtCanLendQty;
        private TextBox txtCanLendDay;
        private Button btnBackMenu;
        private Button btnAdd;
        private Button btnQuery;
        private Button btnDelete;
        private Button btnAlter;
        private DataGridView dgvData;

        public ReaderCategoryForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "读者类别管理";
            this.Size = new Size(820, 520);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void InitializeComponent()
        {
            int leftX = 20;
            int inputLeftX = 120;
            int startY = 20;
            int gapY = 40;

            // 读者类别编号
            lblRdType = new Label
            {
                Text = "类别编号：",
                Font = new Font("微软雅黑", 9),
                Size = new Size(80, 25),
                Location = new Point(leftX, startY),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtRdType = new TextBox
            {
                Font = new Font("微软雅黑", 9),
                Size = new Size(120, 25),
                Location = new Point(inputLeftX, startY)
            };

            // 类别名称
            lblRdTypeName = new Label
            {
                Text = "类别名称：",
                Font = new Font("微软雅黑", 9),
                Size = new Size(80, 25),
                Location = new Point(leftX, startY + gapY),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtRdTypeName = new TextBox
            {
                Font = new Font("微软雅黑", 9),
                Size = new Size(120, 25),
                Location = new Point(inputLeftX, startY + gapY)
            };

            // 可借书数量
            lblCanLendQty = new Label
            {
                Text = "可借数量：",
                Font = new Font("微软雅黑", 9),
                Size = new Size(80, 25),
                Location = new Point(leftX, startY + gapY * 2),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtCanLendQty = new TextBox
            {
                Font = new Font("微软雅黑", 9),
                Size = new Size(120, 25),
                Location = new Point(inputLeftX, startY + gapY * 2)
            };

            // 可借书天数
            lblCanLendDay = new Label
            {
                Text = "可借天数：",
                Font = new Font("微软雅黑", 9),
                Size = new Size(80, 25),
                Location = new Point(leftX, startY + gapY * 3),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtCanLendDay = new TextBox
            {
                Font = new Font("微软雅黑", 9),
                Size = new Size(120, 25),
                Location = new Point(inputLeftX, startY + gapY * 3)
            };

            // 按钮区域
            int btnY = startY + gapY * 4 + 10;
            btnBackMenu = CreateButton("返回主菜单", leftX, btnY, 105);
            btnBackMenu.Click += BtnBackMenu_Click;

            btnAdd = CreateButton("添加", leftX + 115, btnY, 70);
            btnAdd.Click += BtnAdd_Click;

            btnQuery = CreateButton("查询", leftX, btnY + 40, 105);
            btnQuery.Click += BtnQuery_Click;

            btnDelete = CreateButton("删除", leftX + 115, btnY + 40, 70);
            btnDelete.Click += BtnDelete_Click;

            btnAlter = CreateButton("修改", leftX, btnY + 80, 185);
            btnAlter.Click += BtnAlter_Click;

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
                lblRdType, txtRdType, lblRdTypeName, txtRdTypeName,
                lblCanLendQty, txtCanLendQty, lblCanLendDay, txtCanLendDay,
                btnBackMenu, btnAdd, btnQuery, btnDelete, btnAlter, dgvData
            });

            DataBind();
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
        /// 加载读者类别数据并绑定到DataGridView
        /// </summary>
        private void DataBind()
        {
            try
            {
                string sql = "SELECT rdType AS '类别编号', rdTypeName AS '类别名称', " +
                             "canLendQty AS '可借书数量', canLendDay AS '可借书天数' FROM ReaderType";
                DataTable dt = DBHelper.ExecuteQuery(sql);
                dgvData.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据失败！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DataGridView行选中事件 - 绑定到输入框
        /// </summary>
        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null && dgvData.CurrentRow.Index >= 0)
            {
                DataGridViewRow row = dgvData.CurrentRow;
                txtRdType.Text = row.Cells["类别编号"].Value == null ? "" : row.Cells["类别编号"].Value.ToString();
                txtRdTypeName.Text = row.Cells["类别名称"].Value == null ? "" : row.Cells["类别名称"].Value.ToString();
                txtCanLendQty.Text = row.Cells["可借书数量"].Value == null ? "" : row.Cells["可借书数量"].Value.ToString();
                txtCanLendDay.Text = row.Cells["可借书天数"].Value == null ? "" : row.Cells["可借书天数"].Value.ToString();
            }
        }

        /// <summary>
        /// 清空输入框
        /// </summary>
        private void ClearInputs()
        {
            txtRdType.Text = "";
            txtRdTypeName.Text = "";
            txtCanLendQty.Text = "";
            txtCanLendDay.Text = "";
        }

        /// <summary>
        /// 返回主菜单
        /// </summary>
        private void BtnBackMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 添加读者类别
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string rdType = txtRdType.Text.Trim();
            string rdTypeName = txtRdTypeName.Text.Trim();
            string canLendQty = txtCanLendQty.Text.Trim();
            string canLendDay = txtCanLendDay.Text.Trim();

            if (string.IsNullOrEmpty(rdType))
            {
                MessageBox.Show("请输入类别编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sql = "INSERT INTO ReaderType(rdType, rdTypeName, canLendQty, canLendDay) " +
                             "VALUES(@rdType, @rdTypeName, @canLendQty, @canLendDay)";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@rdType", Convert.ToInt32(rdType)),
                    new MySqlParameter("@rdTypeName", rdTypeName),
                    new MySqlParameter("@canLendQty", string.IsNullOrEmpty(canLendQty) ? (object)DBNull.Value : Convert.ToInt32(canLendQty)),
                    new MySqlParameter("@canLendDay", string.IsNullOrEmpty(canLendDay) ? (object)DBNull.Value : Convert.ToInt32(canLendDay))
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

        /// <summary>
        /// 根据类别名称查询
        /// </summary>
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            string rdTypeName = txtRdTypeName.Text.Trim();

            try
            {
                string sql = "SELECT rdType AS '类别编号', rdTypeName AS '类别名称', " +
                             "canLendQty AS '可借书数量', canLendDay AS '可借书天数' FROM ReaderType";
                if (!string.IsNullOrEmpty(rdTypeName))
                {
                    sql += " WHERE rdTypeName LIKE @rdTypeName";
                }
                DataTable dt = DBHelper.ExecuteQuery(sql,
                    new MySqlParameter("@rdTypeName", "%" + rdTypeName + "%"));
                dgvData.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询失败！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除读者类别
        /// </summary>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string rdType = txtRdType.Text.Trim();
            if (string.IsNullOrEmpty(rdType))
            {
                MessageBox.Show("请先选择要删除的类别！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("确定要删除该读者类别吗？", "确认", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM ReaderType WHERE rdType = @rdType";
                    DBHelper.ExecuteNonQuery(sql, new MySqlParameter("@rdType", Convert.ToInt32(rdType)));
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

        /// <summary>
        /// 修改读者类别
        /// </summary>
        private void BtnAlter_Click(object sender, EventArgs e)
        {
            string rdType = txtRdType.Text.Trim();
            if (string.IsNullOrEmpty(rdType))
            {
                MessageBox.Show("请先选择要修改的类别！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rdTypeName = txtRdTypeName.Text.Trim();
            string canLendQty = txtCanLendQty.Text.Trim();
            string canLendDay = txtCanLendDay.Text.Trim();

            try
            {
                string sql = "UPDATE ReaderType SET rdTypeName = @rdTypeName, " +
                             "canLendQty = @canLendQty, canLendDay = @canLendDay WHERE rdType = @rdType";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@rdTypeName", rdTypeName),
                    new MySqlParameter("@canLendQty", string.IsNullOrEmpty(canLendQty) ? (object)DBNull.Value : Convert.ToInt32(canLendQty)),
                    new MySqlParameter("@canLendDay", string.IsNullOrEmpty(canLendDay) ? (object)DBNull.Value : Convert.ToInt32(canLendDay)),
                    new MySqlParameter("@rdType", Convert.ToInt32(rdType))
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
