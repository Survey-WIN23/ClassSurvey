using ClassSurvey.Factories;
using ClassSurvey.Models;
using ClassSurvey.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace ClassSurvey.Services;

public class SurveyService(HttpClient http, IConfiguration configuration)
{
    private readonly HttpClient _http = http;
    private readonly IConfiguration _configuration = configuration;

    public async Task<ResponseResult> GetQuestionsAsync()
    {
        // Gör en questionresult istället för att hantera callet snyggare?
        // Annars måste jag antingen köra dubbla serializers (Först ResponseResult, sen till Question).
        // Alternativt en JArray där content-result hämtas ut. 
        try
        {
            var response = await _http.GetAsync("http://localhost:7121/api/questions");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResponseResult>(json);

                if (result != null)
                {
                    try
                    {
                        var questions = JsonConvert.DeserializeObject<List<Question>>(result.ContentResult!.ToString()!);
                        var newQuestions = result.ContentResult!.ToString()!;

                        if (questions != null)
                        {
                            return ResponseFactory.Ok(questions);
                        }
                        else
                        {
                            return ResponseFactory.Error("Failed to convert content to List<Question>.");
                        }
                    }
                    catch (JsonException ex)
                    {
                        return ResponseFactory.Error($"Failed to deserialize content: {ex.Message}");
                    }
                }
            }

            return ResponseFactory.NotFound("No questions available.");
        }
        catch (JsonException jsonEx)
        {
            return ResponseFactory.Error($"JSON deserialization error: {jsonEx.Message}");
        }
        catch (HttpRequestException httpEx)
        {
            return ResponseFactory.Error($"HTTP request error: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error($"An unexpected error occurred: {ex.Message}");
        }
    }

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

    public async Task<bool> SubmitAnswersAsync(List<AnswerForm> answer)
    {
        try
        {
            if (answer is null)
            {
                throw new ArgumentNullException(nameof(answer), "Answer cannot be null here.");
            }

            var content = new StringContent(JsonConvert.SerializeObject(answer), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(" http://localhost:7121/api/submitAnswer", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Debug.WriteLine($"Failed to submit answers {response.StatusCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<int> GetSurveyCountAsync()
    {
        var response = await _http.GetStringAsync("http://localhost:7121/api/getAnswersCount");
        return JsonConvert.DeserializeObject<int>(response);
    }

    //public bool ValidateQuestionAnswer(Question question, Option selectedOption, string freeTextAnswer)
    //{
    //    if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.MultipleChoiceWithFreeText)
    //    {
    //        if (selectedOption is not null)
    //        {
    //            return false;
    //        }
    //    }

    //    if (question.Type == QuestionType.FreeText || question.Type == QuestionType.MultipleChoiceWithFreeText)
    //    {
    //        if (string.IsNullOrEmpty(freeTextAnswer))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
}
