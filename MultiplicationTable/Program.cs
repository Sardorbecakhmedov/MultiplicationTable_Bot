using MultiplicationTable.Models.UserModel;
using MultiplicationTable.Services.QuestionsServices;
using MultiplicationTable.Services.ResultsServices;
using MultiplicationTable.Services.UsersServices;
using MultiplicationTable.Telegram;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


long MY_CHAT_ID = 2142940558;
bool sendMeToMessage = false;

var userServices = new UserServices();
var resultServices = new SaveResults();

#region [ Telegram codlari ]
const string TOKEN = "Token";

var bot = new TelegramBotClient(TOKEN);

using CancellationTokenSource cancellationTokenSource = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>(), // receive all update types
    ThrowPendingUpdates = true
};

bot.StartReceiving(
    updateHandler: UpdateHandlerAsync,
    pollingErrorHandler: Telegrambot.PollingArrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cancellationTokenSource.Token
    );

var me = await bot.GetMeAsync();

Console.WriteLine($" @{me.Username}:   bot ishladi");
Console.ReadLine();

cancellationTokenSource.Cancel();
#endregion


async Task UpdateHandlerAsync( ITelegramBotClient bot, Update update, CancellationToken cts)
{
    var listCommand = new List<string>
    {
        "/start",
        "mengaxabar...!",
        "/testniboshla",
        "karra...",
        "boshladik...!",
        "/my_results",
        "index❌...!",
        "/info",
        "haroziman",
        "yo'qnoroziman",


        "karra...0",
        "karra...1",
        "karra...2",
        "karra...3",
        "karra...4",
        "karra...5",
        "karra...6",
        "karra...7",
        "karra...8",
        "karra...9",
        "karra...10",
        "karra...11",
    };

    var (messageText, firstName, messageId, chatId, isSuccess) = Telegrambot.GetMessage(update);

    if (!isSuccess)
        return;

    var user = await userServices.GetUser(chatId);

    Console.WriteLine($"User:  {firstName},  messageText:  {messageText}");

    if (user.UserStatusStartTest || sendMeToMessage || messageText != null && messageText.StartsWith("index❌...!") || 
        messageText != null && listCommand.Contains(messageText) )
    {
        if (messageText == "/start")
        {
            if (!user.IsStart)
            {
                user.IsStart = true;
                user.MessageId = messageId;
                await userServices.UpdateUserDataAsync(user);

                await Telegrambot.SendStartMessageAsync(bot, firstName ?? "No name", chatId, cts);
            }
            else if (user.IsStart)
            {
                string path = @"multiplication\menumain.png";

                using (var stream = System.IO.File.OpenRead(path))
                {
                    await bot.SendPhotoAsync(
                       chatId: chatId,
                       photo: stream!,
                       caption: $"⬇ Quyidagi tugmalardan birini tanlang",
                       replyMarkup: Telegrambot.GetButtonMainMenu(),
                       cancellationToken: cts);
                }
            }
        }      
        else if (messageText == "mengaxabar...!")
        {
            sendMeToMessage = true;

            await bot.SendTextMessageAsync(
               chatId: chatId,
               text: "✍  Talab takliflaringizni yozib yuboring: ",
               cancellationToken: cts);
        }
        else if (sendMeToMessage)
        {
            sendMeToMessage = false;

            await bot.SendTextMessageAsync(
              chatId: chatId,
              text: "🙌 Men sizning xabaringizni oldim,  \netibor uchun raxmat!👍",
              replyMarkup: Telegrambot.GetInlineButtonForMainMenu(),
              cancellationToken: cts);

            await bot.SendTextMessageAsync(
               chatId: MY_CHAT_ID,
               text: $"\n💥 Janob sizga @KarraJadval_Bot dan xabar keldi!  \nXabar jo'natuvchi:  {firstName}  \nXabar matni: {messageText}",
               cancellationToken: cts);
        }
        else if (messageText == "/testniboshla")
        {
            if (user.UserStatusStartTest)
            {
                var button = new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData("Ha  ✅", "haroziman"),
                    InlineKeyboardButton.WithCallbackData("Yo'q ❌", "!_yo'qnoroziman")
                };

                await bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: "⚠  Agarda testdan chiqsangiz shu joygacha bajargan test natijalaringiz saqlanmaydi! shunga rozimisiz ? ",
                    replyMarkup: new InlineKeyboardMarkup(button),
                    cancellationToken: cts);
                // Shu yerda testni to'xtashiga ruxsat so'rashi kerak
                return;
            }
           
            user.MessageId = messageId;
            await userServices.UpdateUserDataAsync(user, ENextStep.StartTest);
            await Telegrambot.ChooseMultiplicationTableAsync(bot, chatId, cts);
            
        }
        else if (messageText == "haroziman")  // testni yarmida tugatishga ruxsat
        {
            user.UserStatusStartTest = false;
            user.MessageId = messageId;
            await userServices.UpdateUserDataAsync(user, ENextStep.StartTest);
            await Telegrambot.ChooseMultiplicationTableAsync(bot, chatId, cts);
        }
        else if (messageText!.StartsWith("karra..."))     // Qaysi jadvalni tanlaganini yuborish va saqlash
        {
            await Telegrambot.SendSaveSelectEnterStartAsync(user, messageText, bot, chatId, cts);
            await bot.DeleteMessageAsync(chatId, messageId, cts);
        }
        else if (messageText == "boshladik...!")
        {
            user.UserStatusStartTest = true;
            user.MessageId = messageId;
            await userServices.UpdateUserDataAsync(user);
        }
        else if (messageText == "/my_results") // User test natijalarini ko'rastish
        {
            user.MessageId = messageId;
            await userServices.UpdateUserDataAsync(user);
            await resultServices.ShowUserResultsAsyn(user, chatId, bot, cts);
        }
        else if (messageText.StartsWith("index❌...!"))  // User natijasini index bo'yicha o'chirish uchun
        {
            try
            {
                int index = int.Parse(messageText.Replace("index❌...!", ""));

                user.UserResuts.RemoveAt(index);

                await userServices.UpdateUserDataAsync(user);

                await bot.EditMessageReplyMarkupAsync(
                    chatId: chatId,
                    messageId: messageId,
                    replyMarkup: Telegrambot.GetInlineButtonForResults(user.UserResuts),
                    cancellationToken: cts);
            }
            catch
            {
                await bot.SendTextMessageAsync(
                      chatId: chatId,
                      text: "⚠ Kutilmagan xatolik yuz berdi! \n🔄 Iltimos so'rovni qaytadan yuboring: ",
                      replyMarkup: Telegrambot.GetButtonMainMenu(),
                      cancellationToken: cts);
            }
        }
        else if (messageText == "/info")
        {
            await bot.SendTextMessageAsync(
                chatId: chatId,
                text: await Telegrambot.GetTextInfoBot(),
                replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("↩  Asosiy menuga qaytish", "/start")),
                cancellationToken: cts);
        }
    }
    else
    {
        await bot.SendTextMessageAsync(
              chatId: chatId, 
              text: "⚠ Kutilmagan xatolik yuz berdi! \n🔄 Iltimos so'rovni qaytadan yuboring: ",
              replyMarkup: Telegrambot.GetButtonMainMenu(),
              cancellationToken: cts);
    }

    if (user.UserStatusStartTest)
    {
        var question = QustionServices.GetQuestionsList(user.CurrentMultiplicatioinIndex);
        switch (user.NextStep)
        {
            case ENextStep.StartTest:
                {
                    await bot.DeleteMessageAsync(chatId, user.MessageId, cts);
                    await Telegrambot.SendShowQuestionAsync(user, bot, chatId, cts);
                    await userServices.UpdateUserDataAsync(user, ENextStep.CheckAnswer);
                }
                break;
            case ENextStep.CheckAnswer:
                {
                    if (update.Type == UpdateType.CallbackQuery && question[user.CurrentQuestionIndex].Options!.Any(u => messageText!.Contains(u)))
                    {
                        await bot.EditMessageReplyMarkupAsync(
                            chatId: chatId,
                            messageId: messageId,
                            replyMarkup: Telegrambot.EditInlineButtonForOptions(user, question[user.CurrentQuestionIndex].Options!, messageText!),
                            cancellationToken: cts);

                        await Telegrambot.SendCheckAnswerAsync(user, bot, chatId, cts, messageText!);
                        user.CurrentQuestionIndex++;

                        if (user.CurrentQuestionIndex < question.Count)
                            await Telegrambot.SendShowQuestionAsync(user, bot, chatId, cts);
                        else
                        {
                            await resultServices.SaveUserResultAsync(chatId, user);

                            await Telegrambot.FinishQuestionAsync(user, bot, chatId, cts);

                            user.UserStatusStartTest = false;
                            user.CurrentQuestionIndex = 0;
                            user.CurrentMultiplicatioinIndex = 0;
                            user.CorrectAnswerCount = 0;

                            await resultServices.SaveResultsDataAsync();
                            await userServices.UpdateUserDataAsync(user);
                        }
                    }
                    else
                    {
                        if (messageText == "!_yo'qnoroziman")
                        {
                            await Telegrambot.SendShowQuestionAsync(user, bot, chatId, cts);
                        }
                        else
                        {
                            await Telegrambot.CheckAnswerHasOptionAsync(user, bot, chatId, cts);
                            await Telegrambot.SendShowQuestionAsync(user, bot, chatId, cts);
                        }
                    }
                }
                break;
        }
    }

}




