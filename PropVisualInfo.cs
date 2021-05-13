using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace JF.WPFControls
{
    internal class PropVisualInfo
    {
        public PropertyInfo OriginalProperty { get; internal set; }
        public string Alias { get; internal set; }
        public string Description { get; internal set; }
        public bool Browersable { get; internal set; } = true;
        public string Category { get; internal set; }
    }
}
