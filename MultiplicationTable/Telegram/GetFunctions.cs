using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MultiplicationTable.Telegram;

partial class Telegrambot
{
    // Kelgan message type ga qarab messagText, firstname, chatId, messageId, isSuccess qaytarishi uchun
    public static (string? messageText, string? firstName, int messageId, long chatId, bool isSoccess) GetMessage(Update update)
    {
        if (update.Type == UpdateType.Message)
            return (update.Message?.Text ?? "No text", update.Message?.From?.FirstName ?? "No Name", update.Message?.MessageId ?? -1, update.Message?.Chat.Id ?? -1, true);

        if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null && update.CallbackQuery.Data != null
            && update.CallbackQuery.Message != null)
            return (update.CallbackQuery.Data, update.CallbackQuery.From?.FirstName ?? "No Name", update.CallbackQuery.Message.MessageId, update.CallbackQuery.Message.Chat.Id, true);

        return (null, null, -1, -1, false);
    }


}
