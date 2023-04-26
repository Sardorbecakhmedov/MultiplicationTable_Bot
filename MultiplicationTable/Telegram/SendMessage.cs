using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using User = MultiplicationTable.Models.UserModel.User;


namespace MultiplicationTable.Telegram;

partial class Telegrambot
{
    // User birinchi marta botga kirganda jo'natiladi
    public static async Task SendStartMessageAsync(ITelegramBotClient bot, string firstName, long chatId, CancellationToken cts)
    {
        await bot.SendTextMessageAsync(
           chatId: chatId,
           text: $"🖐  Assalomu alaykum   [ {firstName} ]  ko'paytiruv jadval botimizga xush kelibsiz," +
           "\n\nUshbu bot sizga karra jadvalini yod olishinga yordam beradi degan umiddamiz inshaolloh. " ,    
           replyMarkup: GetButtonMainMenu(),
           cancellationToken: cts);
    }


    // Karra jadvalni tanlang deb yuborish uchun
    public static async Task ChooseMultiplicationTableAsync(ITelegramBotClient bot, long chatId, CancellationToken cts)
    {
        string path = @"multiplication\multiplicationtable2.jpg";

        using (var stream = File.OpenRead(path))
        {
            var startmsg = await bot.SendPhotoAsync(
               chatId: chatId,
               photo: stream!,
               caption: @"⬇  Ko'paytiruv jadvalini tanlang!",
               replyMarkup: GetInlineButtonChooseTableMenu(),
               cancellationToken: cts);
        }
    }


    // Qaysi karra jadvalni tanlaganini yuborish uchun
    public static async Task SendSaveSelectEnterStartAsync( User user, string messageText, ITelegramBotClient bot, long chatId, CancellationToken cts)
    {
        var temp = messageText.Replace("karra...", "");
    
        try
        {
            int index = int.Parse(temp);
            user.CurrentMultiplicatioinIndex = index;

            var list = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("🚀  Boshladik", "boshladik...!") },
                new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("↩  Asosiy menuga qaytish", "/start") },
            };
            
            string path = $@"multiplication\{index}.jpeg";

            using (var stream = File.OpenRead(path))
            {
                await bot.SendPhotoAsync(
                   chatId: chatId,
                   photo: stream!,
                   caption: $"💥  Juda yaxshi! \n\n{index + 1}  Karra jadavali testini bajarishga tayyormisiz?" +
                    "\n\nTayyor bo'lsangiz pastdagi   [ Boshladik ]  tugmasini bosing!",
                   replyMarkup: new InlineKeyboardMarkup(list),
                   cancellationToken: cts);
            }
        }
        catch
        {
            string path = @"multiplication\opps.jpg";

            using (var stream  = File.OpenRead(path))
            {
                await bot.SendPhotoAsync(
                   chatId: chatId,
                   photo: stream!,
                   caption: $"☣ No to'g'ri format kiritildi! \n\nIltimos berilgan tugmalardan birini tanlang! ",
                   replyMarkup: GetInlineButtonChooseTableMenu(),
                   cancellationToken: cts);
            }
        }
    }


    // Nomalum buyruqlar uchun
    public static async Task UnknownCommandAsync(ITelegramBotClient bot, long chatId, CancellationToken cts)
    {
        string path = @"multiplication\opps.jpg";

        using (var stream = File.OpenRead(path))
        {
            await bot.SendPhotoAsync(
               chatId: chatId,
               photo: stream!,
               caption: $"☢  No malum buyruq kiritildi! \nIltimos berilgan tugmalardan birini tanlang! ",
               replyMarkup: GetInlineButtonChooseTableMenu(),
               cancellationToken: cts);
        }
    }
}


