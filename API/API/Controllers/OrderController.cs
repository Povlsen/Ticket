using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Db;

namespace API.Controllers
{
    [RoutePrefix("API/Order")]
    public class OrderController : ApiController
    {
        [HttpPost]
        [Route("Post")]
        public HttpResponseMessage orderPost([FromBody] Orders orderInfo)
        {
            try
            {
                Orders newOrder;
                using (TicketDbEntities db = new TicketDbEntities())
                {
                    var order = db.Orders.Where(x => x.Id == orderInfo.Id).FirstOrDefault();
                    if (order == null)
                    {
                        order = orderInfo;
                        db.Orders.Add(orderInfo);
                        db.SaveChanges();
                        newOrder = db.Orders.Where(x => x.ShopId == orderInfo.ShopId && x.RegNum == orderInfo.RegNum && x.Description == orderInfo.Description).LastOrDefault();
                    }
                    else
                    {
                        order.Description = orderInfo.Description;
                        order.RegNum = orderInfo.RegNum;
                        order.State = orderInfo.State;
                        db.SaveChanges();

                        newOrder = db.Orders.Where(x => x.Id == orderInfo.Id).FirstOrDefault();
                    }                                    
                }

                return Request.CreateResponse(HttpStatusCode.OK, newOrder);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Get/Shop/{ShopId}")]
        public HttpResponseMessage ShopGetOrders([FromUri] int shopId)
        {
            try
            {
                var orders = new List<Orders>();
                using (TicketDbEntities db = new TicketDbEntities())
                {
                    orders = db.Orders.Where(x => x.ShopId == shopId).ToList();                    
                }

                return Request.CreateResponse(HttpStatusCode.OK, orders);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Get/Client/{UserId}")]
        public HttpResponseMessage ClientGetOrders([FromUri] int userId)
        {
            try
            {
                var orders = new List<Orders>();
                using (TicketDbEntities db = new TicketDbEntities())
                {
                    orders = db.Orders.Where(x => x.CreatedBy == userId).ToList();
                }

                return Request.CreateResponse(HttpStatusCode.OK, orders);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }     
    }
}
