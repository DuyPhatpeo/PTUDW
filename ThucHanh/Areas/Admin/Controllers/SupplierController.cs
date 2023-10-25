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
    public class SupplierController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("Index"));
        }

        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong 1 so truong
                //----Create At
                suppliers.CreateAt = DateTime.Now;
                //----Create By
                suppliers.CreateBy = Convert.ToInt32(Session["UserID"]);
                //slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                //UpdateAt
                suppliers.UpdateAt = DateTime.Now;
                //UpdateBy 
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                suppliersDAO.Insert(suppliers);
                TempData["message"] = new XMessage("success", "Thêm mới nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //Xu ly cho muc Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order = suppliers.Order + 1;
                }

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //cap nhat thi phai xoa file cu
                        //Xoa file
                        if (suppliers.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
                            System.IO.File.Delete(DelPath);
                        }

                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Xu ly cho muc UpdateAt
                suppliers.UpdateAt = DateTime.Now;

                //Xu ly cho muc UpdateBy
                suppliers.UpdateBy = Convert.ToInt32(Session["UserId"]);

                suppliersDAO.Update(suppliers);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Sửa danh mục thành công");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Admin/Supplier/Delete/5
        //Xóa hoàn toàn
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại!");
                return RedirectToAction("Trash");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại!");
                return RedirectToAction("Trash");
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliersDAO.Delete(suppliers) == 1)
            {
                //duong dan den anh can xoa
                string PathDir = "~/Public/img/supplier/";
                //cap nhat thi phai xoa file cu
                if (suppliers.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
                    System.IO.File.Delete(DelPath);
                }
            }
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa danh mục thành công");
            return RedirectToAction("Trash");
        }
            //// GET: Admin/Supplier/Status/5
            public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            suppliers.Status = (suppliers.Status == 1) ? 2 : 1;
            //cap nhat Update At
            suppliers.UpdateAt = DateTime.Now;
            //cap nhat Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            suppliersDAO.Update(suppliers);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật loại sản phẩm thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }
        //// GET: Admin/Supplier/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            suppliers.Status = 0;
            //cap nhat Update At
            suppliers.UpdateAt = DateTime.Now;
            //cap nhat Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
             suppliersDAO.Update(suppliers);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }
        /////////////////////////////////////////////////////// 
        // GET: Admin/Category/Trash
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));
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
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            suppliers.Status = 2;
            //cap nhat Update At
            suppliers.UpdateAt = DateTime.Now;
            //cap nhat Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            suppliersDAO.Update(suppliers);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Trash");//ở lại thùng rác tiếp tục Undo
        }
    }
}