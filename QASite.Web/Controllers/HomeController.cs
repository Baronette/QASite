using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QASite.Data;
using QASite.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QASite.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _connection;
        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
           _connection = _configuration.GetConnectionString("constr");
    }
        

        public IActionResult Index()
        {
            var repo = new QARepository(_connection);

            HomePageViewModel vm = new()
            {
                Questions = repo.GetQuestions()
            };
            return View(vm);
        }

    }
}
