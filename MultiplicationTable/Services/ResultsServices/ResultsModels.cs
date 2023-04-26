using MultiplicationTable.Models.ResultModel;
using Newtonsoft.Json;


namespace MultiplicationTable.Services.ResultsServices;

partial class SaveResults
{
    public List<Result>? Results { get; set; } 

    private readonly string FilePathForResults = "Results.json";

    public SaveResults()
    {
        Results = new List<Result>();
        _ = ReadResultsDataAsync();
    }

    public async Task SaveResultsDataAsync()
    {
        var json = JsonConvert.SerializeObject(Results);
        await File.WriteAllTextAsync(FilePathForResults, json);
    }

    public async Task ReadResultsDataAsync()
    {
        var textString = await File.ReadAllTextAsync(FilePathForResults);
        Results = JsonConvert.DeserializeObject<List<Result>>(textString);
    }

}
