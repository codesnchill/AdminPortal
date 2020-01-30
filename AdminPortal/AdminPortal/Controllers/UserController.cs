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
                var responseTask = client.GetAsync("users?isDisabled=false");
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
                    var responseTask = client.GetAsync("users?isDisabled=false");
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




            return View("ManageUser", employeeList);
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

        public IActionResult EditDeletedUser()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Search([FromQuery] EmployeeSearchQuery employeeQuery)
        {
            List<dynamic> employeeList = new List<dynamic>();

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
                        employee = employee.ToList();

                        // store employee in session
                        HttpContext.Session.SetComplexData(EmployeeListSessionName, employee);

                        // do filter
                        if (!string.IsNullOrWhiteSpace(employeeQuery.EmployeeName))
                            employee = employee.Where(u => u.EmployeeName.Contains(employeeQuery.EmployeeName));
                        if (!string.IsNullOrWhiteSpace(employeeQuery.EmployeeId))
                            employee = employee.Where(u => u.EmployeeName.Contains(employeeQuery.EmployeeId));
                        if (!string.IsNullOrWhiteSpace(employeeQuery.DepartmentName))
                            employee = employee.Where(u => u.DepartmentName.Equals(employeeQuery.DepartmentName));
                        if (!string.IsNullOrWhiteSpace(employeeQuery.Roles))
                            employee = employee.Where(u => u.Roles.Equals(employeeQuery.Roles));
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

                // do filter
                if (!string.IsNullOrWhiteSpace(employeeQuery.EmployeeName))
                    employee = employee.Where(u => u.EmployeeName.Contains(employeeQuery.EmployeeName));
                if (!string.IsNullOrWhiteSpace(employeeQuery.EmployeeId))
                    employee = employee.Where(u => u.EmployeeId.Contains(employeeQuery.EmployeeId));
                if (!string.IsNullOrWhiteSpace(employeeQuery.DepartmentName))
                    employee = employee.Where(u => u.DepartmentName.Equals(employeeQuery.DepartmentName));
                if (!string.IsNullOrWhiteSpace(employeeQuery.Roles))
                    employee = employee.Where(u => u.Roles.Equals(employeeQuery.Roles));
            }

            var employeeResult = employee.Select(employee => new
            {
                DepartmentName = employee.DepartmentName,
                EmployeeName = employee.EmployeeName,
                EmployeeBio = employee.EmployeeBio,
                Roles = employee.Roles,
                EmployeeId = employee.EmployeeId,
                IsDisabled = employee.IsDisabled,
                totalPoints = employee.totalPoints
            });

            foreach (var oneResult in employeeResult)
            {
                employeeList.Add(new
                {
                    departmentName = oneResult.DepartmentName,
                    employeeName = oneResult.EmployeeName,
                    employeeBio = oneResult.EmployeeBio,
                    roles = oneResult.Roles,
                    employeeId = oneResult.EmployeeId,
                    isDisabled = oneResult.IsDisabled,
                    totalPoints = oneResult.totalPoints
                });
            }
            return new JsonResult(employeeList);
        }


        IEnumerable<Employee> deletedEmployee = null;
        public PaginatedList<Employee> deletedEmployeeList { get; private set; }

        const string DeletedEmployeeListSessionName = "_DEmployeeList";
        public IActionResult ManageDeletedUser()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("users?isDisabled=true");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Employee>>();
                    readTask.Wait();

                    deletedEmployee = readTask.Result;
                    var myEmployeeList = deletedEmployee.ToList();
                    deletedEmployeeList = PaginatedList<Employee>.Create(deletedEmployee, 1, 4, 5);

                    // store employee in session
                    HttpContext.Session.SetComplexData(DeletedEmployeeListSessionName, myEmployeeList);
                    //HttpContext.Session["employeeList"] = employeeList;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    deletedEmployee = Enumerable.Empty<Employee>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            return View(deletedEmployeeList);
        }


        public IActionResult SetPage2(string pageIndex)
        {
            //int pageIndex = int.Parse(RouteData.Values["pageIndex"].ToString());
            int pageIn = int.Parse(pageIndex);
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(DeletedEmployeeListSessionName)))
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                    //HTTP GET
                    var responseTask = client.GetAsync("users?isDisabled=true");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<Employee>>();
                        readTask.Wait();

                        deletedEmployee = readTask.Result;
                        var myEmployeeList = deletedEmployee.ToList();
                        deletedEmployeeList = PaginatedList<Employee>.Create(deletedEmployee, pageIn, 4, 5);

                        // store employee in session
                        HttpContext.Session.SetComplexData(DeletedEmployeeListSessionName, myEmployeeList);
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        deletedEmployee = Enumerable.Empty<Employee>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }


                }

            }
            else
            {
                deletedEmployee = HttpContext.Session.GetComplexData<IEnumerable<Employee>>(EmployeeListSessionName);

                deletedEmployeeList = PaginatedList<Employee>.Create(employee, pageIn, 4, 5);

            }




            return View("ManageDeletedUser", deletedEmployeeList);
        }



    }
}