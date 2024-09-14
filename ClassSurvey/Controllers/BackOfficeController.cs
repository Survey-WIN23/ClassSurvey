using ClassSurvey.Helpers;
using ClassSurvey.Models;
using ClassSurvey.Services;
using ClassSurvey.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClassSurvey.Controllers;

[Authorize(Roles = "SuperUser")]
public class BackOfficeController(SurveyService surveyService, DataAggregationHelper dataAggregationHelper, AdminService adminService) : Controller
{
    private readonly SurveyService _surveyService = surveyService;
    private readonly DataAggregationHelper _dataAggregationHelper = dataAggregationHelper;
    private readonly AdminService _adminService = adminService;

    public async Task<IActionResult> Index()
    {
        try
        {
            var viewModel = new DashboardVM
            {
                SurveyCount = await _adminService.GetSurveyCountAsync()
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
            var answers = await _adminService.GetAnswersAsync();
            var questionsResult = await _surveyService.GetQuestionsAsync();

            if (questionsResult.ContentResult is List<Question> questions)
            {
                var aggregatedData = _dataAggregationHelper.AggregateData(answers, questions);
                var analysisResponse = await _surveyService.GetAnalysisAsync();

                var viewmodel = new SurveyVM
                {
                    Data = aggregatedData,
                    OverallAnalysis = analysisResponse.OverallAnalysis,
                };
                return View(viewmodel);
            }

            var emptyViewModel = new SurveyVM
            {
                Data = new List<AggregatedQuestionDataVM>(),
                OverallAnalysis = "No analysis available."
            };

            return View(emptyViewModel);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"{ex.Message}");
            throw;
        }
    }

    //[HttpPost]
    //public async Task<IActionResult> GetAnalysis()
    //{
    //    try
    //    {
    //        var analysisResponse = await _surveyService.GetAnalysisAsync();

    //        return PartialView("_AnalysisPartial", analysisResponse);
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log exception and handle errors as needed
    //        return PartialView("_AnalysisPartial", new AnalysisViewModel
    //        {
    //            OverallAnalysis = $"An error occurred: {ex.Message}"
    //        });
    //    }
    //}
}
