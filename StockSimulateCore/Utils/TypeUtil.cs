using System;
using System.Collections.Generic;
using System.Text;

namespace StockSimulateCore.Utils
{
    public class TypeUtil
    {
        public static bool IsNullableType(Type theType)
        {
            return Nullable.GetUnderlyingType(theType) != null;
        }

        public static Type GetNullableType(Type theType)
        {
            return Nullable.GetUnderlyingType(theType);
        }
    }
}
