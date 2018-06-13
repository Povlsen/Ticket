using Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("API/Shop")]
    public class ShopController : ApiController
    {
        [HttpPost]
        [Route("Post")]
        public HttpResponseMessage shopPost([FromBody] Shops shopInfo)
        {
            try
            {
                Shops newShop;
                using (TicketDbEntities db = new TicketDbEntities())
                {
                    var shop = db.Shops.Where(x => x.Id == shopInfo.Id).FirstOrDefault();
                    if (shop == null)
                    {
                        shop = shopInfo;
                        db.Shops.Add(shopInfo);
                        db.SaveChanges();
                        newShop = db.Shops.Where(x => x.Name == shopInfo.Name).LastOrDefault();
                    }
                    else
                    {
                        shop.Name = shopInfo.Name;
                        db.SaveChanges();
                        newShop = db.Shops.Where(x => x.Id == shopInfo.Id).FirstOrDefault();
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, newShop);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Get/{shopId}")]
        public HttpResponseMessage ShopGetOrders([FromUri] int shopId)
        {
            try
            {
                Shops shop;
                using (TicketDbEntities db = new TicketDbEntities())
                {
                    shop = db.Shops.Where(x => x.Id == shopId).FirstOrDefault();
                }

                return Request.CreateResponse(HttpStatusCode.OK, shop);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

    }
}
