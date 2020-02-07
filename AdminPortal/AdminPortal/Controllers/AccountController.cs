using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System;
using AdminPortal.Utils;

namespace AdminPortal.Controllers
{
    public class AccountController : Controller
    {
        const string tokenSession = "tokenSessionObject";
        const string employeeSession = "employeeDetailsObject";
        string baseUrl = BaseURLHelper.GetBaseURL();

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

        public async Task<bool> tokenIsValid(Token token, HttpContext httpContext)
        {
            DateTime now = DateTime.Now;
            //get expiration
            DateTime Token_expiration = DateTime.Parse(token.Token_expiration);
            Token newTokenObj = new Token();
            //compare and refresh if expired
            if (Token_expiration.ToUniversalTime() < now.ToUniversalTime())
            {
                //refresh
                string content = await refreshToken(token.Refresh_token);

                if (String.IsNullOrWhiteSpace(content))
                {
                    return false;
                }

                var contentObj = JsonConvert.DeserializeObject<dynamic>(content);

                //set in session
                newTokenObj.Token1 = contentObj.token;
                newTokenObj.Token_expiration = contentObj.token_expiration;
                newTokenObj.Refresh_token = token.Refresh_token;

                httpContext.Session.SetComplexData(tokenSession, newTokenObj);

                return true;
            }
            return true;
        }

        [HttpGet]
        public async Task<bool> tokenIsValid()
        {
            Token token = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString(tokenSession));
            DateTime now = DateTime.Now;
            //get expiration
            DateTime Token_expiration = DateTime.Parse(token.Token_expiration);
            Token newTokenObj = new Token();
            //compare and refresh if expired
            if (Token_expiration.ToUniversalTime() < now.ToUniversalTime())
            {
                //refresh
                string content = await refreshToken(token.Refresh_token);

                if (String.IsNullOrWhiteSpace(content))
                {
                    return false;
                }

                var contentObj = JsonConvert.DeserializeObject<dynamic>(content);

                //set in session
                newTokenObj.Token1 = contentObj.token;
                newTokenObj.Token_expiration = contentObj.token_expiration;
                newTokenObj.Refresh_token = token.Refresh_token;

                HttpContext.Session.SetComplexData(tokenSession, newTokenObj);

                return true;
            }
            return true;
        }

        public async Task<dynamic> refreshToken(string refreshToken)
        {
            HttpClient client = new HttpClient();
            string url = baseUrl + "/api/v1/refreshtoken";

            var myObject = new
            {
                refresh_token = refreshToken
            };

            string json_bodyObj = "'" + JsonConvert.SerializeObject(myObject) + "'";

            var content = new StringContent(json_bodyObj, Encoding.UTF8, "application/json");


            var response = await client.PostAsync(url, content);

            var contents = await response.Content.ReadAsStringAsync();

            return contents;
        }
    }
}