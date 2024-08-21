using ClassSurvey.Helpers;
using ClassSurvey.Models;
using ClassSurvey.Services;
using ClassSurvey.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey.Controllers;

public class SurveyController(SurveyService surveyService) : Controller
{
    private readonly SurveyService _surveyService = surveyService;
    private const string AnswerSessionKey = "UserAnswers";

    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        var result = await _surveyService.GetQuestionsAsync();

        if (result.StatusCode != Models.StatusCode.OK || result.ContentResult == null)
        {
            return RedirectToAction("Completed");
        }

        if (result.ContentResult is List<Question> questions)
        {
            if (pageNumber < 1 || pageNumber > questions.Count)
            {
                return NotFound();
            }

            var userAnswers = HttpContext.Session.GetObjectFromJson<List<AnswerVM>>(AnswerSessionKey) ?? new List<AnswerVM>();

            var question = questions[pageNumber - 1];
            var viewModel = new QuestionVM
            {
                Questions = new List<Question> { question },
                CurrentPage = pageNumber,
                TotalPages = questions.Count,
                UserAnswers = userAnswers
            };

            //viewModel.Answers = answers;
            return View("Index", viewModel);
        }

        return View("Index", new QuestionVM());
    }

    [HttpPost]
    public async Task<IActionResult> SaveAnswer(int QuestionId, int? optionId, string responseText, bool IsFreeText, bool isFinalSubmit = false)
    {
        // Skapa en ny instans av AnswerVM med de mottagna värdena
        var answer = new AnswerVM
        {
            QuestionId = QuestionId,
            OptionId = optionId,
            ResponseText = responseText,
            IsFreeText = !string.IsNullOrWhiteSpace(responseText)
        };

        // Hantera sparande av svar (t.ex. spara i session eller databas)
        var userAnswers = HttpContext.Session.GetObjectFromJson<List<AnswerVM>>(AnswerSessionKey) ?? new List<AnswerVM>();

        var existingAnswer = userAnswers.FirstOrDefault(a => a.QuestionId == answer.QuestionId);
        if (existingAnswer != null)
        {
            userAnswers.Remove(existingAnswer);
        }
        userAnswers.Add(answer);

        HttpContext.Session.SetObjectAsJson(AnswerSessionKey, userAnswers);

        // Hämta alla frågor
        var result = await _surveyService.GetQuestionsAsync();
        if (result.StatusCode != Models.StatusCode.OK || result.ContentResult == null)
        {
            return RedirectToAction("Completed");
        }

        if (result.ContentResult is not List<Question> questions || questions.Count == 0)
        {
            return RedirectToAction("Completed");
        }

        // Hämta den aktuella sidan från session
        var currentPageNumber = HttpContext.Session.GetInt32("CurrentPageNumber") ?? 1;

        // Beräkna nästa sidnummer
        var nextPageNumber = currentPageNumber + 1;

        if (isFinalSubmit || nextPageNumber > questions.Count)
        {
            // Spara den aktuella sidan som slutlig inlämning
            HttpContext.Session.SetInt32("CurrentPageNumber", currentPageNumber);

            // Här anropar vi en metod som hanterar inskickning av alla svar
            return await SubmitAnswers();
        }

        // Kontrollera att nästa sida är inom det tillåtna intervallet
        if (nextPageNumber < 1 || nextPageNumber > questions.Count)
        {
            return RedirectToAction("Completed");
        }

        // Spara nästa sida i sessionen
        HttpContext.Session.SetInt32("CurrentPageNumber", nextPageNumber);

        // Navigera till nästa sida
        return RedirectToAction("Index", new { pageNumber = nextPageNumber });
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAnswers()
    {
        var userAnswers = HttpContext.Session.GetObjectFromJson<List<AnswerVM>>(AnswerSessionKey);

        if (userAnswers == null || userAnswers.Count == 0)
        {
            return RedirectToAction("Index");
        }

        var answerForms = userAnswers.Select(answerVm => new AnswerForm
        {
            QuestionId = answerVm.QuestionId,
            OptionId = answerVm.OptionId,  
            ResponseText = answerVm.ResponseText,  
            IsFreeText = answerVm.IsFreeText 
        }).ToList();

        var success = await _surveyService.SubmitAnswersAsync(answerForms);

        if (success)
        {
            HttpContext.Session.Remove(AnswerSessionKey);
            return RedirectToAction("Completed");
        }

        return View("Error");
    }

    public IActionResult Completed()
    {
        return View();
    }
}
