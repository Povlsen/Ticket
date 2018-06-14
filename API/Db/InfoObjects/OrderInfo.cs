using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.InfoObjects
{
    public class OrderInfo
    {
        public int Id { get; set; }

        public int CreatedByUserId { get; set; }
        public string CreatedByName { get; set; }

        public string Description { get; set; }

        public int State { get; set; }
        public string StateName { get; set; }

        public string RegNum { get; set; }

        public System.DateTime Created { get; set; }

        public int ShopId { get; set; }
        public string ShopName { get; set; }

        public string Model { get; set; }
    }
}
