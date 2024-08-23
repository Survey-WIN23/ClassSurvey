using ClassSurvey.Models;

namespace ClassSurvey.Helpers
{
    public class Validator
    {
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
}
