using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace AdminPortal.Controllers
{
    public class AccountController : Controller
    {
        const string tokenSession = "tokenSessionObject";
        const string employeeSession = "employeeDetailsObject";
        const string baseUrl = "https://localhost:44300";

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
            string stringifiedTokenObj = JsonConvert.SerializeObject(HttpContext.Session.GetString(employeeSession));

            return Ok(stringifiedTokenObj);
        }

        [HttpPost]
        public async Task<IActionResult> refreshToken()
        {
            // get refresh token
            var input = JsonConvert.DeserializeObject<dynamic>(HttpContext.Session.GetString(tokenSession));
            string refreshToken = input.Refresh_token;
            HttpClient client = new HttpClient();
            string url = baseUrl + "/api/v1/refreshtoken";
            Token token = new Token();


            var myObject = new
            {
                refresh_token = refreshToken
            };

            string json_bodyObj = "'" + JsonConvert.SerializeObject(myObject) + "'";

            var content = new StringContent(json_bodyObj, Encoding.UTF8, "application/json");


            var response = await client.PostAsync(url, content);

            var contents = await response.Content.ReadAsStringAsync();

            return Ok(contents);
        }
        [HttpGet]
        public IActionResult clearSession()
        {
            HttpContext.Session.Clear();

            return Ok();
        }
    }
}