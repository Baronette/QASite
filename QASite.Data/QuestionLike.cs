namespace QASite.Data
{
    public class QuestionLike
    {
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public Question Question { get; set; }
        public User User { get; set; }
    }
}


