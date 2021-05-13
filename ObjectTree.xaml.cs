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
using System.Linq;

namespace JF.WPFControls
{
    /// <summary>
    /// Interaction logic for ObjectTree.xaml
    /// </summary>
    public partial class ObjectTree : UserControl
    {
        public ObjectTree()
        {
            InitializeComponent();
        }

        public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged;
        public static readonly DependencyProperty RootProperty = DependencyProperty.Register(nameof(Root), typeof(object), typeof(ObjectTree), new PropertyMetadata(null, PropertyChangedCallback, null));

        private static void PropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.Property == RootProperty)
            {
                (obj as ObjectTree).BuildTree();
            }
        }

        private ContextMenu contextMenu;

        public ContextMenu ContextMenu
        {
            get
            {
                if (contextMenu == null)
                {
                    //构造命令

                    contextMenu = new ContextMenu();
                }
                return contextMenu;
            }
        }

        public object Root
        {
            get => GetValue(RootProperty);
            set => SetValue(RootProperty, value);
        }


        void BuildTree()
        {
            _tree.Items.Clear();
            var root = Root;
            if (root == null)
                return;
            var node = new TreeViewItem
            {
                Header = root,
                Tag = root,
                Padding = new Thickness(0)
            };
            SetBinding(node, root);
            BuildTree(node.Items, root);
            _tree.Items.Add(node);
        }

        void BuildTree(ItemCollection container, object obj)
        {
            if (obj == null) return;
            if (obj is IList)
            {
                foreach (var item in obj as IList)
                {
                    var node = new TreeViewItem
                    {
                        Header = item,
                        Tag = item
                    };
                    container.Add(node);

                    SetBinding(node, item);

                    BuildTree(node.Items, item);
                }
            }
            else
            {
                foreach (var prop in TypeVisualInfo.Get(obj.GetType()).Props)
                {
                    if (prop.OriginalProperty.PropertyType.IsValueType || prop.OriginalProperty.PropertyType == typeof(string))
                        continue;
                    if (!prop.Browersable) continue;
                    var value = prop.OriginalProperty.GetValue(obj);
                    var node = new TreeViewItem
                    {
                        //Header = (alias ?? prop.Name) + (value == null ? "" : $" :{value}"),
                        Header = prop.Alias ?? prop.OriginalProperty.Name,
                        Tag = value
                    };
                    container.Add(node);
                    BuildTree(node.Items, value);
                }
            }
        }

        void SetBinding(TreeViewItem node, object obj)
        {
            if (obj != null && obj is INotifyPropertyChanged)
            {
                var dept = obj.GetType().GetMethod(nameof(ToString)).GetCustomAttribute<DependenceAttribute>();
                if (dept != null)
                {
                    foreach (var prop in dept.Properties)
                    {
                        (obj as INotifyPropertyChanged).PropertyChanged += (sender, args) =>
                        {
                            node.Header = sender.ToString();
                        };
                    }
                }
            }
        }

        private void _tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItemChanged?.Invoke(this, e);
        }

        private void _tree_KeyUp(object sender, KeyEventArgs e)
        {
            var node = _tree.SelectedItem as TreeViewItem;
            if (node == null) return;
            var obj = node.Tag;
            switch (e.Key)
            {
                case Key.N:
                    if (obj is IList)
                    {
                        var addProps = obj.GetType().GetMethod("Add").GetParameters();
                        var t = addProps[0].ParameterType;
                        var types = Assembly.GetAssembly(t).GetTypes().Where(m => t.IsAssignableFrom(m) && !m.IsAbstract);
                        Type instanceType = null;
                        if (types.Count() == 1)
                            instanceType = types.First();
                        else
                        {
                            var infos = types.Select(m => TypeVisualInfo.Get(m)).ToList();

                        }
                        if (instanceType == null) return;
                        var instance = Assembly.GetAssembly(instanceType).CreateInstance(instanceType.FullName);
                        (obj as IList).Add(instance);
                        var newNode = new TreeViewItem
                        {
                            Header = instance,
                            Tag = instance
                        };
                        node.Items.Add(newNode);
                        SetBinding(newNode, instance);
                        BuildTree(newNode.Items, instance);
                    }
                    break;
                case Key.Delete:
                    if ((node.Parent as TreeViewItem)?.Tag is IList)
                    {
                        var pNode = node.Parent as TreeViewItem;
                        ((pNode).Tag as IList).Remove(node.Tag);
                        pNode.Items.Remove(node);
                    }
                    break;
            }
        }

        private void ListAddOpening(object sender, ContextMenuEventArgs e)
        {
            var selectedObject = (_tree.SelectedItem as TreeViewItem).Tag;
            if (selectedObject == null)
                return;

        }

    }
}
