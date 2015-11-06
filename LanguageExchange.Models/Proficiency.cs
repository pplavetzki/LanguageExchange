using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Models
{
    [Table("LookupCodes")]
    public class Proficiency : LookupCode
    {
    }
}
