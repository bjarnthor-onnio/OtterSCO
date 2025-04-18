﻿using LS.SCO.Entity.Base;

namespace LS.SCO.Plugin.Adapter.Controllers.Models
{
    public class GetItemDetailsInput : BaseCachingEntity
    {
        public string BarCode { get; set; }
        public string ItemId { get; set; }
    }
}
