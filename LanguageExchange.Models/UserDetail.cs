using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LanguageExchange.Models
{
    public class UserDetail
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LanguageDetail[] LanguageDetails { get; set; }

    }
}