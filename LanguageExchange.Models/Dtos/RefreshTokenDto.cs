using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Models.Dtos
{
    public class RefreshTokenDto
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string Subject { get; set; }
        public string Token { get; set; }
    }
}
