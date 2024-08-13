using ClassSurvey.Factories;
using ClassSurvey.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace ClassSurvey.Services;

public class SurveyService(HttpClient http, IConfiguration configuration)
{
    private readonly HttpClient _http = http;
    private readonly IConfiguration _configuration = configuration;

    public async Task<ResponseResult> GetQuestionsAsync()
    {
        try
        {
            var response = await _http.GetAsync(_configuration["ApiUris:Questions"]);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Question>(json);

                if (result is not null)
                {
                    return ResponseFactory.Ok(result);
                }
            }

            return ResponseFactory.Error("No questions available.");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }

    public async Task<bool> SubmitAnswersAsync(AnswerForm answer)
    {
        try
        {
            if (answer is null)
            {
                throw new ArgumentNullException(nameof(answer), "Answer cannot be null here.");
            }

            var content = new StringContent(JsonConvert.SerializeObject(answer), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(_configuration["Api:Answers"], content);

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

    public bool ValidateQuestionAnswer(Question question, Option selectedOption, string freeTextAnswer)
    {
        if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.MultipleChoiceWithFreeText)
        {
            if (selectedOption is not null)
            {
                return false;
            }
        }

        if (question.Type == QuestionType.FreeText || question.Type == QuestionType.MultipleChoiceWithFreeText)
        {
            if (string.IsNullOrEmpty(freeTextAnswer))
            {
                return false;
            }
        }
        return true;
    }
}
