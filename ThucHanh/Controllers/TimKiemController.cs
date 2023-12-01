using MyClass.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThucHanh.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string searchString)
        {
            ViewBag.searchString = searchString;
            ProductsDAO productsDAO = new ProductsDAO();
            var products = productsDAO.getList("Index");
            var product = products.Where(p => p.Name.Contains(searchString));
            if (product != null)
            {
                return View(product);
            }
            return View();
        }
    }
}