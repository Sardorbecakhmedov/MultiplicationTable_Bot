using Telegram.Bot.Exceptions;
using Telegram.Bot;

namespace MultiplicationTable.Telegram;

partial class Telegrambot
{
    // Agar telegram funksiyalarida qandaydir error sodir bo'lsa, exeption qaytaradi
    public static Task PollingArrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cts)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:  {apiRequestException.ErrorCode},  {apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }

    public static Task<string> GetTextInfoBot()
    {
        
        return Task.FromResult($"🖐 Assalomu  alekum, xurmatli foydalanuvchi!" +
             $"\n\n📡 Ushbu bot orqali biz sizga 1️⃣ dan 1️⃣2️⃣ ko'paytiruv jadvaligacha test ko'rinishida savollar taqdim etganmiz." +
             $"\n\nHar bir jadvalda   1️⃣0️⃣  ta dan savol mavjud" +
             $"\n\nSiz berilgan jadvallar orasidan o'zingiz yoqtirgan jadvalga o'tib test bajarishingiz mumkin va test yakunada natijalaringizni " +
             $"ko'rishingiz mumkin." +

             $"\n\nKeling  BOT  buyruqlari bilan birgalikda tanishamiz!" +
             $"\n\n /start  -   testni boshlash uchun yuboriladi," +
             $"\n\n /my_results   -  test natijalarini ko'rish uchun yuboriladi," +
             $"\n\n /info  -  bot haqida malumot olish uchun yuboriladi.\"" +

             $"\n\n ❗ Agarda talab yoki takliflaringiz bo'lsa asosiy menuda TAKLIF YUBORISH TUGMASI ORQALI bizga yuborishingiz mumkun, " +
             $"imkon qadar talab va takliflarga binoan dasturni yaxshilashga xarakat qilamiz." +

             $"\n\n💻 Dasturchi: Akhmedov Sardorbek Ro'ziboy O'g'li " +
             $"\n\nDastur versiyasi: 1.1,16.03.2023");
    }
}
