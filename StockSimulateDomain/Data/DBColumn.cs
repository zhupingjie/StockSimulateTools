using System;
using System.Collections.Generic;
using System.Text;

namespace StockSimulateDomain.Data
{
    public class DBColumn
    {
        public DBColumn(string name, string type)
        {
            this.ColumnName = name;
            this.DataType = type;
            this.Length = 0;
            this.DecLength = 0;
        }

        public DBColumn(string name, string type, int length, int decLength = 0)
        {
            this.ColumnName = name;
            this.DataType = type;
            this.Length = length;
            this.DecLength = decLength;
        }

        public string ColumnName { get; set; }

        public string DataType { get; set; }

        public int Length { get; set; }

        public int DecLength { get; set; }
    }
}
