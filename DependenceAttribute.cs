using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JF.WPFControls
{
    public class DependenceAttribute : Attribute
    {
        public DependenceAttribute(params string[] Properties)
        {
            this.Properties = Properties;
        }

        public string[] Properties { get; private set; }
    }
}
