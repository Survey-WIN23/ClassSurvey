using ClassSurvey.Entities;
using ClassSurvey.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ClassSurvey.Services;

public class AdminService(UserManager<UserEntity> userManager, HttpClient http, JWTService jwtService)
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly HttpClient _http = http;
    private readonly JWTService _jwtService = jwtService;

    public async Task<IEnumerable<AnswerForm>> GetAnswersAsync()
    {
        try
        {
            var responseString = await _http.GetStringAsync("http://localhost:7121/api/getAnswers");

            // Deserialisera responsen som en lista av ResponseResult
            var responseResults = JsonConvert.DeserializeObject<List<ResponseResult>>(responseString);
            var answers = new List<AnswerForm>();

            if (responseResults != null)
            {
                foreach (var result in responseResults)
                {
                    if (result.ContentResult != null)
                    {
                        // Deserialisera ContentResult till AnswerForm
                        var contentResultJson = result.ContentResult.ToString();
                        var answer = JsonConvert.DeserializeObject<AnswerForm>(contentResultJson);

                        if (answer != null)
                        {
                            answers.Add(answer);
                        }
                    }
                }
            }

            return answers;
        }
        catch (Exception)
        {
            return [];
        }
    }

    public async Task<int> GetSurveyCountAsync()
    {
        try
        {
            var response = await _http.GetStringAsync("http://localhost:7121/api/getAnswersCount");
            return JsonConvert.DeserializeObject<int>(response);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return -1;
        }
    }
}
