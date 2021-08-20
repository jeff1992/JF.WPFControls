using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JF.WPFControls.DefaultEditors
{
    /// <summary>
    /// Interaction logic for ObjectEditor.xaml
    /// </summary>
    public partial class ObjectEditor : UserControl, IPropertyEditor
    {
        public ObjectEditor()
        {
            InitializeComponent();
        }

        object selectedObject;

        public object SelectedObject
        {
            get => selectedObject;
            set
            {
                selectedObject = value;
                GetValue();
            }
        }

        PropertyInfo property;
        public PropertyInfo Property
        {
            get => property;
            set
            {
                property = value;
                GetValue();
                SetBinding();
            }
        }
        void SetBinding()
        {
            if (selectedObject != null && selectedObject is INotifyPropertyChanged)
            {
                (selectedObject as INotifyPropertyChanged).PropertyChanged += PropertyValueChanged;
            }
        }

        private void PropertyValueChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Property.Name)
            {
                if (Dispatcher.CheckAccess())
                    GetValue();
                else
                    Dispatcher.Invoke(GetValue);
            }
        }

        void GetValue()
        {
            return;
        }

        void SetValue()
        {
            return;
        }

        private void _c_LostFocus(object sender, RoutedEventArgs e)
        {
            SetValue();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var v = Property.GetValue(SelectedObject);
            var alias = Property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            var w = new Window()
            {
                Content = new PropertyGrid()
                {
                    SelectedObject = v
                },
                Title = alias ?? Property.Name,
                Width = 300,
                Height = 380,
                Owner = Window.GetWindow(this)
            };
            w.ShowDialog();
        }
    }
}
