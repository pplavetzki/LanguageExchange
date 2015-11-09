using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace LanguageExchange.Repository
{
    public class DocumentRepository
    {
        private readonly DocumentClient _clientDb;

        public DocumentRepository(DocumentClient clientDb)
        {
            _clientDb = clientDb;
        }
    }
}
