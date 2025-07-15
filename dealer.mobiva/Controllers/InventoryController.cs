using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dealer.mobiva.Controllers
{
    public class InventoryController : DefaultController
    {
        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }
    }
}