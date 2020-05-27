using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;
using NWAPI.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Web.Hosting;
using System.Data;
using System.Linq;
using System.Net;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using System.Dynamic;
using System.Net.Http;

namespace NWAPI.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")] 
    public class ReportsController : ApiController
    {
        [System.Web.Mvc.Route("api/Reports/getReportData")]
        [HttpGet]
        public dynamic getReportData(int categorySelection)
        {
            NorthwindEntities db = new NorthwindEntities();
            db.Configuration.ProxyCreationEnabled = false;
            List<Product> products;

            if(categorySelection!=9)
            {
                products = db.Products.Include(ff => ff.Category).Include(ff => ff.Supplier).Where(xx => xx.CategoryID == categorySelection).ToList();
            }
            else
            {
                products = db.Products.Include(ff => ff.Category).Include(ff => ff.Supplier).ToList();
            }

            return getExpandingReport(products); 
        }

        private dynamic getExpandingReport(List<Product> products)
        {
            dynamic outObject = new ExpandoObject();

            var supList = products.GroupBy(ff => ff.Supplier.CompanyName);
            supList = supList.Where(gg => gg.Sum(ff => ff.UnitsInStock) > 55);
            List<dynamic> sups = new List<dynamic>();
            foreach(var group in supList)
            {
                dynamic Suppliers = new ExpandoObject();
                Suppliers.Key = group.Key;
                Suppliers.TotalUnits = group.Sum(ff => ff.UnitsInStock);
                sups.Add(Suppliers);
            }
            outObject.ChartData = sups;

            var prodList = products.GroupBy(ff => ff.Supplier.CompanyName);
            List<dynamic> prods = new List<dynamic>();
            foreach(var group in prodList)
            {
                dynamic Suppliers = new ExpandoObject();
                Suppliers.Key = group.Key;
                Suppliers.TotalUntis = group.Sum(ff => ff.UnitsInStock);

                List<dynamic> supplierProducts = new List<dynamic>();
                foreach(var product in group)
                {
                    dynamic prodObj = new ExpandoObject();
                    prodObj.Name = product.ProductName;
                    prodObj.Stock = product.UnitsInStock;
                    supplierProducts.Add(prodObj);
                }
                Suppliers.supProducts = supplierProducts;
                prods.Add(Suppliers);
            }
            outObject.TableData = prods;
            return outObject;
        }

        //[System.Web.Mvc.Route("api/Report/getCategories")]
        //[HttpGet]
        //public IHttpActionResult getCategory()
        //{
        //    NorthwindEntities db = new NorthwindEntities();
        //    List<dynamic> dynaCategories = new List<dynamic>();
        //    foreach (Category category in db.Categories)
        //    {
        //        dynamic dynamicCategories = new ExpandoObject();
        //        dynamicCategories.ID = category.CategoryID;
        //        dynamicCategories.Name = category.CategoryName;
        //        dynaCategories.Add(dynamicCategories);
        //    }
        //    return Ok(dynaCategories);
        //}

        [System.Web.Mvc.Route("api/Reports/getCategories")]
        [HttpPost]
        public List<dynamic> getCategories()
        {
            NorthwindEntities db = new NorthwindEntities();
            db.Configuration.ProxyCreationEnabled = false;
            return getCategoryList(db.Categories.ToList());
        }

        public List<dynamic> getCategoryList(List<Category> client)
        {
            List<dynamic> dynaRoles = new List<dynamic>();
            foreach (Category c in client)
            {
                dynamic dynaRole = new ExpandoObject();
                dynaRole.ID = c.CategoryID;
                dynaRole.Description = c.CategoryName;
                dynaRoles.Add(dynaRole);
            }
            return dynaRoles;
        }

    }
}
