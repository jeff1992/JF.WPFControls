using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace JF.WPFControls
{
    interface IPropertyEditor
    {
        object SelectedObject { get; set; }
        PropertyInfo Property { get; set; }
    }
}
