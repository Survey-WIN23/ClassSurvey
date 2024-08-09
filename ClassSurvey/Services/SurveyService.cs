using ClassSurvey.Factories;
using ClassSurvey.Models;
using System.Diagnostics;

namespace ClassSurvey.Services;

public class SurveyService()
{

    //public async Task<ResponseResult> GetQuestionsAsync()
    //{
    //    try
    //    {
    //        // Get all answers from API. 

    //        if (result.StatusCode == StatusCode.OK && result.ContentResult is IEnumerable<QuestionEntity> questionEntities)
    //        {
    //            var questions = QuestionFactory.Create(questionEntities);

    //            return new ResponseResult
    //            {
    //                StatusCode = result.StatusCode,
    //                ContentResult = questions,
    //            };
    //        }
    //        return ResponseFactory.Error("No questions available.");
    //    }
    //    catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    //}

    public async Task<bool> SubmitAnswersAsync(AnswerForm answer)
    {
        try
        {
            if (answer is null)
            {
                throw new ArgumentNullException(nameof(answer), "Answer cannot be null here.");
            }
                // Send answers to API
            return true;
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
