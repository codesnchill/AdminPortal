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
        const string QuestionListSessionName = "_GameQuestionList";
        public IActionResult ManageGames()
        {
            List<Question> questionList = new List<Question>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("games/1?deleted=false");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Question>>();
                    readTask.Wait();

                    questions = readTask.Result;
                    questionList = questions.ToList();

                    // store employee in session
                    HttpContext.Session.SetComplexData(QuestionListSessionName, questionList);
                    //HttpContext.Session["employeeList"] = employeeList;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    questions = Enumerable.Empty<Question>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(questionList);
        }

        public IActionResult EditQuestion()
        {
            return View();
        }

        public IActionResult AddQuestion()
        {
            return View();
        }

        public IActionResult EditDeletedQuestion()
        {
            return View();
        }

        IEnumerable<Question> deletedQuestions = null;
        const string DeletedQuestionListSessionName = "_DeletedGameQuestionList";
        public IActionResult ManageDeletedGames()
        {
            List<Question> questionList = new List<Question>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("games/1?deleted=true");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Question>>();
                    readTask.Wait();

                    deletedQuestions = readTask.Result;
                    questionList = deletedQuestions.ToList();

                    // store employee in session
                    HttpContext.Session.SetComplexData(DeletedQuestionListSessionName, questionList);
                    //HttpContext.Session["employeeList"] = employeeList;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    deletedQuestions = Enumerable.Empty<Question>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            //make sure it returns the whole list retrieve from database (not the paginated list)
            return View(questionList);
        }
    }
}