using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QASite.Data
{
    public partial class QARepository
    {
        private readonly string _connection;

        public QARepository(string connection)
        {
            _connection = connection;
        }
        public List<Question> GetQuestions()
        {
            using var context = new QASiteContext(_connection);
            return context.Questions.Include(q => q.Answers).Include(q => q.Answers)
                .Include(q => q.QuestionsTags).ThenInclude(q => q.Tag)
                .Include(q => q.Likes)
                .OrderByDescending(q => q.Datesubmitted).ToList();
        }
        public Question GetById(int id)
        {
            using var context = new QASiteContext(_connection);
            return context.Questions
                .Include(q => q.Answers).ThenInclude(a => a.User)
                .Include(q => q.User).ThenInclude(u => u.Likes)
                .Include(q => q.QuestionsTags).ThenInclude(qt => qt.Tag)
                .Include(q => q.Likes).FirstOrDefault(q => q.Id == id);
        }
        public void AddQuestion(Question question, List<string> tags)
        {
            using var context = new QASiteContext(_connection);
            context.Questions.Add(question);
            context.SaveChanges();
            foreach (var name in tags)
            {
                Tag tag = GetTag(name);
                int tagId;
                if (tag == null)
                {
                    tagId = AddTag(name);
                }
                else
                {
                    tagId = tag.Id;
                }
                context.QuestionsTags.Add(new QuestionsTags
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });
                context.SaveChanges();

            }
        }
        public int AddTag(string name)
        {
            using var context = new QASiteContext(_connection);
            var tag = new Tag { Name = name };
            context.Tags.Add(tag);
            context.SaveChanges();
            return tag.Id;
        }

        public Tag GetTag(string name)
        {
            using var context = new QASiteContext(_connection);
            return context.Tags.FirstOrDefault(t => t.Name == name);
        }
        public int GetLikes(int id)
        {
            using var context = new QASiteContext(_connection);
            return context.Likes.Where(l => l.QuestionId == id).Count();
        }
        public void AddLike(int questionId, int userId)
        {
            using var context = new QASiteContext(_connection);
            context.Likes.Add(new QuestionLike
            {
                QuestionId = questionId,
                UserId = userId
            });
            context.SaveChanges();
        }
        public void AddAnswer(Answer answer)
        {
            using var context = new QASiteContext(_connection);
            context.Answers.Add(answer);
            context.SaveChanges();
        }
    }
}