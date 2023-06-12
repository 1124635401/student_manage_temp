using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace student_manager
{
    public partial class StuEditDialog : Form
    {
        public StuEditDialog()
        {
            InitializeComponent();
            this.sexField.SelectedIndex = 1;

        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            Student stu = GetValue();
            if (stu == null)
                return;

            this.DialogResult = DialogResult.OK;
        }

        public Student GetValue()
        {
            Student stu = new Student();
            try
            {
                stu.Id = Convert.ToInt32(idField.Text.Trim());
            }catch(Exception e)
            {
                MessageBox.Show("学号须为数字!");
                return null;
            }
            
            stu.Name = nameField.Text.Trim();
            stu.Sex = (sexField.SelectedIndex == 1);
            stu.Phone = phoneField.Text.Trim();

            return stu;
        }

        public void InitValue(Student stu)
        {
            idField.Enabled = false;// 学号不可编辑

            idField.Text = stu.Id + "";
            nameField.Text = stu.Name;
            sexField.SelectedIndex = stu.Sex ? 1 : 0;
            phoneField.Text = stu.Phone;
        }
    }
}
