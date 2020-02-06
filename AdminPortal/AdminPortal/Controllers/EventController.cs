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
    public class EventController : Controller
    {

        IEnumerable<Event> events = null;
        const string EventListSessionName = "_EventList";
        const string tokenSession = "tokenSessionObject";
        public async Task<IActionResult> ManageEvents()
        {

            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            List<Event> eventList = new List<Event>();
            var tokenObj = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString(tokenSession));
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
                    var responseTask = client.GetAsync("events?deleted=false");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<Event>>();
                        readTask.Wait();

                        events = readTask.Result;
                        eventList = events.ToList();

                        // store employee in session
                        HttpContext.Session.SetComplexData(EventListSessionName, eventList);
                        //HttpContext.Session["employeeList"] = employeeList;
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        events = Enumerable.Empty<Event>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }


                }
            }
            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(eventList);
        }

        public IActionResult AddEvent()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public IActionResult EditEvent()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public IActionResult EditDeletedEvent()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        IEnumerable<Event> deletedEvents = null;
        const string DeletedEventListSessionName = "_DeletedEventList";
        public async Task<IActionResult> ManageDeletedEvents ()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            List<Event> eventList = new List<Event>();
            var tokenObj = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString(tokenSession));
            var token = tokenObj.Token1;
            AccountController account = new AccountController();
            bool tokenIsValid = await account.tokenIsValid(tokenObj, HttpContext);

            if (tokenIsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                    //HTTP GET
                    var responseTask = client.GetAsync("events?deleted=true");
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<Event>>();
                        readTask.Wait();

                        deletedEvents = readTask.Result;
                        eventList = deletedEvents.ToList();

                        // store employee in session
                        HttpContext.Session.SetComplexData(DeletedEventListSessionName, eventList);
                        //HttpContext.Session["employeeList"] = employeeList;
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        deletedEvents = Enumerable.Empty<Event>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }


                }
            }
            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(eventList);
        }
    }
}