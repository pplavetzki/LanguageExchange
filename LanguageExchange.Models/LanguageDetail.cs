namespace LanguageExchange.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LanguageDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LanguageDetail()
        {
            CommunicationMethods = new HashSet<LookupCode>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int LanguageId { get; set; }

        public int ProficiencyId { get; set; }

        public short? YearsOfExperience { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public bool WillingToExchange { get; set; }

        [ForeignKey("LanguageId")]
        public virtual LookupCode Language { get; set; }

        [ForeignKey("ProficiencyId")]
        public virtual LookupCode Proficiency { get; set; }

        public virtual UserDetail UserDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LookupCode> CommunicationMethods { get; set; }
    }
}
