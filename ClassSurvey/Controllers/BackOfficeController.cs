using ClassSurvey.Helpers;
using ClassSurvey.Models;
using ClassSurvey.Services;
using ClassSurvey.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClassSurvey.Controllers;

[Authorize(Roles = "SuperUser")]
public class BackOfficeController(SurveyService surveyService, DataAggregationHelper dataAggregationHelper) : Controller
{
    private readonly SurveyService _surveyService = surveyService;
    private readonly DataAggregationHelper _dataAggregationHelper = dataAggregationHelper;

    public async Task<IActionResult> Index()
    {
        try
        {
            var viewModel = new DashboardVM
            {
                SurveyCount = await _surveyService.GetSurveyCountAsync()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"{ex.Message}");
            throw;
        }
    }

    public async Task<IActionResult> SurveyData()
    {
        try
        {
            var answers = await _surveyService.GetAnswersAsync();
            var questionsResult = await _surveyService.GetQuestionsAsync();

            if (questionsResult.ContentResult is List<Question> questions)
            {
                var aggregatedData = _dataAggregationHelper.AggregateData(answers, questions);
                return View(aggregatedData);
            }

            var viewModel = new AggregatedQuestionDataVM();
            return View(viewModel);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"{ex.Message}");
            throw;
        }
       
    }

    public async Task<IActionResult> ManageQuestions()
    {
        return View();
    }
}
