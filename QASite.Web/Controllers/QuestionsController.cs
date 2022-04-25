using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QASite.Data;
using QASite.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QASite.Web.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _connection;
        public QuestionsController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _connection = _configuration.GetConnectionString("constr");
        }

        public IActionResult ViewQuestion(int id)
        {
            var questionRepo = new QARepository(_connection);
            var question = questionRepo.GetById(id);
            if (question == null)
            {
                return Redirect("/");
            }
            var vm = new ViewQuestionViewModel
            {
                Question = question
            };
            if (User.Identity.IsAuthenticated)
            {
                var acctRepo = new AccountRepository(_connection);
                vm.CurrentUser = acctRepo.GetUser(User.Identity.Name);
                vm.CurrentUser.Likes = acctRepo.GetUserLikes(vm.CurrentUser.Id);
            }
            return View(vm);
        }
        public IActionResult NewQuestion()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddQuestion(Question question, List<string> tags)
        {
            var acctRepo = new AccountRepository(_connection);
            var user = acctRepo.GetUser(User.Identity.Name);
            question.UserId = user.Id;
            var questionRepo = new QARepository(_connection);
            questionRepo.AddQuestion(question, tags);
            return Redirect($"/questions/viewquestion?id={question.Id}");
        }
        public IActionResult GetLikes(int id)
        {
            var questionRepo = new QARepository(_connection);
            return Json(new { Likes = questionRepo.GetLikes(id) });
        }
        [Authorize]
        [HttpPost]
        public void LikeQuestion(int questionId)
        {
            var questionRepo = new QARepository(_connection);
            var acctRepo = new AccountRepository(_connection);
            var userId = acctRepo.GetUser(User.Identity.Name).Id;
            questionRepo.AddLike(questionId, userId);

        }
        [Authorize]
        [HttpPost]
        public IActionResult AddAnswer(Answer answer)
        {
            var questionRepo = new QARepository(_connection);
            var acctRepo = new AccountRepository(_connection);
            answer.UserId = acctRepo.GetUser(User.Identity.Name).Id;
            answer.DateSubmitted = DateTime.Now;
            questionRepo.AddAnswer(answer);
            return Redirect($"/questions/viewquestion?id={answer.QuestionId}");

        }

    }
}
