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
            List<Menus> list = menusDAO.getListByParentId(0,"MainMenu");
            return View("MainMenu",list);
        }
        ///////////////////////////////////////////////////////////////////////////
        // GET: MainmenuSub
        public ActionResult MainMenuSub(int id)
        {
            List<Menus> list = menusDAO.getListByParentId(id, "MainMenu");
            //tra ve dong hien hanh cua menu co id = id
            Menus menus = menusDAO.getRow(id);
            if (list.Count == 0)//menu khong co cap con
            {
                return View("MainMenuSub_0", menus);
            }
            else//menu co cap con
            {
                ViewBag.Menu2cap = menus;
                return View("MainMenuSub_1", list);
            }
        }
    }
}