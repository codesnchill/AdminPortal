using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AdminPortal.Controllers
{
    
    public class PointController : Controller
    {
        IEnumerable<Employee> employee = null;
        public PaginatedList<Employee> employeeList { get; private set; }
        public IActionResult ManagePoints()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("users");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Employee>>();
                    readTask.Wait();

                    employee = readTask.Result;

                    employeeList = PaginatedList<Employee>.Create(employee, 1, 4, 5);

                    //HttpContext.Session["employeeList"] = employeeList;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    employee = Enumerable.Empty<Employee>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            return View(employeeList);
        }

        public IActionResult AwardPoint()
        {
            return View();
        }

        public IActionResult DeductPoint()
        {
            return View();
        }

        public IActionResult SetPage(string pageIndex)
        {
            //int pageIndex = int.Parse(RouteData.Values["pageIndex"].ToString());
            int pageIn = int.Parse(pageIndex);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("users");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Employee>>();
                    readTask.Wait();

                    employee = readTask.Result;

                    employeeList = PaginatedList<Employee>.Create(employee, pageIn, 4, 5);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    employee = Enumerable.Empty<Employee>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }
            return View("ManagePoints", employeeList);
        }
    }
}