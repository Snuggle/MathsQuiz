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
    public class SubmittedData
    {
        public int firstNumber { get; set; }
        public int secondNumber { get; set; }
        public int operatororor { get; set; }
        public int answerText { get; set; }
    }

    public class UserResults
    {
        public IList<int> userAnswers { get; set; }
        public IList<bool> userResults { get; set; }
        public int HowManyQuestions { get; set; }
    }

    public class Quiz
    {
        Random rnd = new Random();
        Dictionary<char, Func<double, double, double>> potato =
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
            return potato[op](firstNum, secondNum);
        }

        public Tuple<string, int, int, char> Web_AskQuestion(int maxRandNumber)
        {
            int index = rnd.Next(potato.Count);
            char op = potato.Keys.ElementAt(index);

            int firstNum = rnd.Next(1, maxRandNumber);
            int secondNum = rnd.Next(1, maxRandNumber);

            string questionText = $"What is the answer to {firstNum} {op} {secondNum}? "; // Much nicer, string interpolation! Closes #12. 
            return Tuple.Create(questionText, firstNum, secondNum, op);
        }


        public void AskQuestion(HtmlGenericControl question, HtmlInputHidden firstNumber, HtmlInputHidden secondNumber, HtmlInputHidden operators)
        {
            (string questionText, int firstNum, int secondNum, char op) = Web_AskQuestion(10);
            question.InnerText = questionText;
            firstNumber.Attributes["value"] = firstNum.ToString();
            secondNumber.Attributes["value"] = secondNum.ToString();
            operators.Attributes["value"] = op.ToString();
        }

        public string IsTheUserCorrect(Tuple<int, int> results, string text, SubmittedData something)
        {
            (int userAnswer, int correctAnswer) = results;
            if (userAnswer.Equals(correctAnswer))
            {
                return $"The answer you had provided to '{something.firstNumber} {something.operatororor} {something.secondNumber}' was: {text}. YAY, CORRECT! ✅🎉";
            }
            return $"The answer you had provided to '{something.firstNumber} {something.operatororor} {something.secondNumber}' was: {text}. Sadly, you were wrong... It was {correctAnswer}! ❌";
        }
    }

    public partial class Default : System.Web.UI.Page
    {
        Quiz quiz = new Quiz();

        public Tuple<int, int> GetResultsFromPage(UserResults userState, HtmlGenericControl stateDebug)
        {
            int.TryParse(Request.Form["firstNumber"], out int firstNum);
            int.TryParse(Request.Form["secondNumber"], out int secondNum);
            char oper = Request.Form["operators"][0];

            int correctAnswer = (int)quiz.GetCorrectAnswer(oper, firstNum, secondNum);

            int.TryParse(Request.Form["text"], out int userAnswer);

            userState.userAnswers.Add(userAnswer);
            userState.userResults.Add(userAnswer == correctAnswer);

            string debugString = JsonSerializer.Serialize(userState);

            stateDebug.InnerHtml = debugString + " ~ Questions Remaining: " + userState.HowManyQuestions;

            return Tuple.Create(userAnswer, correctAnswer);
        }

        public SubmittedData CaptureSubmittedData()
        {
            SubmittedData something = new SubmittedData();
            something.firstNumber = int.Parse(Request.Form["firstNumber"]);
            something.secondNumber = int.Parse(Request.Form["secondNumber"]);
            something.operatororor = int.Parse(Request.Form["operators"]);

            something.answerText = int.Parse(Request.Form["text"]);

            return something;
        }

        public void ProcessQuestion(UserResults userState)
        {
            if (userState.HowManyQuestions > 0)
            {
                Tuple<int, int> results = GetResultsFromPage(userState, stateDebug);

                SubmittedData UserSubmittedData = CaptureSubmittedData();

                answerText.InnerText = quiz.IsTheUserCorrect(results, Request.Form["text"], UserSubmittedData);

                userState.HowManyQuestions--;
                UserAnswers.Attributes["value"] = JsonSerializer.Serialize(userState);
            }
            else
            {
                GetResultsFromPage(userState, stateDebug);

                Func<bool, bool> isTrue = x => x;
                int correctCount = userState.userResults.Count(isTrue);
                question.InnerText = "Congratulations! You have finished the quiz with " + correctCount + " out of " + (userState.userResults.Count()) + " correct! ";
                answerText.InnerText = "🎉🎉🎉";
            }
        }

        public void HandleButtonClick()
        {
            if (UserAnswers.Attributes["value"] == null) // If no maths questions have been asked yet.
            {
                int.TryParse(Request.Form["text"], out int HowManyQuestions);
                HowManyQuestions--;
                UserAnswers.Attributes["value"] = "{\"userAnswers\": [],\"userResults\": [],\"HowManyQuestions\":" + HowManyQuestions + "}";

            }
            else // The user has submitted answers
            {
                UserResults userState = JsonSerializer.Deserialize<UserResults>(Request.Form["UserAnswers"]);

                ProcessQuestion(userState);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            question.InnerText = "How many questions would you like to attempt?";

            if (IsPostBack) // Button has been clicked
            {
                quiz.AskQuestion(question, firstNumber, secondNumber, operators);

                HandleButtonClick();
            }
        }
    }
}