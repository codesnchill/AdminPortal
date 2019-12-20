using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    public class UserController : Controller
    {
        public IActionResult ManageUser()
        {
            return View();
        }
    }
}