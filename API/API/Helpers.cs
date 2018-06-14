using Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace API
{
    public class Helpers
    {
        public int getUserIdFromRequest()
        {
            HttpRequestMessage httpRequestMessage = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
            var token = httpRequestMessage.Headers.Authorization.ToString().Replace("Basic ", "");

            using (TicketDbEntities db = new TicketDbEntities())
            {
                return db.Tokens.Where(x => x.Token == token).FirstOrDefault().UserId;
            }
        }
    }
}