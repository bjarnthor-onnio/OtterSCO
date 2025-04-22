using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartnerTech.Otter.Client.Models.BaseModels
{

    public class Message
    {
        public string jsonrpc { get; set; }
        public string method { get; set; }
        public string id { get; set; }
        public string error { get; set; }


        public string ToJson()
        {
            JsonSerializerSettings sett = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(this, sett);
        }

        public Message()
        {
            jsonrpc = "2.0";
        }

    }

    public class Params
    {

    }

    public class Result
    {
        public bool successful { get; set; }
        public string message { get; set; }
    }


    

}
