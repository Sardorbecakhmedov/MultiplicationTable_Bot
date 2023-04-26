namespace MultiplicationTable.Services.QuestionsServices;

partial class QustionServices
{
    private static ListQuestion questions = new ListQuestion();

    public static List<Question> GetQuestionsList(int index)
    {
        var list = new List<List<Question>>()
        {
            questions.ListQuestions1(),
            questions.ListQuestions2(),
            questions.ListQuestions3(),
            questions.ListQuestions4(),
            questions.ListQuestions5(),
            questions.ListQuestions6(),
            questions.ListQuestions7(),
            questions.ListQuestions8(),
            questions.ListQuestions9(),
            questions.ListQuestions10(),
            questions.ListQuestions11(),
            questions.ListQuestions12(),
        };

        if (index >= 0 && index < list.Count )
        {
            return list[index];
        }

        return null!;
    }


}
