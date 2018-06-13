using Db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.InfoObjects
{
    public class UserInfo
    {
        public int Id { get; set; }
        public int UserType { get; set; } = (int)UserTypes.client; // for now the users created onlive will be clients
        public string Password { get; set; }
        public string Password2 { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int ShopId { get; set; } = 1; // the system doesn't handle multipule shops yet
    }
}
