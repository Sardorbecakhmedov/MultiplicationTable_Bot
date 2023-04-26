using User = MultiplicationTable.Models.UserModel.User;

namespace MultiplicationTable.Services.UsersServices;

partial class UserServices
{
    public async Task<User> GetUser(long chatId)
    {
        User? user = Users?.FirstOrDefault(u => u.ChatId == chatId);

        if (user == null)
        {
            user = new User();
            user.ChatId = chatId;
            user.IsStart = false;
            user.CorrectAnswerCount = 0;
            user.UserStatusStartTest = false;

            Users!.Add(user); //  ! belgi qo'yilgan, Shu joyini ustozga ko'rsatish kerak
            await SaveUserDataAsync();
        }

        return user;
    }
}
