using MultiplicationTable.Models.ResultModel;
using MultiplicationTable.Services.QuestionsServices;
using Telegram.Bot.Types.ReplyMarkups;


namespace MultiplicationTable.Telegram;

partial class Telegrambot
{
    // Bosh menuga, barcha karra jadvallar ro'yxati uchun buttonlar yasash
    public static InlineKeyboardMarkup GetInlineButtonChooseTableMenu()
    {
        var inlineButton = new List<List<InlineKeyboardButton>>();
        int index = 1;

        for (int i = 0; i < 4; i++)
        {
            var inline = new List<InlineKeyboardButton>();

            for (int j = 0; j < 3; j++)
            {
                inline.Add(InlineKeyboardButton.WithCallbackData($"{index}   Karra", $"karra...{index - 1}"));
                index++;
            }

            inlineButton.Add(inline);
        }

        var but1 = new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("↩  Asosiy menuga qaytish", "/start") };
        var but2 = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("👁‍  Test natijalarimni ko'rish", "/my_results" ) };

        inlineButton.Add(but1);
        inlineButton.Add(but2);

        return new InlineKeyboardMarkup(inlineButton);
    }


    // Savollarning variyantlariga button yasash uchun
    public static InlineKeyboardMarkup GetInlineButtonForOptions(List<string> options)
    {
        var inlineButoons = new List<List<InlineKeyboardButton>>();

        for (int i = 0; i < options.Count; i++)
        {
            var button = new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData(options[i], options[i].ToString())
            };

            inlineButoons.Add(button);
        }

    //    var but1 = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("↩  Asosiy menuga qaytish", "/start") };
        var but2 = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("♻  Qayta test bajarish", "/testniboshla") };

    //    inlineButoons.Add(but1);
        inlineButoons.Add(but2);

        return new InlineKeyboardMarkup(inlineButoons);
    }


    //  Javobni teskrirish va edit qilish uchun inline buttonchalar!
    public static InlineKeyboardMarkup EditInlineButtonForOptions(Models.UserModel.User user, List<string> options, string messageText)
    {
        var inlineButoons = new List<List<InlineKeyboardButton>>();

        var question = QustionServices.GetQuestionsList(user.CurrentMultiplicatioinIndex);

        for (int i = 0; i < options.Count; i++)
        {
            List<InlineKeyboardButton> button;

            if (options[i].ToString() == messageText)
            {
                if (question[user.CurrentQuestionIndex].CorrectAnswer == messageText)
                {
                    button = new List<InlineKeyboardButton>()
                    {
                        InlineKeyboardButton.WithCallbackData($"{options[i]}   ✅", options[i].ToString())
                    };

                    inlineButoons.Add(button);
                    continue;
                }
                else
                {
                    button = new List<InlineKeyboardButton>()
                    {
                        InlineKeyboardButton.WithCallbackData($"{options[i]}   ❌", options[i].ToString())
                    };

                    inlineButoons.Add(button);
                    continue;
                }
            }
            else if (question[user.CurrentQuestionIndex].CorrectAnswer == options[i].ToString())
            {
                button = new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData($"{options[i]}   ✅", options[i].ToString())
                };
                inlineButoons.Add(button);
                continue;
            }
            else
            {
                button = new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(options[i], options[i].ToString())
                };
                inlineButoons.Add(button);
            }
        }
        return new InlineKeyboardMarkup(inlineButoons);
    }


    // Test tugagandan kegn jo'natish uchun
    public static InlineKeyboardMarkup GetInlineButtonForMainMenu()
    {
        var buttons = new List<List<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("↩  Asosiy menuga qaytish", "/start" ) },
            new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("👁‍  Test natijalarimni ko'rish", "/my_results" ) },
            new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("♻  Qayta test bajarish", "/testniboshla") }
    };

        return new InlineKeyboardMarkup(buttons);
    }

    // Userga resultlarni jo'natish uchun
    public static InlineKeyboardMarkup GetInlineButtonForResults(List<Result> myResults)
    {
        var buttons = new List<List<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("📂"),
                InlineKeyboardButton.WithCallbackData("✅  / ❓"),
                InlineKeyboardButton.WithCallbackData("📆"),
                InlineKeyboardButton.WithCallbackData("🗑")
            }
        };

        for (int i = 0; i < myResults.Count; i++)
        {
            var button = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(myResults[i].MultiplicationName.ToString()),
                InlineKeyboardButton.WithCallbackData($"{myResults[i].CorrectAnswerCount} / {myResults[i].QuestionCount}"),
                InlineKeyboardButton.WithCallbackData(myResults[i]._DateTime.ToShortDateString()),
                InlineKeyboardButton.WithCallbackData("❌", $"index❌...!{i}"),
            };
            buttons.Add(button);
        }

        var but1 = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("↩  Asosiy menuga qaytish", "/start" ) };
        var but2 = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("♻  Qayta test bajarish", "/testniboshla") };

        buttons.Add(but1);
        buttons.Add(but2);

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup GetButtonMainMenu()
    {
        var buttons = new List<List<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("▶  Test bajarish", "/testniboshla") },
            new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("👁‍  Test natijalarimni ko'rish", "/my_results") },
            new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("📕  Bot haqida malumotlar", "/info") },
            new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("📲  Dasturchiga taklif yuborish ", "mengaxabar...!") },
        };

        return new InlineKeyboardMarkup(buttons);
    }

}