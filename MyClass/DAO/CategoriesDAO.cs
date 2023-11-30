using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MyClass.Model;

namespace MyClass.DAO
{

    public class CategoriesDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Categories> getList()
        {
            return db.Categories.ToList();
        }
        // GET: Admin/Category
        public List<Categories> getListByPareantId(int parentid = 0)
        {
            return db.Categories
                .Where(m => m.ParentID == parentid && m.Status == 1)
                .OrderBy(m => m.Order)
                .ToList();
        }

        //INDEX dua vao status = 1,2, còn status = 0 == thung rac
        public List<Categories> getList(string status = "All")
        {
            List<Categories> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Categories.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Categories.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        return db.Categories.ToList();
                    }
            }
            return list;
        }
        // Details
        public Categories getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Categories.Find(id);
            }
        }

        public Categories getRow(string slug)
        {

            return db.Categories
                .Where(m => m.Slug == slug && m.Status == 1)
                .FirstOrDefault();
        }
        // Create 
        public int Insert(Categories row)
        {
            db.Categories.Add(row);
            return db.SaveChanges();

        }
        //Update
        public int Update(Categories row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Delete
        public int Delete(Categories row)
        {
            db.Categories.Remove(row);
            return db.SaveChanges();
        }
    }

}