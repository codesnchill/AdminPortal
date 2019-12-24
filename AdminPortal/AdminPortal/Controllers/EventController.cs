﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models;

namespace AdminPortal.Controllers
{
    public class EventController : Controller
    {

        IEnumerable<Event> events = null;
        public IActionResult ManageEvents()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("events");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Event>>();
                    readTask.Wait();

                    events = readTask.Result;
                   
                }
                else //web api sent error response 
                {
                    //log response status here..

                    events = Enumerable.Empty<Event>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            return View(events);
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