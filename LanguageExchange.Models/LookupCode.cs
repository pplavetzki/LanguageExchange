namespace LanguageExchange.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LookupCode
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Code { get; set; }

        [StringLength(25)]
        public string GroupName { get; set; }

        [Required]
        [StringLength(100)]
        public string Value { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool Active { get; set; }

        public short? SortOrder { get; set; }
    }
}
