using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net;
using System.Net.Http;
using Db;

namespace API
{
    public class Authentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null || !actionContext.Request.Headers.Authorization.ToString().Contains("Basic "))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else if (actionContext.Request.Headers.Authorization.ToString().Contains("Basic "))
            {
                using (TicketDbEntities data = new TicketDbEntities())
                {
                    var token = actionContext.Request.Headers.Authorization.ToString().Replace("Basic ", "");
                    if (!data.Tokens.Where(x => x.Token == token && DateTime.Compare(x.ExpireDate, DateTime.Now) > 0).Any())
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }

                }
            }
        }
    }
}