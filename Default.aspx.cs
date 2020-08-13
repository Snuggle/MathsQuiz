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
    public class UserResults
    {
        public List<int> UsersAnswers { get; set; }
        public List<bool> UsersResults { get; set; }
        public int HowManyQuestions { get; set; }

            public int prevFirstNumber { get; set; }
            public int prevSecondNumber { get; set; }
            public char prevOperator { get; set; }
            public int prevAnswerText { get; set; }
    }

    public class Quiz
    {
        private readonly Random rnd = new Random();
        private readonly Dictionary<char, Func<double, double, double>> potato =
            new Dictionary<char, Func<double, double, double>>()
            {
                { '+', (oneNum, twoNum) => oneNum + twoNum },
                { '-', (oneNum, twoNum) => oneNum - twoNum },
                { '*', (oneNum, twoNum) => oneNum * twoNum },
                { '/', (oneNum, twoNum) => oneNum / twoNum },
                { '√', (oneNum, twoNum) => (int) Math.Sqrt(oneNum) },
                { '^', (oneNum, twoNum) => (int) Math.Pow(oneNum, twoNum) }
            };

        public double GetCorrectAnswer(char op, int firstNum, int secondNum)
        {
            try
            {
                return potato[op](firstNum, secondNum);
            } catch {
                return 999;
            }
        }

        public Tuple<string, int, int, char> AskQuestion(int maxRandNumber)
        {
            int index = rnd.Next(potato.Count);
            char op = potato.Keys.ElementAt(index);

            int firstNum = rnd.Next(1, maxRandNumber);
            int secondNum = rnd.Next(1, maxRandNumber);

            if (op.Equals('/'))
            {
                firstNum *= secondNum;
            }

            string questionText = $"What is the answer to {firstNum} {op} {secondNum}? "; // Much nicer, string interpolation! Closes #12. 
            return Tuple.Create(questionText, firstNum, secondNum, op);
        }

        public string IsTheUserCorrect(UserResults userState, int correctAnswer)
        {
            if (userState.prevAnswerText.Equals(correctAnswer))
            {
                return $"YAY, CORRECT! ✅🎉";
            }
            return $"Wrong. '{userState.prevFirstNumber} {userState.prevOperator} {userState.prevSecondNumber}' is {correctAnswer}! ❌";
        }
    }

    public partial class Default : System.Web.UI.Page
    {
        private readonly Quiz quiz = new Quiz();

        public int GetResultsFromPage(UserResults userState, HtmlGenericControl stateDebug)
        {
            int correctAnswer = (int)quiz.GetCorrectAnswer(userState.prevOperator, userState.prevFirstNumber, userState.prevSecondNumber);
            int.TryParse(Request.Form["text"], out int userAnswer);
            userState.prevAnswerText = userAnswer;

            userState.UsersAnswers.Add(userState.prevAnswerText);
            userState.UsersResults.Add(userState.prevAnswerText == correctAnswer);

            stateDebug.InnerHtml = JsonSerializer.Serialize(userState);
            QuestionsRemaining.InnerText = "Questions Remaining: " + userState.HowManyQuestions;

            return correctAnswer;
        }

        public void ProcessQuestion(UserResults userState)
        {

            if (userState.HowManyQuestions > 0)
            {
                int correctAnswer = GetResultsFromPage(userState, stateDebug);

                answerText.InnerText = quiz.IsTheUserCorrect(userState, correctAnswer);

                userState.HowManyQuestions--;
            }
            else
            {
                GetResultsFromPage(userState, stateDebug);

                bool isTrue(bool x) => x;
                int correctCount = userState.UsersResults.Count(isTrue);
                question.InnerText = "Congratulations! You have finished the quiz with " + correctCount + " out of " + (userState.UsersResults.Count()) + " correct! ";
                answerText.InnerText = "🎉🎉🎉";
                Session["UserState"] = null;
            }

        }

        public UserResults HandleButtonClick()
        {
            UserResults userState = JsonSerializer.Deserialize<UserResults>(Session["UserState"].ToString());
            if (userState.HowManyQuestions == -1) // If no maths questions have been asked yet.
            {
                int.TryParse(Request.Form["text"], out int HowManyQuestions);
                HowManyQuestions--;

                userState.HowManyQuestions = HowManyQuestions;
            }
            else // The user has submitted answers
            {
                (string questionText, int firstNum, int secondNum, char op) = quiz.AskQuestion(10);
                question.InnerText = questionText;

                ProcessQuestion(userState);

                userState.prevFirstNumber = firstNum;
                userState.prevSecondNumber = secondNum;
                userState.prevOperator = op;
            }
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
                newSession.HowManyQuestions = -1;

                Session["UserState"] = JsonSerializer.Serialize<UserResults>(newSession);
            }
        }
    }
}