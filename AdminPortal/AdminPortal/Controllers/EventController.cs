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
        public IActionResult ManageEvents()
        {
            List<Event> eventList = new List<Event>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
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

            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(eventList);
        }

        public IActionResult AddEvent()
        {
            return View();
        }

        public IActionResult EditEvent()
        {
            return View();
        }

      
    }
}