using System;
using System.Collections.Generic;
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
using System.Reflection;
using System.ComponentModel;

namespace JF.WPFControls.DefaultEditors
{
    /// <summary>
    /// Interaction logic for EnumEditor.xaml
    /// </summary>
    public partial class EnumEditor : UserControl, IPropertyEditor
    {
        public EnumEditor()
        {
            InitializeComponent();
        }

        private void _c_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetValue();
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
            _c.ItemsSource = Property.PropertyType.GetEnumValues();
            _c.SelectedItem = Property.GetValue(SelectedObject);
        }

        void SetValue()
        {
            if (SelectedObject == null || Property == null) return;
            Property.SetValue(SelectedObject, _c.SelectedItem);
        }
    }
}
