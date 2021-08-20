using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Example
{
    public class ClassRoom
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
            }
        }

        private List<Student> students;
        [DisplayName("学生")]
        public List<Student> Students
        {
            get { return students; }
            set
            {
                students = value;
            }
        }

        private Teacher teacher;
        [DisplayName("老师")]
        public Teacher Teacher
        {
            get { return teacher; }
            set
            {
                teacher = value;
            }
        }

    }
}
