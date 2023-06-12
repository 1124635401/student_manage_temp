using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace student_manager
{
    // 学生类
    public class Student
    {
        // 学号、姓名、性别、手机号
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Sex { get; set; }
        public string Phone { get; set; }

        public Student()
        {
        }
        public Student(int id, string name, bool sex, string phone)
        {
            this.Id = id;
            this.Name = name;
            this.Sex = sex;
            this.Phone = phone;
        }
    }
}
