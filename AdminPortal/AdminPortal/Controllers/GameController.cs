using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models;

namespace AdminPortal.Controllers
{
    public class GameController : Controller
    {
        IEnumerable<Question> questions = null;
        public IActionResult ManageGames()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("games/1");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Question>>();
                    readTask.Wait();

                    questions = readTask.Result;
                    
                }
                else //web api sent error response 
                {
                    //log response status here..

                    questions = Enumerable.Empty<Question>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            return View(questions);
        }

        public IActionResult EditQuestion()
        {
            return View();
        }

        public IActionResult AddQuestion()
        {
            return View();
        }
    }
}