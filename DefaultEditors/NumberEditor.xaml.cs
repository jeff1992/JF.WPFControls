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
    /// Interaction logic for NumberEditor.xaml
    /// </summary>
    public partial class NumberEditor : UserControl, IPropertyEditor
    {
        public NumberEditor()
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
            _c.Text = Property.GetValue(SelectedObject).ToString();
        }

        void SetValue()
        {
            if (SelectedObject == null || Property == null) return;
            var t = property.PropertyType;
            object v = null;
            try
            {
                if (t == typeof(byte))
                    v = Convert.ToByte(_c.Text);
                else if (t == typeof(UInt16))
                    v = Convert.ToUInt16(_c.Text);
                else if (t == typeof(UInt32))
                    v = Convert.ToUInt32(_c.Text);
                else if (t == typeof(UInt64))
                    v = Convert.ToUInt64(_c.Text);
                else if (t == typeof(Int16))
                    v = Convert.ToInt16(_c.Text);
                else if (t == typeof(Int32))
                    v = Convert.ToInt32(_c.Text);
                else if (t == typeof(Int64))
                    v = Convert.ToInt64(_c.Text);
                else if (t == typeof(float))
                    v = Convert.ToSingle(_c.Text);
                else if (t == typeof(double))
                    v = Convert.ToDouble(_c.Text);
                else if (t == typeof(decimal))
                    v = Convert.ToDecimal(_c.Text);
                Property.SetValue(SelectedObject, v);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                GetValue();
            }
        }

        private void _c_LostFocus(object sender, RoutedEventArgs e)
        {
            SetValue();
        }

        private void _c_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetValue();
        }
    }
}
