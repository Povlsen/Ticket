using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Db;
using Db.Enums;

namespace API.Controllers
{
    [RoutePrefix("API/Order")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
                        order.Created = DateTime.Now;
                        order.ShopId = 1; // default for now
                        order.State = (int)OrderStates.newOrder;

                        if (orderInfo.CreatedBy <= 0)
                        {
                            order.CreatedBy = new Helpers().getUserIdFromRequest();
                        }

                        db.Orders.Add(orderInfo);
                        db.SaveChanges();
                        newOrder = db.Orders.Where(x => x.CreatedBy == order.CreatedBy && x.RegNum == order.RegNum && x.Model == order.Model && x.ShopId == order.ShopId && x.Description == order.Description).OrderByDescending(y => y.Created).FirstOrDefault();
                    }
                    else
                    {
                        order.Description = orderInfo.Description;
                        order.RegNum = orderInfo.RegNum;
                        order.State = orderInfo.State;
                        order.Model = orderInfo.Model;
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

        [HttpGet]
        [Route("Get/{orderId}")]
        public HttpResponseMessage GetOrder([FromUri] int orderId)
        {
            try
            {
                Orders order;
                using (TicketDbEntities db = new TicketDbEntities())
                {
                    order = db.Orders.Where(x => x.Id == orderId).FirstOrDefault();
                }

                return Request.CreateResponse(HttpStatusCode.OK, order);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
