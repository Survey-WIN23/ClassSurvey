using ClassSurvey.Models;
using ClassSurvey.ViewModels;
using System.Diagnostics;

namespace ClassSurvey.Helpers
{
    public class DataAggregationHelper
    {
        public List<AggregatedQuestionDataVM> AggregateData(IEnumerable<AnswerForm> answers, IEnumerable<Question> questions)
        {
            var questionDictionary = questions.ToDictionary(q => q.Id);
            var aggregatedData = new List<AggregatedQuestionDataVM>();

            var questionGroups = answers.GroupBy(a => a.QuestionId);

            foreach (var group in questionGroups)
            {
                var questionId = group.Key;
                var question = questionDictionary.GetValueOrDefault(questionId);

                if (question == null)
                {
                    Debug.WriteLine($"No question found with ID: {questionId}");
                    continue;
                }

                var optionsCount = new Dictionary<string, int>();

                foreach (var answer in group)
                {
                    var optionValue = answer.SelectedOption?.Value ?? "No Option";
                    if (optionsCount.TryGetValue(optionValue, out int value))
                    {
                        optionsCount[optionValue] = ++value;
                    }
                    else
                    {
                        optionsCount[optionValue] = 1;
                    }
                }

                var responseText = group.FirstOrDefault()?.ResponseText ?? string.Empty;

                aggregatedData.Add(new AggregatedQuestionDataVM
                {
                    QuestionId = questionId,
                    QuestionText = question.QuestionText!,
                    OptionsCount = optionsCount,
                    ResponseText = responseText
                });
            }

            return aggregatedData;
        }
    }
}
