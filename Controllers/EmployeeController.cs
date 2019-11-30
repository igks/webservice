using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EntityFrameworkPaginate;
using WebAPI.Models;
using PagedList;

namespace WebAPI.Controllers
{
    public class EmployeeController : ApiController
    {
        private EmployeeDB db = new EmployeeDB();

        // GET: api/Employee
        public IQueryable<Employee> GetEmployees(string searchText, int pageNumber, int pageSize )
        {
            var employees = from e in db.Employees select e;
            int takeList = pageSize;
            int skippedList = pageSize * pageNumber;
            
            if (!String.IsNullOrEmpty(searchText))
            {
                employees = employees.Where(e => e.FullName.Contains(searchText));
            }

            return employees.Skip(skippedList).Take(takeList);
        }

        //public IQueryable<Employee> GetEmployees(int pageSize, int currentPage, string searchText )
        /* public IQueryable<Employee> GetEmployees(int pageSize, int currentPage, string searchText)
         {
             var filters = new Filters<Employee>();
             filters.Add(!string.IsNullOrEmpty(searchText), x => x.Contains(searchText));

             using (var context = new AdventureWorksEntities()) { 

 }


             return db.Employees;
         }*/

        // GET: api/Employee/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult GetEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employee/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {

            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
  
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employee
        [ResponseType(typeof(Employee))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            db.Employees.Add(employee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeID }, employee);
        }

        // DELETE: api/Employee/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeID == id) > 0;
        }
    }
}