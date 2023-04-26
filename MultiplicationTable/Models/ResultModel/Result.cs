
namespace MultiplicationTable.Models.ResultModel;

internal class Result
{
    public long ChatId { get; set; }
    public int MultiplicationName { get; set; }
    public int CorrectAnswerCount { get; set; }
    public int QuestionCount { get; set; }
    public DateTime _DateTime { get; set; }
}
