using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StockSimulateDomain.Model
{
    public class EntityPropertyInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Type Type { get; set; }
    }
}
