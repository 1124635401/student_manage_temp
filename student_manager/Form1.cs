using Newtonsoft.Json;
using Zj.Common;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace student_manager
{
    public partial class Form1 : Form
    {
        private string excelFilePath = "students.xlsx";
        private ExcelPackage excelPackage;

        public Form1()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            InitGridView();

            excelPackage = new ExcelPackage();
            LoadDataFromExcel();

            // Subscribe to the FormClosing event to save the data before closing the form
            this.FormClosing += Form1_FormClosing;
        }

        private void InitGridView()
        {
            grid.ReadOnly = true;
        }

        private void AddRow(Student stu)
        {
            object[] row =
            {
                stu.Id ,
                stu.Name,
                stu.Sex ? "男" : "女",
                stu.Phone
            };

            int rowIndex = grid.Rows.Add(row);
            grid.Rows[rowIndex].Tag = stu;

            SaveDataToExcel();
        }

        private void UpdateRow(int rowIndex, Student stu)
        {
            grid.Rows[rowIndex].Tag = stu;
            grid[0, rowIndex].Value = stu.Id;
            grid[1, rowIndex].Value = stu.Name;
            grid[2, rowIndex].Value = stu.Sex ? "男" : "女";
            grid[3, rowIndex].Value = stu.Phone;

            SaveDataToExcel();
        }

        private void grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point p = GetCellAt(grid, e.Location);
                int rowIndex = p.X;

                grid.ClearSelection();
                if (rowIndex >= 0)
                    grid.Rows[rowIndex].Selected = true;

                menu_Delete.Enabled = (rowIndex >= 0);
                menu_Edit.Enabled = (rowIndex >= 0);

                contextMenu.Show(grid, e.Location);
            }
        }

        private Point GetCellAt(DataGridView grid, Point location)
        {
            int row = -1, col = -1;

            for (int i = grid.FirstDisplayedScrollingRowIndex;
                i < grid.FirstDisplayedScrollingRowIndex + grid.DisplayedRowCount(true);
                i++)
            {
                Rectangle rect = grid.GetRowDisplayRectangle(i, true);
                if (location.Y > rect.Top && location.Y < rect.Bottom)
                {
                    row = i;
                    break;
                }
            }

            for (int k = grid.FirstDisplayedScrollingColumnIndex;
                k < grid.FirstDisplayedScrollingColumnIndex + grid.DisplayedColumnCount(true);
                k++)
            {
                Rectangle rect = grid.GetColumnDisplayRectangle(k, true);
                if (location.X > rect.Left && location.X < rect.Right)
                {
                    col = k;
                    break;
                }
            }

            return new Point(row, col);
        }

        private void menu_Add_Click(object sender, EventArgs e)
        {
            StuEditDialog dlg = new StuEditDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Student stu = dlg.GetValue();
                AddRow(stu);
            }
        }

        private void menu_Edit_Click(object sender, EventArgs e)
        {
            int rowIndex = grid.SelectedRows[0].Index;
            Student tag = (Student)grid.Rows[rowIndex].Tag;

            StuEditDialog dlg = new StuEditDialog();
            dlg.InitValue(tag);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Student stu = dlg.GetValue();
                UpdateRow(rowIndex, stu);
            }
        }

        private void menu_Delete_Click(object sender, EventArgs e)
        {
            MyConfirmDialog dlg = new MyConfirmDialog();
            dlg.label.Text = "此操作不可恢复，确定要删除吗？";
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                foreach (DataGridViewRow row in grid.SelectedRows)
                {
                    grid.Rows.Remove(row);
                }

                SaveDataToExcel();
            }
        }

        private void SaveDataToExcel()
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet;

                if (package.Workbook.Worksheets.Count > 0)
                {
                    worksheet = package.Workbook.Worksheets[1];
                }
                else
                {
                    worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    for (int col = 0; col < grid.Columns.Count; col++)
                    {
                        string columnHeader = grid.Columns[col].HeaderText;
                        worksheet.Cells[1, col + 1].Value = columnHeader;
                    }
                }

                int rowCount = grid.Rows.Count;
                int columnCount = grid.Columns.Count;

                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < columnCount; col++)
                    {
                        string cellValue = grid.Rows[row].Cells[col].Value?.ToString();
                        worksheet.Cells[row + 2, col + 1].Value = cellValue;
                    }
                }

                // 将文件保存到指定路径
                package.SaveAs(new FileInfo(excelFilePath));
            }
        }



        private void LoadDataFromExcel()
        {
            if (!File.Exists(excelFilePath))
            {
                return;
            }

            try
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        return;
                    }

                    grid.Rows.Clear();

                    int rowCount = worksheet.Dimension?.Rows ?? 0;
                    int columnCount = worksheet.Dimension?.Columns ?? 0;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1]?.Value);
                        string name = Convert.ToString(worksheet.Cells[row, 2]?.Value);
                        bool sex = (Convert.ToString(worksheet.Cells[row, 3]?.Value) == "男");
                        string phone = Convert.ToString(worksheet.Cells[row, 4]?.Value);

                        Student stu = new Student(id, name, sex, phone);
                        AddRow(stu);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据时出错：" + ex.Message);
            }
        }




        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the data before closing the form
            SaveDataToExcel();
        }
    }
}
