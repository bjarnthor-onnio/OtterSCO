using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    internal class totals : Message
    {
        public totalsParams @params { get; set; }
        public totals()
        {
            method = this.GetType().Name;
        }
    }

    public class totalsParams : Params
    {
        public int? transactionAmount { get; set; }
        public int? transactionDiscount { get; set; }

        public int? leftToPay { get; set; }
        public int? change { get; set; }
        public int? productsCount { get; set; }

        public int? savedAmount { get; set; }
        public int? taxAmount { get; set; }
        public int? foodStampAmount { get; set; }
        public List<transactionDiscounts> discount { get; set; }
    }



    public class transactionDiscounts
    {
        public int transactionDiscount { get; set; }
        public string transactionDiscountText { get; set; }
    }
}
