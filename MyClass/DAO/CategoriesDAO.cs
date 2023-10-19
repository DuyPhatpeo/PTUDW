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

        public List<Categories> getlist()
        {
            return db.Categories.ToList();
        }
        // INDEX dua vao Status = 1,2, con Status = 0 == thung rac
        public List<Categories> getList(string status = "ALL")
        {
            List<Categories> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Categories
                            .Where(m => m.Status !=0)
                            .ToList ();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Categories
                            .Where(m => m.Status != 0)
                            .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Categories.ToList ();
                        break;
                    }
            }
            return list;
        }

        //DETAILS
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

        //CREATE
        public int Insert(Categories row)
        {
            db.Categories.Add(row);
            return db.SaveChanges();
        }

        //UPDATE
        public int Update(Categories row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //DELETE
        public int Delete(Categories row)
        {

            db.Categories.Remove(row);
            return db.SaveChanges();
        }
    }

}