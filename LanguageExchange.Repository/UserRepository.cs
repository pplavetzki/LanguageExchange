using LanguageExchange.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
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

        public async Task<UserDetail> InsertUser(UserDetail user)
        {
            UserDetail userDocument = null;

            try
            {
                SqlQuerySpec query = new SqlQuerySpec("SELECT u.id FROM Users u WHERE u.Email = @email or u.Username = @username");
                query.Parameters = new SqlParameterCollection();
                query.Parameters.Add(new SqlParameter("@email", user.Email));
                query.Parameters.Add(new SqlParameter("@username", user.Username));

                var document = _clientDb.CreateDocumentQuery("dbs/LanguageExchange/colls/Users", query);
                var retrievedUser = document.AsEnumerable().FirstOrDefault();

                if (retrievedUser == null)
                {
                    var dment = await _clientDb.CreateDocumentAsync("dbs/LanguageExchange/colls/Users", user);
                    userDocument = (dynamic)dment.Resource;
                }
                else
                {
                    throw new Exception("Duplicate username or email address!");
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return userDocument;
        }
    }
}
