using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using JF.NET.ComponentModel;

namespace Example
{
    public class Student : INotifyPropertyChanged
    {
        private string name = "unknown";
        [DisplayName("姓名")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        private int age;
        [DisplayName("年龄")]
        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Age)));
            }
        }

        private Sex sex;

        public event PropertyChangedEventHandler PropertyChanged;

        [DisplayName("性别")]
        public Sex Sex
        {
            get { return sex; }
            set
            {
                sex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sex)));
            }
        }

        [Dependence(nameof(Name))]
        public override string ToString()
        {
            return Name;
        }
    }
}
