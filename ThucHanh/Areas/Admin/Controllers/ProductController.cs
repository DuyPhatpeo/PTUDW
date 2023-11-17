using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using ThucHanh.Library;

namespace ThucHanh.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductsDAO productsDAO = new ProductsDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View(productsDAO.getList("Index"));
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products== null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");//category
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "ID", "Name");//supplier
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products)
        {
            if (ModelState.IsValid)
            {
                // xu ly tu dong cho mot so truong
                //----Create At
                products.CreateAt = DateTime.Now;
                //----Create By
                products.CreateBy = Convert.ToInt32(Session["UserID"]);
                //slug
                products.Slug = XString.Str_Slug(products.Name);
                //----Update At
                products.UpdateAt = DateTime.Now;
                //----Update By
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //xu ly cho muc hinh anh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                // thong bao them moi thanh cong
                productsDAO.Insert(products);
                TempData["message"] = new XMessage("success", "Thêm mới sản phẩm thành công");
                return RedirectToAction("Index");

            }
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");//category
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "ID", "Name");//supplier
            return View(products);
        }
        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");//category
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "ID", "Name");//supplier
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                // xu ly tu dong cho mot so truong
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //cap nhat thi phai xoa file cu
                        //Xoa file
                        if (products.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), products.Image);
                            System.IO.File.Delete(DelPath);
                        }

                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                //slug
                products.Slug = XString.Str_Slug(products.Name);
                //----Update At
                products.UpdateAt = DateTime.Now;
                //----Update By
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //xu ly cho muc hinh anh

                // cap nhat mau tin
                productsDAO.Update(products);
                //thong báo
                TempData["message"] = new XMessage("success", "Cập nhật sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");//category
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "ID", "Name");//supplier
            return View(products);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");

            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
            }
            return View(products);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = productsDAO.getRow(id);
            if (productsDAO.Delete(products) == 1)
            {
                //duong dan den anh can xoa
                string PathDir = "~/Public/img/product/";
                //cap nhat thi phai xoa file cu
                if (products.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), products.Image);
                    System.IO.File.Delete(DelPath);
                }
            }
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa sản phẩm thành công");
            return RedirectToAction("Trash");
        }

        //// GET: Admin/Product/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            products.Status = (products.Status == 1) ? 2 : 1;
            //cap nhat Update At
            products.UpdateAt = DateTime.Now;
            //cap nhat Update By
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            productsDAO.Update(products);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật loại sản phẩm thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }
        //// GET: Admin/Products/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            products.Status = 0;
            //cap nhat Update At
            products.UpdateAt = DateTime.Now;
            //cap nhat Update By
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            productsDAO.Update(products);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }
        /////////////////////////////////////////////////////// 
        // GET: Admin/Product/Trash
        public ActionResult Trash()
        {
            return View(productsDAO.getList("Trash"));
        }
        //// GET: Admin/Category/Undo/5
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            products.Status = 2;
            //cap nhat Update At
            products.UpdateAt = DateTime.Now;
            //cap nhat Update By
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            productsDAO.Update(products);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Trash");//ở lại thùng rác tiếp tục Undo
        }
    }
}
