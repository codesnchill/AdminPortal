using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models;
using System.Windows;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace AdminPortal.Controllers
{
    
    public class PointController : Controller
    {
        IEnumerable<Employee> employee = null;

        const string EmployeeListSessionName = "_EmployeeList";
        const string tokenSession = "tokenSessionObject";
        public IActionResult ManagePoints()
        {
            List<Employee> myEmployeeList = new List<Employee>();
            var tokenObj = JsonConvert.DeserializeObject<dynamic>(HttpContext.Session.GetString(tokenSession));
            var token = tokenObj.Token1;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                //HTTP GET
                var responseTask = client.GetAsync("users?isDisabled=false");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Employee>>();
                    readTask.Wait();

                    employee = readTask.Result;
                    myEmployeeList = employee.ToList();

                    // store employee in session
                    HttpContext.Session.SetComplexData(EmployeeListSessionName, myEmployeeList);
                    //HttpContext.Session["employeeList"] = employeeList;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    employee = Enumerable.Empty<Employee>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(myEmployeeList);
        }

        public IActionResult AwardPoint()
        {
            return View();
        }

        public IActionResult DeductPoint()
        {
            return View();
        }

       
    }
}