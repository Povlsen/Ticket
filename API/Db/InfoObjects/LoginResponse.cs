using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.InfoObjects
{
    public class LoginResponse
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
