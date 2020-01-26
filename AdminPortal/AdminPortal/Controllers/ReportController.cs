using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult GenerateReport()
        {
            return View();
        }
    }
}