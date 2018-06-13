using Db;
using Db.InfoObjects;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    
    [RoutePrefix("API/User")]
    public class UserController : ApiController
    {
        [OverrideAuthorization]
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

        [OverrideAuthorization]
        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage Login([FromBody] LoginInfo info)
        {
            try
            {
                using (TicketDbEntities data = new TicketDbEntities())
                {
                    var user = data.Users.Where(x => (x.Email == info.userName || x.UserName == info.userName) && x.Password == info.password).FirstOrDefault();
                    if (user != null)
                    {
                        string t = null;
                        DateTime compare = DateTime.Now.AddHours(2);
                        try
                        {
                            t = data.Tokens.Where(x => x.UserId == user.Id && x.ExpireDate > compare).FirstOrDefault().Token;
                        }
                        catch
                        {

                        }

                        if (t == null)
                        {
                            Tokens token = new Tokens();
                            token.UserId = user.Id;
                            token.Token = GetToken();
                            token.ExpireDate = DateTime.Now.AddDays(1);
                            data.Tokens.Add(token);
                            data.SaveChanges();

                            t = token.Token;
                        }

                        LoginResponse response = new LoginResponse();
                        response.Name = user.Name;
                        response.UserId = user.Id;
                        response.Token = t;
                        
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        private string GetToken()
        {
            Random random = new Random();
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!#&/()=?@${[]}|+-£%¤*^<>½§_";
            string token;
            while (true)
            {
                token = new string(Enumerable.Repeat(chars, 100).Select(s => s[random.Next(s.Length)]).ToArray());
                using (TicketDbEntities data = new TicketDbEntities())
                {
                    if (!data.Tokens.Where(x => x.Token == token).Any())
                    {
                        break;
                    }
                }
            }
            return token;

        }

    }
}
