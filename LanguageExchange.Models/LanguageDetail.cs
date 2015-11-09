namespace LanguageExchange.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class LanguageDetail
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public short? YearsOfExperience { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }

        public bool WillingToExchange { get; set; }

        public Language Language { get; set; }

        public Proficiency Proficiency { get; set; }

        public CommunicationMethod[] CommunicationMethods { get; set; }
    }
}
