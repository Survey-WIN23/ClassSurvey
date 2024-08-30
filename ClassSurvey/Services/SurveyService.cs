using ClassSurvey.Factories;
using ClassSurvey.Models;
using ClassSurvey.ViewModels;
using Markdig;
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

    //public async Task<SurveyVM> GetAnalysisAsync()
    //{
    //    // Testdata för att simulera ett svar från OpenAI
    //    var testAnalysisText = @"
    //                            **Sammanfattning av enkätsvaren:**

    //                            **Positiva aspekter:**
    //                            - De flesta deltagare är mycket nöjda med utbildningens kvalitet och tycker att den var informativ och välstrukturerad. 
    //                            - Många uppskattar den engagerande undervisningen och de praktiska övningarna som hjälper till att förstå teorin bättre.

    //                            **Neutrala aspekter:**
    //                            - Några deltagare har nämnt att utbildningen var bra men att vissa delar var lite repetitiva. 
    //                            - Det finns kommentarer om att kursmaterialet kunde ha varit mer uppdaterat.

    //                            **Negativa aspekter:**
    //                            - En del deltagare upplever att utbildningen var för kort och att vissa ämnen inte täcktes tillräckligt djupgående.
    //                            - Det finns också feedback om att logistik och schemaläggning kunde förbättras för att underlätta deltagarnas planering.

    //                            **Exempel på citat:**
    //                            - 'Utbildningen var fantastisk och lärorik, men vissa delar kändes överflödiga.'
    //                            - 'Jag önskar att det fanns mer djupgående material om de avancerade ämnena.'

    //                            **Bedömning:**
    //                            - Den allmänna uppfattningen om utbildningen är positiv, men det finns utrymme för förbättringar, särskilt när det gäller längden på utbildningen och detaljeringsgraden i vissa ämnen.
    //                            ";


    //    var pipeline = new MarkdownPipelineBuilder()
    //        .UseAdvancedExtensions() 
    //        .Build(); 
    //    string convertedToHTML = Markdown.ToHtml(testAnalysisText, pipeline);
    //    return await Task.FromResult(new SurveyVM
    //    {

    //        Data = [],
    //        OverallAnalysis = convertedToHTML
    //    });
    //}

    // The "real" method when OpenAI is up and running.
    public async Task<SurveyVM> GetAnalysisAsync()
    {

        var requestUrl = $"http://localhost:7121/api/getAIResponse";

        try
        {
            var response = await _http.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var apiAnalysisResults = await response.Content.ReadAsStringAsync();
                if (apiAnalysisResults != null)
                {

                    var pipeline = new MarkdownPipelineBuilder()
                                .UseAdvancedExtensions()
                                .Build();

                    string analysConvertedToHTML = Markdown.ToHtml(apiAnalysisResults, pipeline);
                    return await Task.FromResult(new SurveyVM
                    {

                        OverallAnalysis = analysConvertedToHTML
                    });
                }
            }
            else
            {
                Debug.WriteLine($"API responded with error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching analysis: {ex.Message}");
        }

        return new SurveyVM
        {
            Data = new List<AggregatedQuestionDataVM>(),
            OverallAnalysis = null
        };
    }
}
