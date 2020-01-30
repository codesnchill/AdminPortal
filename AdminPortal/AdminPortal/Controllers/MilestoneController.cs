using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models;

namespace AdminPortal.Controllers
{
    public class MilestoneController : Controller
    {
        IEnumerable<Milestone> reward = null;
        public PaginatedList<Milestone> rewardList { get; private set; }
        public IActionResult ManageMilestones()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("milestone");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Milestone>>();
                    readTask.Wait();

                    reward = readTask.Result;

                    rewardList = PaginatedList<Milestone>.Create(reward, 1, 4, 5);

                    //HttpContext.Session["employeeList"] = employeeList;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    reward = Enumerable.Empty<Milestone>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            return View(rewardList);
        }

        public IActionResult SetPage(string pageIndex)
        {
            //int pageIndex = int.Parse(RouteData.Values["pageIndex"].ToString());
            int pageIn = int.Parse(pageIndex);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("milestone");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Milestone>>();
                    readTask.Wait();

                    reward = readTask.Result;

                    rewardList = PaginatedList<Milestone>.Create(reward, pageIn, 4, 5);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    reward = Enumerable.Empty<Milestone>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }
            return View("ManageMilestones", rewardList);
        }

        public IActionResult AddReward()
        {
            return View();
        }

        public IActionResult EditReward()
        {
            return View();
        }
    }
}