using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceTools.Utils
{
    public class SQLiteColumn
    {
        public SQLiteColumn(string name, string type)
        {
            this.ColumnName = name;
            this.DataType = type;
            this.Length = 0;
            this.DecLength = 0;
        }

        public SQLiteColumn(string name, string type, int length, int decLength = 0)
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
