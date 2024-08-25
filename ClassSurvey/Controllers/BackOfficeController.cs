using ClassSurvey.Helpers;
using ClassSurvey.Models;
using ClassSurvey.Services;
using ClassSurvey.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey.Controllers;

[Authorize(Roles = "SuperUser")]
public class BackOfficeController(HttpClient httpClient, SurveyService surveyService, DataAggregationHelper dataAggregationHelper) : Controller
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly SurveyService _surveyService = surveyService;
    private readonly DataAggregationHelper _dataAggregationHelper = dataAggregationHelper;

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardVM
        {
            SurveyCount = await _surveyService.GetSurveyCountAsync()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> SurveyData()
    {
        var answers = await _surveyService.GetAnswersAsync();
        var questionsResult = await _surveyService.GetQuestionsAsync();
        var questions = questionsResult.ContentResult as List<Question>;

        if (questions != null)
        {
            var aggregatedData = _dataAggregationHelper.AggregateData(answers, questions);
            return View(aggregatedData);
        }

        var viewModel = new AggregatedQuestionDataVM();
        return View(viewModel);
    }

    public async Task<IActionResult> ManageQuestions()
    {
        return View();
    }
}
