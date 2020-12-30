﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StockSimulateWeb.Base
{
    public class APIResult
    {
        public bool Success => this.Code == 200;

        public int Code { get; set; } = 200;

        public string Message { get; set; }

        public Exception Error { get; set; }

        public object Result { get; set; }
    }
}
