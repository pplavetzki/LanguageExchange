namespace LanguageExchange.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CommunicationDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LanguageId { get; set; }

        public int CommunicationMethodId { get; set; }

        [ForeignKey("CommunicationMethodId")]
        public virtual CommunicationMethod CommunicationMethod { get; set; }

        public virtual UserDetail UserDetail { get; set; }
    }
}
