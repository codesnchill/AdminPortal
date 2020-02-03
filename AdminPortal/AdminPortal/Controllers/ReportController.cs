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

namespace AdminPortal.Controllers
{

    public class ReportController : Controller
    {
        IEnumerable<Milestone> report = null;
        IEnumerable<AuditReport> auditreport = null;
        public PaginatedList<Milestone> reportList { get; private set; }
        public PaginatedList<AuditReport> auditreportList { get; private set; }
        public IActionResult GenerateReport()
        {
            return View();
        }

        public IActionResult AuditReport()
        {

            return new ViewAsPdf();
        }

        [HttpGet]
        public IActionResult AuditReport([FromQuery] AuditReport employeeQuery)
        {
            //var type = employeeQuery.ReportType;
            //var start = employeeQuery.StartDate;
            //var end = employeeQuery.EndDate;
            var type = "audit";
            var start = "2020-01-01";
            var end = "2020-01-31";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("report?ReportType=" + type + "&StartDate=" + start + "&EndDate=" + end);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<AuditReport>>();
                    readTask.Wait();

                    auditreport = readTask.Result;
                    auditreportList = PaginatedList<AuditReport>.Create(auditreport, 1, 4, 5);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    auditreport = Enumerable.Empty<AuditReport>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(auditreportList);

            }
        }


        [HttpGet]
        public IActionResult MilestoneReport([FromQuery] Report employeeQuery)
        {
            //var type = employeeQuery.ReportType;
            //var start = employeeQuery.StartDate;
            //var end = employeeQuery.EndDate;
            var type = "milestone";
            var start = "2020-01-01";
            var end = "2020-01-31";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("report?ReportType="+type+"&StartDate="+start+"&EndDate="+end);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Milestone>>();
                    readTask.Wait();

                    report = readTask.Result;
                    reportList = PaginatedList<Milestone>.Create(report, 1, 4, 5);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    report = Enumerable.Empty<Milestone>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(reportList);

            }
        }


}
}