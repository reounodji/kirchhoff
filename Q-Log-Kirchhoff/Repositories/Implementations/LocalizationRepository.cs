using MVC.Repositories.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Repositories.Implementations
{
    public class LocalizationRepository : ILocalizationRepository
    {
        public Dictionary<string, string> Get(string name)
        {
            var result = new Dictionary<string, string>();
            name = name.ToLower();
            if(name != "de" && name != "en")
            {
                name = "en";
            }

            var path = "Localizations/" + name + ".json";
            using (StreamReader file = new StreamReader(path))
            {
                var full = file.ReadToEnd();
                var jObject = JObject.Parse(full);


                foreach (var entry in jObject.Children())
                {
                    result.Add(entry.Path, entry.Values<string>().First());
                }

            }
            return result;
        }

        public string GetSMSMessage(string languageCode)
        {
            var result = "";
            languageCode = languageCode.ToLower();
            var path = "Localizations/SMS/SMS-" + languageCode + ".json";

            var items = new Dictionary<string, string>();
            using (StreamReader file = new StreamReader(path))
            {
                var full = file.ReadToEnd();
                var jObject = JObject.Parse(full);


                foreach (var entry in jObject.Children())
                {
                    items.Add(entry.Path, entry.Values<string>().First());
                }

            }
            result = items["Message"];
            return result;
        }
    }
}
