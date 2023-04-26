using MultiplicationTable.Models.ResultModel;

namespace MultiplicationTable.Models.UserModel;

internal class User
{
    public long ChatId { get; set; }
    public ENextStep NextStep { get; set; }
    public bool IsStart { get; set; }
    public bool UserStatusStartTest { get; set; }
    public int CurrentQuestionIndex { get; set; }
    public int CurrentMultiplicatioinIndex { get; set; }
    public int CorrectAnswerCount { get; set; }
    public int MessageId { get; set; }  
    public List<Result> UserResuts { get; set; }

    public User()
    {
        UserResuts = new List<Result>();
    }
}
