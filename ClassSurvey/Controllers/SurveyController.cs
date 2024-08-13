using ClassSurvey.Models;
using ClassSurvey.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey.Controllers;

public class SurveyController(SurveyService surveyService) : Controller
{
    private readonly SurveyService _surveyService = surveyService;

    public async Task<IActionResult> Index(int id = 1)
    {
        var result = await _surveyService.GetQuestionsAsync();

        if (result.StatusCode != Models.StatusCode.OK || result.ContentResult == null)
        {
            // Change this to an error page instead of completed. 
            return RedirectToAction("Completed");
        }

        if (result.ContentResult is not IEnumerable<Question> questions || id > questions.Count() || id < 1)
        {
            return RedirectToAction("Completed");
        }

        var question = questions.FirstOrDefault(q => q.Id == id);

        if (question is null)
        {
            return RedirectToAction("Completed");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(int id, Option selectedOption, string responseText)
    {
        //var result = await _surveyService.GetQuestionsAsync();

        //if (result.StatusCode != Models.StatusCode.OK || result.ContentResult == null)
        //{
        //    return RedirectToAction("Completed");
        //}

        //if (result.ContentResult is not IEnumerable<Question> questions)
        //{
        //    return RedirectToAction("Completed");
        //}

        //var question = questions.FirstOrDefault(q => q.Id == id);

        //if (question == null)
        //{
        //    return RedirectToAction("Completed");
        //}

        //if (!_surveyService.ValidateQuestionAnswer(question, selectedOption, responseText))
        //{
        //    ModelState.AddModelError("", "Please provide valid answers.");
        //    return View(question);
        //}

        //var answerForm = new AnswerForm
        //{
        //    QuestionId = id,
        //    SelectedOption = selectedOption,
        //    ResponseText = responseText,
        //};

        //bool saveResult = await _surveyService.SaveQuestionAnswersAsync(answerForm);

        //if (!saveResult)
        //{
        //    ModelState.AddModelError("", "An error occurred while saving your answer.");
        //    return View(question);
        //}

        return RedirectToAction("Index", new { id = id + 1 });
    }

    public IActionResult Completed()
    {
        return View();
    }
}
