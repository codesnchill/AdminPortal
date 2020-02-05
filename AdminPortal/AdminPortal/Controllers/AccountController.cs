using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net;

namespace AdminPortal.Controllers
{
    public class AccountController : Controller
    {
        const string tokenSession = "tokenSessionObject";
        const string employeeSession = "employeeDetailsObject";

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult setTokenSession([FromBody]Token TokenObj)
        {
            HttpContext.Session.SetComplexData(tokenSession, TokenObj);
            return Ok("Session set");
        }

        [HttpPost]
        public IActionResult setEmployeeSession([FromBody]EmployeeSession EmployeeObj)
        {
            HttpContext.Session.SetComplexData(employeeSession, EmployeeObj);
            return Ok("Session set");
        }

        [HttpGet]
        public IActionResult getTokenSession()
        {
            string stringifiedTokenObj = JsonConvert.SerializeObject(HttpContext.Session.GetString(tokenSession));
            return Ok(stringifiedTokenObj);
        }

        [HttpGet]
        public IActionResult getEmployeeSession()
        {
            string stringifiedTokenObj = HttpContext.Session.GetString(employeeSession);

            return Ok(stringifiedTokenObj);
        }
    }
}