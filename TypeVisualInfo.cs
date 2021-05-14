using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace JF.WPFControls
{
    internal class TypeVisualInfo
    {
        static Dictionary<Type, TypeVisualInfo> savedTypeInfos = new Dictionary<Type, TypeVisualInfo>();
        public List<PropVisualInfo> Props { get; } = new List<PropVisualInfo>();
        public Type OriginalType { get; private set; }
        public string Alias { get; private set; }
        public static TypeVisualInfo Get(Type type)
        {
            TypeVisualInfo info;
            savedTypeInfos.TryGetValue(type, out info);
            if (info == null)
            {
                info = new TypeVisualInfo();
                info.OriginalType = type;
                info.Alias = type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                foreach (var p in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    var prop = new PropVisualInfo();
                    prop.OriginalProperty = p;
                    foreach (var attr in p.GetCustomAttributes())
                    {
                        var t = attr.GetType();
                        if (t == typeof(DisplayNameAttribute))
                        {
                            prop.Alias = (attr as DisplayNameAttribute).DisplayName;
                        }
                        else if (t == typeof(DescriptionAttribute))
                        {
                            prop.Description = (attr as DescriptionAttribute).Description;
                        }
                        else if (t == typeof(CategoryAttribute))
                        {
                            prop.Category = (attr as CategoryAttribute).Category;
                        }
                        else if (t == typeof(BrowsableAttribute))
                        {
                            prop.Browersable = (attr as BrowsableAttribute).Browsable;
                        }
                        else if (t == typeof(CategoryAttribute))
                        {
                            prop.Category = (attr as CategoryAttribute).Category;
                        }
                    }
                    info.Props.Add(prop);
                }
                savedTypeInfos.Add(type, info);
            }
            return info;
        }
    }
}
