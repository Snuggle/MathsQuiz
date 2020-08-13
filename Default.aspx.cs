using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.Json;
using System.Web.UI.HtmlControls;

namespace SimpleWebMathsQuiz
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly Quiz quiz = new Quiz();

        public int GetResultsFromPage(UserResults userState, HtmlGenericControl stateDebug)
        {
            PreviousValues previous = userState.Previous;

            int correctAnswer = (int)quiz.GetCorrectAnswer(previous.PrevOperator, previous.PrevFirstNumber, previous.PrevSecondNumber);
            int.TryParse(Request.Form["text"], out int userAnswer);
            previous.PrevAnswerText = userAnswer;

            userState.UsersAnswers.Add(previous.PrevAnswerText);
            userState.UsersResults.Add(previous.PrevAnswerText == correctAnswer);

            stateDebug.InnerHtml = JsonSerializer.Serialize(userState);
            QuestionsRemaining.InnerText = "Questions Remaining: " + userState.HowManyQuestions;

            return correctAnswer;
        }

        public bool ProcessQuestion(UserResults userState)
        {

            if (userState.HowManyQuestions > 0)
            {
                int correctAnswer = GetResultsFromPage(userState, stateDebug);

                answerText.InnerText = quiz.IsTheUserCorrect(userState, correctAnswer);

                userState.HowManyQuestions--;
                return true;
            }
            else
            {
                bool isTrue(bool x) => x;
                int correctCount = userState.UsersResults.Count(isTrue);
                question.InnerText = "Congratulations! You have finished the quiz with " + correctCount + " out of " + (userState.UsersResults.Count()) + " correct! ";
                answerText.InnerText = "🎉🎉🎉";
                return false;
            }

        }

        public UserResults HandleButtonClick()
        {
            UserResults userState = JsonSerializer.Deserialize<UserResults>(Session["UserState"].ToString());
            if (userState.HowManyQuestions == null) // If no maths questions have been asked yet.
            {
                int.TryParse(Request.Form["text"], out int HowManyQuestions);
                HowManyQuestions--;

                userState.HowManyQuestions = HowManyQuestions;
            } else
            {
                bool shouldContinue = ProcessQuestion(userState);
                if (!shouldContinue) { return userState; }
            }
                Question randomlyGeneratedQuestion = quiz.AskQuestion(10);
                question.InnerText = randomlyGeneratedQuestion.QuestionText;

                userState.Previous.PrevFirstNumber = randomlyGeneratedQuestion.FirstNumber;
                userState.Previous.PrevSecondNumber = randomlyGeneratedQuestion.SecondNumber;
                userState.Previous.PrevOperator = randomlyGeneratedQuestion.Operator;
            
            return userState;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) // Button has been clicked
            {
                Session["UserState"] = JsonSerializer.Serialize(HandleButtonClick());
            }
            else
            {
                question.InnerText = "How many questions would you like to attempt?";
                UserResults newSession = new UserResults();
                newSession.UsersAnswers = new List<int>();
                newSession.UsersResults = new List<bool>();
                newSession.Previous = new PreviousValues();

                Session["UserState"] = JsonSerializer.Serialize<UserResults>(newSession);
            }
        }
    }
}