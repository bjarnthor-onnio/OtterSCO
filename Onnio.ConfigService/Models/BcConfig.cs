﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.ConfigService.Models
{
    public class BcConfig
    {
        public string? OdataServiceBaseUrl { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public string? StaffId { get; set; }
    }
}
