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
    public static class SessionExtensions
    {
        public static T GetComplexData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetComplexData(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }

    public class UserController : Controller
    {
        IEnumerable<Employee> employee = null;
        public PaginatedList<Employee> employeeList { get; private set; }

        const string EmployeeListSessionName = "_EmployeeList";

        public IActionResult ManageUser()
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
                    var myEmployeeList = employee.ToList();
                    employeeList = PaginatedList<Employee>.Create(employee, 1, 4, 5);

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

            return View(employeeList);
        }



        public IActionResult AddUser()
        {
            return View();
        }

        public IActionResult EditUser()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult Search()
        {

            return View("ManageUser");
        }

        public IActionResult SetPage(string pageIndex)
        {
            //int pageIndex = int.Parse(RouteData.Values["pageIndex"].ToString());
            int pageIn = int.Parse(pageIndex);
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(EmployeeListSessionName)))
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
                        var myEmployeeList = employee.ToList();
                        employeeList = PaginatedList<Employee>.Create(employee, pageIn, 4, 5);

                        // store employee in session
                        HttpContext.Session.SetComplexData(EmployeeListSessionName, myEmployeeList);
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        employee = Enumerable.Empty<Employee>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }


                }

            }
            else
            {
                employee = HttpContext.Session.GetComplexData<IEnumerable<Employee>>(EmployeeListSessionName);

                employeeList = PaginatedList<Employee>.Create(employee, pageIn, 4, 5);

            }
            

            

            return View("ManageUser",employeeList);
        }

    }
}