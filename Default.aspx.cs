using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MathsQuiz;
using System.Text.Json;

namespace SimpleWebMathsQuiz
{
    public partial class Default : System.Web.UI.Page
    {
        public class UserResults
        {
            public IList<int> userAnswers { get; set; }
            public IList<bool> userResults { get; set; }
            public int HowManyQuestions { get; set; }
        }

        public Tuple<int, int> GetResultsFromPage(Program MathsQuiz, UserResults userState)
        {
            int.TryParse(Request.Form["firstNumber"], out int firstNum);
            int.TryParse(Request.Form["secondNumber"], out int secondNum);
            char oper = Request.Form["operators"][0];

            int correctAnswer = (int)MathsQuiz.GetCorrectAnswer(oper, firstNum, secondNum);

            int.TryParse(Request.Form["text"], out int userAnswer);

            userState.userAnswers.Add(userAnswer);
            userState.userResults.Add(userAnswer == correctAnswer);

            string debugString = JsonSerializer.Serialize(userState);

            stateDebug.InnerHtml = debugString + " ~ Questions Remaining: " +  userState.HowManyQuestions;

            return Tuple.Create(userAnswer, correctAnswer);
        }

        public void AskQuestion(Program MathsQuiz)
        {
            (string questionText, int firstNum, int secondNum, char op) = MathsQuiz.Web_AskQuestion(MathsQuiz, 10);
            question.InnerText = questionText;
            firstNumber.Attributes["value"] = firstNum.ToString();
            secondNumber.Attributes["value"] = secondNum.ToString();
            operators.Attributes["value"] = op.ToString();
        }

        public string IsTheUserCorrect(Tuple<int, int> results)
        {
            (int userAnswer, int correctAnswer) = results;
            if (userAnswer.Equals(correctAnswer))
            {
                return $"The answer you had provided to '{Request.Form["firstNumber"]} {Request.Form["operators"]} {Request.Form["secondNumber"]}' was: {Request.Form["text"]}. YAY, CORRECT! ✅🎉";
            }
            return $"The answer you had provided to '{Request.Form["firstNumber"]} {Request.Form["operators"]} {Request.Form["secondNumber"]}' was: {Request.Form["text"]}. Sadly, you were wrong... It was {correctAnswer}! ❌";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Program MathsQuiz = new MathsQuiz.Program();

            question.InnerText = "How many questions would you like to attempt?";

            if (IsPostBack) // Button has been clicked
            {
                AskQuestion(MathsQuiz);

                if (UserAnswers.Attributes["value"]==null) // If no maths questions have been asked yet.
                {
                    int.TryParse(Request.Form["text"], out int HowManyQuestions);
                    HowManyQuestions--;
                    UserAnswers.Attributes["value"] = "{\"userAnswers\": [],\"userResults\": [],\"HowManyQuestions\":"+HowManyQuestions+"}";
                } else // The user has submitted answers
                {
                    UserResults userState = JsonSerializer.Deserialize<UserResults>(Request.Form["UserAnswers"]);

                    if (userState.HowManyQuestions > 0)
                    {
                        Tuple<int, int> results = GetResultsFromPage(MathsQuiz, userState);

                        answerText.InnerText = IsTheUserCorrect(results);

                        userState.HowManyQuestions--;
                        UserAnswers.Attributes["value"] = JsonSerializer.Serialize(userState);
                    } else
                    {
                        GetResultsFromPage(MathsQuiz, userState);

                        Func<bool, bool> isTrue = x => x;
                        int correctCount = userState.userResults.Count(isTrue);
                        question.InnerText = "Congratulations! You have finished the quiz with " + correctCount + " out of " + (userState.userResults.Count()) + " correct! ";
                        answerText.InnerText = "🎉🎉🎉";
                    }
                }
            }
        }
    }
}