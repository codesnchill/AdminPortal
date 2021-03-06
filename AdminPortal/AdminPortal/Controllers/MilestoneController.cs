﻿using System;
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

    public class MilestoneController : Controller
    {
        IEnumerable<Milestone> reward = null;
        const string RewardListSessionName = "_RewardList";
        const string tokenSession = "tokenSessionObject";
        string baseUrl = BaseURLHelper.GetBaseURL();

        public async Task<IActionResult> ManageMilestones()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            List<Milestone> rewardList = new List<Milestone>();
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
                    var responseTask = client.GetAsync("milestone?deleted=false");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<Milestone>>();
                        readTask.Wait();

                        reward = readTask.Result;
                        rewardList = reward.ToList();

                        // store employee in session
                        HttpContext.Session.SetComplexData(RewardListSessionName, rewardList);
                        //HttpContext.Session["employeeList"] = employeeList;
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        reward = Enumerable.Empty<Milestone>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }


                }
            }
            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(rewardList);
        }

        public IActionResult AddReward()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public IActionResult EditReward()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public IActionResult EditDeletedReward()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        IEnumerable<Milestone> deletedReward = null;

        const string DeletedRewardListSessionName = "_DeletedRewardList";
        public async Task<IActionResult> ManageDeletedMilestones()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            List<Milestone> rewardList = new List<Milestone>();
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
                    var responseTask = client.GetAsync("milestone?deleted=true");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<Milestone>>();
                        readTask.Wait();

                        deletedReward = readTask.Result;
                        rewardList = deletedReward.ToList();

                        // store employee in session
                        HttpContext.Session.SetComplexData(DeletedRewardListSessionName, rewardList);
                        //HttpContext.Session["employeeList"] = employeeList;
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        deletedReward = Enumerable.Empty<Milestone>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }


                }
            }
            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(rewardList);
        }
    }
}