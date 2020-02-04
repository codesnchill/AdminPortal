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
        const string QuestionListSessionName = "_CompanyQuizQuestionList";
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

        IEnumerable<Question> questions2 = null;
        const string QuestionListSessionName2 = "_GuessTheEmployeeQuestionList";


        [HttpGet]
        public JsonResult LoadCompanyQuiz()
        {
            List<Question> questionList = new List<Question>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("games/2?deleted=false");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Question>>();
                    readTask.Wait();

                    questions2 = readTask.Result;
                    questionList = questions2.ToList();

                    // store employee in session
                    HttpContext.Session.SetComplexData(QuestionListSessionName2, questionList);
                    //HttpContext.Session["employeeList"] = employeeList;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    questions2 = Enumerable.Empty<Question>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            //make sure it returns the whole list retrieve from database (not the paginated list)
            return new JsonResult(questionList);
        }



        IEnumerable<Question> questions1 = null;
        const string EmployeeQuizSessionList = "_GuessTheEmployeeQuestionList";

        [HttpGet]
        public JsonResult LoadGuessTheEmployeeQuiz()
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

                    questions1 = readTask.Result;
                    questionList = questions1.ToList();

                    // store employee in session
                    HttpContext.Session.SetComplexData(EmployeeQuizSessionList, questionList);
                    //HttpContext.Session["employeeList"] = employeeList;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    questions1 = Enumerable.Empty<Question>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            //make sure it returns the whole list retrieve from database (not the paginated list)
            return new JsonResult(questionList);
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


          IEnumerable<Question> deletedQuestions1 = null;
        const string DeletedCompanyQuizListSessionName = "_DeletedGameQuestionList";

        [HttpGet]
        public JsonResult LoadDeletedCompanyQuiz()
        {
            List<Question> questionList = new List<Question>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44300/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("games/2?deleted=true");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Question>>();
                    readTask.Wait();

                    deletedQuestions1 = readTask.Result;
                    questionList = deletedQuestions1.ToList();

                    // store employee in session
                    HttpContext.Session.SetComplexData(DeletedCompanyQuizListSessionName, questionList);
                    //HttpContext.Session["employeeList"] = employeeList;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    deletedQuestions1 = Enumerable.Empty<Question>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            //make sure it returns the whole list retrieve from database (not the paginated list)
            return new JsonResult(questionList);
        }

        IEnumerable<Question> deletedQuestions2 = null;
        const string DeletedEmployeeListSessionName = "_GuessTheEmployeeQuestionList";


        [HttpGet]
        public JsonResult LoadDeletedGuessTheEmployeeQuiz()
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

                    deletedQuestions2 = readTask.Result;
                    questionList = deletedQuestions2.ToList();

                    // store employee in session
                    HttpContext.Session.SetComplexData(DeletedEmployeeListSessionName, questionList);
                    //HttpContext.Session["employeeList"] = employeeList;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    deletedQuestions2 = Enumerable.Empty<Question>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            //make sure it returns the whole list retrieve from database (not the paginated list)
            return new JsonResult(questionList);
        }
    }
}