﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Infrastructure.ErrorHandler
{
    interface IErrorPage
    {
        Dictionary<string, string> Headers { get; set; } 
        string Render();
    }
}
