using MyClass.DAO;
using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ThucHanh.Controllers
{
    public class ModuleController : Controller
    {
        MenusDAO menusDAO = new MenusDAO();
        // GET: Module
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MainMenu() 
        {
            List<Menus> list = menusDAO.getListByParentId(0);
            return View(list);
        }
    }
}