using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Db;
using Db.Enums;
using Db.InfoObjects;

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

                var resOrder = toOrdetInfo(new List<Orders>() { newOrder }).First();
                return Request.CreateResponse(HttpStatusCode.OK, resOrder);
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
                var tempOrders = new List<Orders>();
                using (TicketDbEntities db = new TicketDbEntities())
                {
                    tempOrders = db.Orders.Where(x => x.ShopId == shopId).ToList();                    
                }

                var orders = toOrdetInfo(tempOrders);
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
                var tempOrders = new List<Orders>();
                using (TicketDbEntities db = new TicketDbEntities())
                {
                    tempOrders = db.Orders.Where(x => x.CreatedBy == userId).ToList();
                }

                var orders = toOrdetInfo(tempOrders);
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
                OrderInfo order;
                using (TicketDbEntities db = new TicketDbEntities())
                {
                    var tempOrder = db.Orders.Where(x => x.Id == orderId).FirstOrDefault();
                    if (tempOrder == null)
                        return Request.CreateResponse(HttpStatusCode.BadRequest);

                    order = toOrdetInfo(new List<Orders>() { tempOrder }).First();
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, order);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        private List<OrderInfo> toOrdetInfo(List<Orders> orders)
        {
            var res = new List<OrderInfo>();
            var users = new List<Users>();
            var shops = new List<Shops>();

            using (TicketDbEntities db = new TicketDbEntities())
            {
                var tempUserIds = orders.Select(y => y.CreatedBy).ToList();
                var tempShopIds = orders.Select(y => y.ShopId).ToList();

                users = db.Users.Where(x => tempUserIds.Contains(x.Id)).ToList();
                shops = db.Shops.Where(x => tempShopIds.Contains(x.Id)).ToList();
            }

            foreach (var o in orders)
            {
                var rO = new OrderInfo();
                rO.Id = o.Id;
                rO.Model = o.Model;
                rO.RegNum = o.RegNum;
                rO.ShopId = o.ShopId;
                rO.ShopName = shops.FirstOrDefault(x => x.Id == o.ShopId).Name;
                rO.State = o.State;
                rO.StateName = Convert.ToString((OrderStates)o.State);
                rO.Created = o.Created;
                rO.CreatedByName = users.FirstOrDefault(x => x.Id == o.CreatedBy).Name;
                rO.CreatedByUserId = o.CreatedBy;
                rO.Description = o.Description;

                res.Add(rO);
            }

            return res;
        }
    }
}
