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
    /// Interaction logic for BooleanEditor.xaml
    /// </summary>
    public partial class BooleanEditor : UserControl, IPropertyEditor
    {
        public BooleanEditor()
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
                SetBinding();
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
            _c.SelectedIndex = (bool)Property.GetValue(SelectedObject) ? 1 : 0;
        }

        void SetValue()
        {
            if (SelectedObject == null || Property == null || !Property.CanWrite) return;
            Property.SetValue(SelectedObject, _c.SelectedIndex == 1);
        }

        private void _c_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetValue();
        }
    }
}
