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
    public class EmployeesController : ApiController
    {
        NorthwindEntities db = new NorthwindEntities();


        public List<EmployeeVM> GetEmployees()
        {
            //Ali: ReporstTo alanını gördüğü anda atlasın. kod akışına devam etmesin.
            //İrem: Report to atlansın.


            //List<EmployeeVM> employeeVMs = new List<EmployeeVM>();

            //foreach (Employee employee in db.Employees)
            //{
            //    EmployeeVM employeeVM = new EmployeeVM();
            //    employeeVM.EmployeeID = employee.EmployeeID;
            //    employeeVM.FirstName = employee.FirstName;
            //    employeeVM.Surname = employee.LastName;
            //    employeeVM.Title = employee.Title;

            //    employeeVMs.Add(employeeVM);

            //}




            var employees = db.Employees.Select(x => new EmployeeVM
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                Surname = x.LastName,
                Title = x.Title,
                ReportTo = x.ReportsTo

            });




            return employees.ToList();
        }

        public HttpResponseMessage GetById(int id)
        {
            EmployeeVM employee = GetEmployees().Find(x => x.EmployeeID == id);

            if (employee == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"{id} olan herhangi bir çalışan bulunamadı!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, employee);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostEmployee([FromBody] EmployeeVM employeeVM)
        {
            try
            {
                Employee employee = new Employee
                {
                    FirstName = employeeVM.FirstName,
                    LastName = employeeVM.Surname,
                    Title = employeeVM.Title

                };

                db.Employees.Add(employee);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, employee);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public HttpResponseMessage DeleteEmployee(int id)
        {
            try
            {
                Employee employee = db.Employees.Find(id);
                db.Employees.Remove(employee);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "çalışan silindi!");

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public HttpResponseMessage PutEmployee(int id,[FromBody] EmployeeVM employeeVM)
        {
            try
            {
                var updated = db.Employees.Find(id);
                if (updated != null)
                {
                    updated.FirstName = employeeVM.FirstName;
                    updated.LastName = employeeVM.Surname;
                    updated.Title = employeeVM.Title;

                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, $"{updated.FirstName} {updated.LastName} olarak değiştirildi!");

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "çalışan bulunamadı!");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
