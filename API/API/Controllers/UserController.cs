using Db;
using Db.Enums;
using Db.InfoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [RoutePrefix("API/User")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        [OverrideAuthorization]
        [HttpPost]
        [Route("Post")]
        public HttpResponseMessage userPost([FromBody] UserInfo user)
        {
            try
            {
                var errors = new List<string>();

                using (TicketDbEntities db = new TicketDbEntities())
                {
                    Users oldUser = null;

                    if (user.Id > 0)
                    {
                        var userId = new Helpers().getUserIdFromRequest();

                        // eddit user
                        oldUser = db.Users.Where(x => x.Id == user.Id).FirstOrDefault();
                        if (oldUser == null)
                            return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        // new user
                        oldUser = new Users();
                        oldUser.CreatedDate = DateTime.Now;
                    }

                    oldUser.Name = user.Name;
                    oldUser.Phone = user.Phone;
                    oldUser.UserType = user.UserType;
                    oldUser.ShopId = user.ShopId;

                    if (oldUser.Email != user.Email)
                    {
                        if (db.Users.Where(x => x.Email == user.Email).Any())
                        {
                            errors.Add("email alredy used");
                        }

                        if (!isValidEmail(user.Email))
                        {
                            errors.Add("email is not valid");
                        }

                        if (errors.Count == 0)
                            oldUser.Email = user.Email;
                    }

                    var passwordErrors = passwordValidator(user.Password, user.Password2);
                    errors.AddRange(passwordErrors);
                    if (passwordErrors.Count == 0)
                        oldUser.Password = user.Password;

                    if (errors.Count == 0)
                    {
                        if (user.Id <= 0)
                            db.Users.Add(oldUser);

                        db.SaveChanges();
                    }               
                }

                return Request.CreateResponse(HttpStatusCode.OK, errors);
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
                    var user = data.Users.Where(x => x.Email == info.email && x.Password == info.password).FirstOrDefault();
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
                        response.ShopId = user.ShopId;
                        response.Token = t;
                        response.UserType = (UserTypes)user.UserType;
                        
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

        private List<string> passwordValidator(string password1, string password2)
        {
            var errors = new List<string>();

            if (password1 != password2)
                errors.Add("Passwords dosn't match");
            if (password1.Length < 8)
                errors.Add("Passwords not long enougth");
            if (!password1.Any(char.IsUpper))
                errors.Add("Passwords need uppercase");
            if (!password1.Any(char.IsLower))
                errors.Add("Passwords need lowercase");
            if (!password1.Any(char.IsNumber))
                errors.Add("Passwords need number");

            return errors;
        }

        private bool isValidEmail(string email)
        {
            try
            {
                var temp = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
