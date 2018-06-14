using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Enums
{
    public enum UserTypes
    {
        client = 0,
        shopOwners = 1
    }

    public enum OrderStates
    {
        newOrder = 0,
        booked = 1,
        onHold = 2,
        finished = 3
    }
}
