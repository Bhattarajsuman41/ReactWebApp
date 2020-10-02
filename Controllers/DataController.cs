using ReactWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReactWebApp.Controllers
{
    public class DataController : Controller
    {
        public IList<FoodItem> menuItems;
        // GET: Data
       
        [HttpGet] 
        public ActionResult GetMenuList()
        {
            menuItems = new List<FoodItem>();
            using (var db = new AppDbContext())
            {
                foreach(var f in db.FoodItems)
                {
                    menuItems.Add(f);
                }
            }

            return Json(menuItems, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        [AuthorizeNadia]
        public string GetUserId()
        {
            int uid = -1;
            if (Session["UserId"] != null)
                uid = Convert.ToInt32(Session["UserId"].ToString());
            return uid.ToString();
        }

        [HttpPost]
        [AuthorizeNadia]
        public ActionResult PlaceOrder(IList<FoodItem> items, int id)
        {
            bool dbSuccess = false;
            try
            {
                using (var db = new AppDbContext())
                {
                    Order o = new Order();
                    o.CustomerId = id;
                    o.OrderDate = DateTime.Now;

                    db.Orders.Add(o);
                    db.SaveChanges();

                    int orderId = o.Id;
                    decimal grandTotal = 0;
                    foreach (var f in items)
                    {
                        OrderDetail orderDetail = new OrderDetail
                        {
                            OrderId = orderId,
                            FoodItemId = f.Id,
                            Quantity = f.Quantity,
                            TotalPrice = f.Price * f.Quantity,
                        };
                        db.OrderDetails.Add(orderDetail);
                        grandTotal += orderDetail.TotalPrice;
                    }

                    o.TotalPaid = grandTotal;
                    o.Status = 1;
                    o.OrderDate = DateTime.Now;
                    db.SaveChanges();
                    dbSuccess = true;
                }
            }
            catch(Exception ex)
            {
                //log ex
                dbSuccess = false; 
            }

            if (dbSuccess)
                return Json("success: true", JsonRequestBehavior.AllowGet);
            else
                return Json("success: false", JsonRequestBehavior.AllowGet);

        }
    }
    
    public class AuthorizeNadia : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");

            // Make sure the user logged in.
            if (httpContext.Session["Email"] == null)
            {
                return false;
            }

            // Do you own custom stuff here
            // Check if the user is allowed to Access resources;

            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (this.AuthorizeCore(filterContext.HttpContext) == false)
            {
                filterContext.Result = new RedirectResult("/Account/Login/?ret=" + filterContext.HttpContext.Request.CurrentExecutionFilePath);
            }
        }
    }

}