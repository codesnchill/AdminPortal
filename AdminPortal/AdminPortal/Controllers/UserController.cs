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

        const string EmployeeListSessionName = "_EmployeeList";
        const string tokenSession = "tokenSessionObject";
        public async Task<IActionResult> ManageUser()
        {
            List<Employee> myEmployeeList = new List<Employee>();

            Token tokenObj = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString(tokenSession));
            var token = tokenObj.Token1;

            AccountController account = new AccountController();
            bool tokenIsValid = await account.tokenIsValid(tokenObj, HttpContext);

            if (tokenIsValid)
            {
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
            }
            
            //else clear session and logout the user

            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(myEmployeeList);
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

        IEnumerable<Employee> deletedEmployee = null;

        const string DeletedEmployeeListSessionName = "_DEmployeeList";
        public async Task<IActionResult> ManageDeletedUser()
        {
            List<Employee> myEmployeeList = new List<Employee>();
            Token tokenObj = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString(tokenSession));
            var token = tokenObj.Token1;

            AccountController account = new AccountController();
            bool tokenIsValid = await account.tokenIsValid(tokenObj, HttpContext);

            if (tokenIsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    //HTTP GET
                    var responseTask = client.GetAsync("users?isDisabled=true");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<Employee>>();
                        readTask.Wait();

                        deletedEmployee = readTask.Result;
                        myEmployeeList = deletedEmployee.ToList();

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
            }
            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(myEmployeeList);
        }


     


    }
}