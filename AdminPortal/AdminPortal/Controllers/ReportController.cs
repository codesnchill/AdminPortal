﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            //HtmlToPdfConverter converter = new HtmlToPdfConverter();

            //WebKitConverterSettings settings = new WebKitConverterSettings();
            //settings.WebKitPath = ("C:/AdminPortal/AdminPortal/AdminPortal/QtBinariesWindows/");
            //converter.ConverterSettings = settings;

            //PdfDocument document = converter.Convert("https://localhost:44332/Point/ManagePoints");

            //MemoryStream ms = new MemoryStream();
            //document.Save(ms);
            //document.Close(true);
            //ms.Position = 0;

            //FileStreamResult fsr = new FileStreamResult(ms, "application/pdf");
            //fsr.FileDownloadName = "invoice.pdf";

            //return fsr;
            return View();
        }
        public IActionResult MilestoneReport()
        {
            //HtmlToPdfConverter converter = new HtmlToPdfConverter();

            //WebKitConverterSettings settings = new WebKitConverterSettings();
            //settings.WebKitPath = ("C:/AdminPortal/AdminPortal/AdminPortal/QtBinariesWindows/");
            //converter.ConverterSettings = settings;

            //PdfDocument document = converter.Convert("https://localhost:44332/Point/ManagePoints");

            //MemoryStream ms = new MemoryStream();
            //document.Save(ms);
            //document.Close(true);
            //ms.Position = 0;

            //FileStreamResult fsr = new FileStreamResult(ms, "application/pdf");
            //fsr.FileDownloadName = "invoice.pdf";

            //return fsr;
            return View();
        }
    }
}