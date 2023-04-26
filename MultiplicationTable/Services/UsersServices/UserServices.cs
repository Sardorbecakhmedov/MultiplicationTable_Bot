using MultiplicationTable.Models.UserModel;
using Newtonsoft.Json;

namespace MultiplicationTable.Services.UsersServices;

partial class UserServices
{
    public  List<User>? Users { get; set; }

    private readonly string FilePath = "UsersData.json";

    public UserServices()
    {
        Users = new List<User>();
        _ = ReadUserDataAsync(); // shu joyini ustozga ko'rsatish kerak
    }

    public async Task UpdateUserDataAsync(User user, ENextStep nextStep = default)
    {
        user.NextStep = nextStep;
        await SaveUserDataAsync(); ;
    }

    public async Task SaveUserDataAsync()
    {
        var jsonFile = JsonConvert.SerializeObject(Users);
        await File.WriteAllTextAsync(FilePath, jsonFile);
    }

    public async Task ReadUserDataAsync(CancellationToken cts = default)
    {
        if (File.Exists(FilePath))
        {
           var jsonFile = await File.ReadAllTextAsync(FilePath);
           Users = JsonConvert.DeserializeObject<List<User>>(jsonFile);
        }
        else
            Users = new List<User>();
    }

}
