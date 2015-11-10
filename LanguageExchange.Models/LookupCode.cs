namespace LanguageExchange.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LookupCode
    {
        public int Id { get; set; }
        [StringLength(15)]
        public string Code { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(25)]
        public string GroupName { get; set; }

        [StringLength(100)]
        public string Value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(500)]
        public string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Active { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public short? SortOrder { get; set; }
    }
}
