using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WEbApiNorthwind.Models.ViewModels
{
    public class ProductsController : ApiController
    {
        NorthwindEntities db = new NorthwindEntities();

        public List<ProductVM> GetProducts()
        {
            var products = db.Products.Select(x => new ProductVM
            {
                ProductId = x.ProductID,
                ProductName = x.ProductName,
                UnitPrice = (short)x.UnitPrice,
                UnitsInStock = (short)x.UnitsInStock
            });

            return products.ToList();

        }

        public HttpResponseMessage GetById(int id)
        {
            ProductVM product = GetProducts().Find(x => x.ProductId == id);

            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"{id} olan herhangi bir ürün bulunamadı!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostProducts([FromBody] ProductVM productVM)
        {
            try
            {
                Product product = new Product
                {
                    ProductID = productVM.ProductId,
                    ProductName = productVM.ProductName,
                    UnitPrice = productVM.UnitPrice,
                    UnitsInStock = productVM.UnitsInStock

                };

                db.Products.Add(product);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, product);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public HttpResponseMessage DeleteProducts(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Ürün silindi!");

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public HttpResponseMessage PutProduct(int id, [FromBody] ProductVM productVM)
        {
            try
            {
                var updated = db.Products.Find(id);
                if (updated != null)
                {
                    updated.ProductName = productVM.ProductName;
                    updated.UnitPrice = productVM.UnitPrice;
                    updated.UnitsInStock = productVM.UnitsInStock;

                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, $"{updated.ProductName} olarak değiştirildi!");

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ürün bulunamadı!");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


    }
}
