using System;
using System.Collections;
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
using JF.WPFControls.DefaultEditors;
using System.Linq;
using System.Collections.Specialized;

namespace JF.WPFControls
{
    /// <summary>
    /// Interaction logic for PropertyGrid.xaml
    /// </summary>
    public partial class PropertyGrid : UserControl
    {
        public PropertyGrid()
        {
            InitializeComponent();
        }

        static Dictionary<Type, Type> defaultEditors;
        static Dictionary<Type, Type> DefaultEditors
        {
            get
            {
                if (defaultEditors == null)
                {
                    defaultEditors = new Dictionary<Type, Type>();
                    defaultEditors.Add(typeof(string), typeof(StringEditor));
                    defaultEditors.Add(typeof(bool), typeof(BooleanEditor));
                    defaultEditors.Add(typeof(Enum), typeof(EnumEditor));
                    defaultEditors.Add(typeof(byte), typeof(NumberEditor));
                    defaultEditors.Add(typeof(Int16), typeof(NumberEditor));
                    defaultEditors.Add(typeof(Int32), typeof(NumberEditor));
                    defaultEditors.Add(typeof(Int64), typeof(NumberEditor));
                    defaultEditors.Add(typeof(UInt16), typeof(NumberEditor));
                    defaultEditors.Add(typeof(UInt32), typeof(NumberEditor));
                    defaultEditors.Add(typeof(UInt64), typeof(NumberEditor));
                    defaultEditors.Add(typeof(float), typeof(NumberEditor));
                    defaultEditors.Add(typeof(double), typeof(NumberEditor));
                    defaultEditors.Add(typeof(decimal), typeof(NumberEditor));
                    defaultEditors.Add(typeof(object), typeof(ObjectEditor));
                }
                return defaultEditors;
            }
        }

        public object SelectedObject
        {
            get { return (object)GetValue(SelectedObjectProperty); }
            set
            {
                SetValue(SelectedObjectProperty, value);
                BuildGrids();
            }
        }

        // Using a DependencyProperty as the backing store for SelectedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof(object), typeof(PropertyGrid), new PropertyMetadata(0));


        void BuildGrids()
        {
            _list.Children.Clear();
            var selectedObject = SelectedObject;
            if (selectedObject == null) return;
            var info = TypeVisualInfo.Get(selectedObject.GetType());
            foreach (var prop in info.Props)
            {
                if (!prop.OriginalProperty.CanWrite) continue;
                if (!prop.Browersable) continue;

                if (selectedObject is IList && prop.OriginalProperty.Name == "Item")
                {
                    var items = selectedObject as IList;
                    var innerInfo = TypeVisualInfo.Get(prop.OriginalProperty.PropertyType);
                    var grid = new Grid() { Margin = new Thickness(2) };
                    foreach (var p in innerInfo.Props.Where(m => m.Browersable))
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                    for (var i = 0; i < items.Count + 1; i++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition());
                    }
                    var col = 0;
                    var row = 0;
                    foreach (var p in innerInfo.Props.Where(m => m.Browersable))
                    {
                        var head = new Label();
                        head.Content = p.Alias ?? p.OriginalProperty.Name;
                        Grid.SetColumn(head, col);
                        Grid.SetRow(head, row);
                        grid.Children.Add(head);
                        col++;
                    }
                    row++;
                    foreach (var item in items)
                    {
                        col = 0;
                        foreach (var p in innerInfo.Props.Where(m => m.Browersable))
                        {
                            var editor = GetEditor(p.OriginalProperty.PropertyType);
                            if (editor != null)
                            {
                                editor.SelectedObject = item;
                                editor.Property = p.OriginalProperty;
                                var ui = editor as UIElement;
                                grid.Children.Add(ui);
                                Grid.SetColumn(ui, col);
                                Grid.SetRow(ui, row);
                            }
                            col++;
                        }
                        row++;
                    }
                    _list.Children.Add(grid);
                    if (selectedObject is IBindingList)
                    {
                        var bindingList = selectedObject as IBindingList;
                        bindingList.ListChanged += (sender, args) =>
                        {
                            switch (args.ListChangedType)
                            {
                                case ListChangedType.Reset:

                                    break;
                                case ListChangedType.ItemChanged:
                                    break;
                                case ListChangedType.ItemAdded:
                                    break;
                                case ListChangedType.ItemDeleted:
                                    break;
                            }
                        };
                    }
                    else if (selectedObject is INotifyCollectionChanged)
                    {
                        var collection = selectedObject as INotifyCollectionChanged;
                        collection.CollectionChanged += (sender, args) =>
                        {
                            switch (args.Action)
                            {
                                case NotifyCollectionChangedAction.Add:
                                    break;
                                case NotifyCollectionChangedAction.Remove:
                                    break;
                                case NotifyCollectionChangedAction.Replace:
                                    break;
                                case NotifyCollectionChangedAction.Reset:
                                    break;
                                case NotifyCollectionChangedAction.Move:
                                    break;
                            }
                        };
                    }
                }
                else
                {
                    var panel = new DockPanel() { Margin = new Thickness(2) };
                    var editor = GetEditor(prop.OriginalProperty.PropertyType);
                    if (editor != null)
                    {
                        editor.SelectedObject = selectedObject;
                        editor.Property = prop.OriginalProperty;
                        panel.Children.Add(editor as UIElement);
                        DockPanel.SetDock(editor as UIElement, Dock.Right);
                    }

                    var label = new Label() { Content = prop.Alias ?? prop.OriginalProperty.Name, MinWidth = 120 };
                    panel.Children.Add(label);

                    _list.Children.Add(panel);
                }
                if (prop.Description != null)
                {
                    _list.Children.Add(new Label
                    {
                        Content = prop.Description,
                        Foreground = Brushes.Gray,
                        Margin = new Thickness(2, 0, 2, 2)
                    });
                }
            }
        }

        IPropertyEditor GetEditor(Type valueType)
        {
            foreach (var key in DefaultEditors.Keys)
            {
                if (key.IsAssignableFrom(valueType))
                {
                    var editorType = DefaultEditors[key];
                    var editor = Assembly.GetAssembly(editorType).CreateInstance(editorType.FullName) as IPropertyEditor;
                    return editor;
                }
            }
            return null;
        }

    }
}
