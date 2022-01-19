using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DockerWeb.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace DockerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private string APIURL;//= "http://{dockerapicontainer}/api/weatherforecast";

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index(string servicename)
        {
            //try
            //{
            //    URL = _configuration["AppSettings:APIURL"].ToString();
            //    Response.WriteAsync(URL);
            //}
            //catch (Exception ex)
            //{
            //    Response.WriteAsync(ex.StackTrace).RunSynchronously();
            //}
            try
            {
                //if (!string.IsNullOrWhiteSpace(servicename))
                //{
                //    APIURL = $"http://{servicename}/api/weatherforecast";
                //}
                //else
                //{
                    APIURL = _configuration["AppSettings:APIURL"];
                //}
                ViewData["APIURL"] = APIURL;
                var model = MakeAPICall<WeatherForecast[]>(APIURL, false);
                ViewData["Summary"] = model[0].Summary;
            }
            catch (Exception ex)
            {
                ViewData["Summary"] = ex.StackTrace;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private TResult MakeAPICall<TResult>(string apiUrl, bool post, params KeyValuePair<string, string>[] parameters)
        {

            //Encoding encoding = Encoding.GetEncoding("iso-8859-1");

            var httpClientHandler = new HttpClientHandler();

            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                //httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("{0}{1}", "Basic ", authorizationKey));
                //if (appConfig.UseHttpsAPI())
                //    ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                HttpResponseMessage result;
                if (post)
                {
                    using (HttpContent content = new FormUrlEncodedContent(parameters))
                    {
                        result = httpClient.PostAsync(apiUrl, content).Result;
                    }
                }
                else
                {
                    result = httpClient.GetAsync(apiUrl).Result;
                }

                var resultContent = result.Content.ReadAsStringAsync().Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<TResult>(resultContent);
                }

            }

            return default(TResult);
        }
    }
}
