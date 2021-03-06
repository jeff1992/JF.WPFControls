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
    /// Interaction logic for StringEditor.xaml
    /// </summary>
    public partial class StringEditor : UserControl, IPropertyEditor
    {
        public StringEditor()
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
            if (SelectedObject == null || Property == null) return;
            _c.Text = Property.GetValue(SelectedObject) as string;
        }

        void SetValue()
        {
            if (SelectedObject == null || Property == null) return;
            Property.SetValue(SelectedObject, _c.Text);
        }

        private void _c_LostFocus(object sender, RoutedEventArgs e)
        {
            SetValue();
        }

        private void _c_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetValue();
            }
        }
    }
}
