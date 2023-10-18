using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using ThucHanh.Library;

namespace ThucHanh.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();   
        /// ////////////////////////////////////////////////////////////
     
      
        // GET: Admin/Category/Index
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }
        /// ////////////////////////////////////////////////////////////

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        ///// ////////////////////////////////////////////////////////////

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"),"Id","Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac tuong sau:
                //---CreateAt
                categories.CreateAt = DateTime.Now;
                //--CreateBy
                categories.CreateBy = Convert.ToInt32(Session["UserID"]);
                //slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentID
                if(categories.ParentID == null)
                {
                    categories.ParentID = 0;
                }
                //Order
                if(categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //UpdateAt
                categories.UpdateAt = DateTime.Now;
                //UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);

                categoriesDAO.Insert(categories);
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success","Tạo mới loại sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        ///// ////////////////////////////////////////////////////////////

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Slug,ParentID,Order,MetaDesc,MetaKey,CreateBy,CreateAt,UpdateBy,UpdateAt,Status")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                categoriesDAO.Update(categories);
                return RedirectToAction("Index");
            }

            return View(categories);
        }

        ///// ////////////////////////////////////////////////////////////

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories =categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        /// ////////////////////////////////////////////////////////////

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            categoriesDAO.Delate(categories);
            return RedirectToAction("Index");
        }

        // GET: Admin/Category/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            
            //cap nhat trang thai
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //cap nhat UpdateAt
            categories.UpdateAt = DateTime.Now;
            //cap nhat UpdateBy
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update Db
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //tro ve trang inddex
            return RedirectToAction("Index");
        }
    }
}
