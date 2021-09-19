using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WEbApiNorthwind.Models;
using WEbApiNorthwind.Models.ViewModels;

namespace WEbApiNorthwind.Controllers
{
    public class CategoriesController : ApiController
    {
        NorthwindEntities db = new NorthwindEntities();

        public List<CategoryVM> GetCategories()
        {
            var categories = db.Categories.Select(x => new CategoryVM
            {
                CategoryID = x.CategoryID,
                CategoryName = x.CategoryName,
                Description = x.Description
            });

            return categories.ToList();
        }

        public HttpResponseMessage GetById(int id)
        {
            CategoryVM category = GetCategories().Find(x => x.CategoryID == id);

            if (category == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $" {id} adında bir kategori bulunamadı ");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostCategory([FromBody] CategoryVM categoryVM)
        {

            try
            {
                Category category = new Category
                {
                    CategoryID = categoryVM.CategoryID,
                    CategoryName = categoryVM.CategoryName,
                    Description = categoryVM.Description

                };
                db.Categories.Add(category);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, category);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }


        }

        public HttpResponseMessage DeleteCategory(int id)
        {

            try
            {
                Category category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, $"Kategori silindi");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        public HttpResponseMessage PutCategory(int id, [FromBody] CategoryVM categoryVM)
        {

            try
            {
                var updated = db.Categories.Find(id);
                if (updated == null)
                {
                    updated.CategoryID = categoryVM.CategoryID;
                    updated.CategoryName = categoryVM.CategoryName;
                    updated.Description = categoryVM.Description;

                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, $"{id} kategorisi eklendi! ");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Kategori bulunamadı!");
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

            }

        }
    }
}
