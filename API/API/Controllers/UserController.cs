using Db;
using Db.InfoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("API/User")]
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateUser([FromBody] Users user)
        {
            try
            {
                using(TicketDbEntities db = new TicketDbEntities())
                {
                    if (db.Users.Where(x => x.Email == user.Email || x.UserName == user.UserName).Count() == 0)
                    {
                        user.CreatedDate = DateTime.Now;
                        db.Users.Add(user);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage Login([FromBody] LoginInfo info)
        {
            try
            {
                using (TicketDbEntities db = new TicketDbEntities())
                {
                    var user = db.Users.Where(x => (x.Email == info.userName || x.UserName == info.userName) && x.Password == info.password).FirstOrDefault();
                    if (user != null)
                    {
                        db.Users.Add(user);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

    }
}
