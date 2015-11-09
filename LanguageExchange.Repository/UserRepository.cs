using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Repository
{
    public class UserRepository : DocumentRepository
    {
        public UserRepository(DocumentClient clientDb) : base(clientDb) { }
    }
}
