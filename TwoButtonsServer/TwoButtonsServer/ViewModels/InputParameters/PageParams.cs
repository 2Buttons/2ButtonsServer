﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.InputParameters
{
    public class PageParams
    {
        public int Page { get; set; } = 0;
        public int Amount { get; set; } = 100;
    }
}