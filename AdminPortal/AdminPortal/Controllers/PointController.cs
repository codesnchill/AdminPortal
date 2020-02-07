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
using AdminPortal.Utils;

namespace AdminPortal.Controllers
{
    
    public class PointController : Controller
    {
        IEnumerable<Employee> employee = null;

        const string EmployeeListSessionName = "_EmployeeList";
        const string tokenSession = "tokenSessionObject";
        string baseUrl = BaseURLHelper.GetBaseURL();

        public async Task<IActionResult> ManagePoints()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            List<Employee> myEmployeeList = new List<Employee>();
            var tokenObj = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString(tokenSession));
            var token = tokenObj.Token1;
            AccountController account = new AccountController();
            bool tokenIsValid = await account.tokenIsValid(tokenObj, HttpContext);

            if (tokenIsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl + "/api/v1/");
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
            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(myEmployeeList);
        }

        public IActionResult AwardPoint()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public IActionResult DeductPoint()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

       
    }
}