using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PartnerTech.Otter.Client.Models.BaseModels
{
    public class OtterMessageDeserializer
    {

		protected Regex MessageNameRegex { get; }
			= new Regex(@"[\s\S]*?method['|""]:[\s]*?['|""](\w+)", RegexOptions.IgnoreCase);


		protected string GetMessageName(string message)
		{
			return MessageNameRegex.Match(message).Groups[1].Value;
		}

		public Message DeserializeMessage(string json)
		{
			var type = Type.GetType($"LS.SCO.Plugin.Adapter.Otter.Models.FromSCO.{GetMessageName(json)}");
			if (type != null)
			{
				return (Message)Deserialize(json, type);
			}

			return null;

		}

		private object Deserialize(string json, Type type)
		{

			return JsonConvert.DeserializeObject(json, type);


		}



	}
}
