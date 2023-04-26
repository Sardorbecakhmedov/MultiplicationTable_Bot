using MultiplicationTable.Models.UserModel;
using MultiplicationTable.Services.QuestionsServices;
using Telegram.Bot;

namespace MultiplicationTable.Telegram;

partial class Telegrambot
{
    // Savollarni botga jo'natish
    public static async Task SendShowQuestionAsync(User user, ITelegramBotClient bot, long chatId, CancellationToken cts)
    {
        var question = QustionServices.GetQuestionsList(user.CurrentMultiplicatioinIndex);

        await bot.SendTextMessageAsync(
        chatId: chatId,
            text: $"⚠  Diqqat savol ❓  \n\n{question[user.CurrentQuestionIndex].QestionText} \n\n⬇  Quyidagi berilgan variyantlardan birini tanlang ",
            replyMarkup: GetInlineButtonForOptions(question[user.CurrentQuestionIndex].Options!),
            cancellationToken: cts);
    }


    // Javobni to'g'ri yoki no to'g'riligini tekshirish
    public static async Task SendCheckAnswerAsync(User user, ITelegramBotClient bot, long chatId, CancellationToken cts, string messageText)
    {
        var question = QustionServices.GetQuestionsList(user.CurrentMultiplicatioinIndex);

        if (question[user.CurrentQuestionIndex].CorrectAnswer == messageText)
        {
            await bot.SendTextMessageAsync(chatId, "Qoyil 👍  javobingiz to'g'ri  ✅", cancellationToken: cts);
            user.CorrectAnswerCount++;
        }
        else
            await bot.SendTextMessageAsync(chatId, "Afsus ☹  javob no to'g'ri  ❌", cancellationToken: cts);
    }


    // Agar karra jadval oxiriga yetsa
    public static async Task FinishQuestionAsync(User user, ITelegramBotClient bot, long chatId, CancellationToken cts)
    {
        string path = @"multiplication\finish.png";

        using (var stream = File.OpenRead(path))
        {
            await bot.SendPhotoAsync(
               chatId: chatId,
               photo: stream!,
               caption: $"💥  Tabriklaymiz siz [ {user.CurrentMultiplicatioinIndex + 1} ] karra jadvali testini yakunladingiz! ",
               replyMarkup: GetInlineButtonForMainMenu(),
               cancellationToken: cts);
        }
    }


    //   Agar kiritilgan javob variantlar orasidan bo'lmasa
    public static async Task CheckAnswerHasOptionAsync(User user, ITelegramBotClient bot, long chatId, CancellationToken cts)   
    {
        string path = @"multiplication\opps.jpg";

        using (var stream = File.OpenRead(path))
        {
            await bot.SendPhotoAsync(
               chatId: chatId,
               photo: stream!,
               caption: $"☢  No to'g'ri format kiritildi " +
                        $"\nIltimos javobni qayta yuboring " +
                        $"\nJavobni pastdagi ⬇ tugmalardan foydalanib yuboring! ",
               cancellationToken: cts);
        }
    }

}
