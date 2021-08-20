using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var classRoom = new ClassRoom()
            {
                Name = "1"
            };
            classRoom.Teacher = new Teacher()
            {
                Name = "Jeff"
            };
            classRoom.Students = new List<Student>();
            for (var i = 0; i < 10; i++)
            {
                classRoom.Students.Add(new Student
                {
                    Name = "Student" + i,
                    Age = 18,
                    Sex = Sex.Mail
                });
            }

            _tree.Root = classRoom;
        }

        private void _tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var obj = (e.NewValue as TreeViewItem).Tag;
            _propGrid.SelectedObject = obj;
        }
    }
}
