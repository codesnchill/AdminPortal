using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace AdminPortal.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult GenerateReport()
        {
            return View();
        }

        public IActionResult AuditReport()
        {

            return new ViewAsPdf();
        }

        [HttpPost]
        public IActionResult AuditReport(string submit)
        {
            HtmlToPdfConverter converter = new HtmlToPdfConverter();

            WebKitConverterSettings settings = new WebKitConverterSettings();
            settings.WebKitPath = ("C:/AdminPortal/AdminPortal/AdminPortal/QtBinariesWindows/");
            //settings.WindowStatus = "completed";
            converter.ConverterSettings = settings;
           

            PdfDocument document = converter.Convert("https://localhost:44332/Report/AuditReport");

            MemoryStream ms = new MemoryStream();
            document.Save(ms);
            document.Close(true);
            ms.Position = 0;

            FileStreamResult fsr = new FileStreamResult(ms, "application/pdf");
            fsr.FileDownloadName = "invoice.pdf";

            return fsr;
            //return View();
        }
        public IActionResult MilestoneReport()
        {
            return View();
        }



}
}