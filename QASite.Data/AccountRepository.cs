using System.Collections.Generic;
using System.Linq;

namespace QASite.Data
{
    public class AccountRepository
    {
        private readonly string _connection;

        public AccountRepository(string connection)
        {
            _connection = connection;
        }
        public User GetUser(string email)
        {
            var context = new QASiteContext(_connection);
            return context.Users.Where(u => u.Email == email).FirstOrDefault();
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return isValid ? user : null;
        }

        public void AddUser(User user, string password)
        {
            using var context = new QASiteContext(_connection);
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            context.Users.Add(user);
            context.SaveChanges();
        }

        public User GetByEmail(string email)
        {
            using var context = new QASiteContext(_connection);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
        public List<QuestionLike> GetUserLikes(int id)
        {
            using var context = new QASiteContext(_connection);
            return context.Likes.Where(l => l.UserId == id).ToList();     }
    }
}
