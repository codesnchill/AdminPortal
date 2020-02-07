using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdminPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AdminPortal.Utils;

namespace AdminPortal.Controllers
{

    public class ReportController : Controller
    {
        IEnumerable<Milestone> report = null;
        IEnumerable<RankingReport> rankingreport = null;
        IEnumerable<AuditReport> auditreport = null;
        const string tokenSession = "tokenSessionObject";
        string baseUrl = BaseURLHelper.GetBaseURL();

        public IActionResult GenerateReport()
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public async Task<IActionResult> AuditReport(string ReportType, string StartDate, string EndDate)
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            List<AuditReport> auditreportList = new List<AuditReport>();
            var tokenObj = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString(tokenSession));
            var token = tokenObj.Token1;
            var type = ReportType;
            var start = StartDate;
            var end = EndDate;
            AccountController account = new AccountController();
            bool tokenIsValid = await account.tokenIsValid(tokenObj, HttpContext);
            if (tokenIsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl + "/api/v1/");
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    //HTTP GET
                    var responseTask = client.GetAsync("report?ReportType=" + type + "&StartDate=" + start + "&EndDate=" + end);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<AuditReport>>();
                        readTask.Wait();

                        auditreport = readTask.Result;

                        auditreportList = auditreport.ToList();
                        foreach (var i in auditreport)
                        {
                            i.startDate = start;
                            i.endDate = end;
                        }
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        auditreport = Enumerable.Empty<AuditReport>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
                return new ViewAsPdf(auditreportList);
            }
        
        public async Task<IActionResult> MilestoneReport(string ReportType, string StartDate, string EndDate)
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            List<Milestone> reportList = new List<Milestone>();
            var tokenObj = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString(tokenSession));
            var token = tokenObj.Token1;
            //var type = employeeQuery.ReportType;
            //var start = employeeQuery.StartDate;
            //var end = employeeQuery.EndDate;
            var type = ReportType;
            var start = StartDate;
            var end = EndDate;
            AccountController account = new AccountController();
            bool tokenIsValid = await account.tokenIsValid(tokenObj, HttpContext);
            if (tokenIsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl + "/api/v1/");
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    //HTTP GET
                    var responseTask = client.GetAsync("report?ReportType=" + type + "&StartDate=" + start + "&EndDate=" + end);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<Milestone>>();
                        readTask.Wait();

                        report = readTask.Result;
                        reportList = report.ToList();
                        foreach (var i in reportList)
                        {
                            i.startDate = start;
                            i.endDate = end;
                        }
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        report = Enumerable.Empty<Milestone>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
                return new ViewAsPdf(reportList);
            }

        public async Task<IActionResult> RankingReport(string ReportType, string StartDate, string EndDate)
        {
            // check if user goes into a page without logging in
            if (HttpContext.Session.GetString(tokenSession) == null)
                return RedirectToAction("Login", "Account");

            List<RankingReport> rankingreportList = new List<RankingReport>();
            var tokenObj = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString(tokenSession));
            var token = tokenObj.Token1;
            //var type = employeeQuery.ReportType;
            //var start = employeeQuery.StartDate;
            //var end = employeeQuery.EndDate;
            var type = ReportType;
            var start = StartDate;
            var end = EndDate;
            AccountController account = new AccountController();
            bool tokenIsValid = await account.tokenIsValid(tokenObj, HttpContext);
            if (tokenIsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl + "/api/v1/");
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    //HTTP GET
                    var responseTask = client.GetAsync("report?ReportType=" + type + "&StartDate=" + start + "&EndDate=" + end);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<RankingReport>>();
                        readTask.Wait();

                        rankingreport = readTask.Result;
                        rankingreportList = rankingreport.ToList();
                        foreach (var i in rankingreport)
                        {
                            i.startDate = start;
                            i.endDate = end;
                        }
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        rankingreport = Enumerable.Empty<RankingReport>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
                return new ViewAsPdf(rankingreportList);
            }
        }
    }

