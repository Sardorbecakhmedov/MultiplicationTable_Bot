using MultiplicationTable.Models.ResultModel;
using MultiplicationTable.Services.QuestionsServices;
using MultiplicationTable.Telegram;
using Telegram.Bot;
using User = MultiplicationTable.Models.UserModel.User;

namespace MultiplicationTable.Services.ResultsServices;

partial class SaveResults
{
    public async Task SaveUserResultAsync(long chatId, User user)
    {
        var question = QustionServices.GetQuestionsList(user.CurrentMultiplicatioinIndex);

        var result = new Result()
        {
            ChatId = chatId,
            MultiplicationName = user.CurrentMultiplicatioinIndex + 1,
            CorrectAnswerCount = user.CorrectAnswerCount,
            QuestionCount = question.Count,
            _DateTime = DateTime.Now,
        };

        await Task.Run(() => { user.UserResuts.Add(result); });
        await Task.Run(() => { Results!.Add(result); });
    }

    public async Task ShowUserResultsAsyn(User user, long chatId, ITelegramBotClient bot, CancellationToken cts)
    {
        string path = @"C:\Users\sardo\OneDrive\Pictures\results.jpg";

        using (var stream = System.IO.File.OpenRead(path))
        {
            await bot.SendPhotoAsync(
                chatId: chatId,
                photo: stream!,
                caption: "📒 Sizning test natijalaringiz \n\n 1️⃣ - ustunda, qaysi ko'paytiruv jadval ekanligi \n 2️⃣ - ustunda, to'g'ri javoblar va savollar soni " +
                "\n 3️⃣ - ustunda, test qaysi sanada ishlangani " +
                "\n 4️⃣ - ustunda, tanlangan qatordagi natijani o'chirish \n\n Natijani o'chirish uchun ❌ belgili tugmani bosing",
                replyMarkup: Telegrambot.GetInlineButtonForResults(user.UserResuts),
                cancellationToken: cts);
        }
    }

}
