using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Models
{
    public class CommunicationMethod : LookupCode
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? PreferredMethod { get; set; }
    }
}
