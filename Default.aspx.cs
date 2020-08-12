using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ;
using System.Text.Json;

namespace SimpleWeb
{
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

        public class UserResults
        {
            public IList<int> userAnswers { get; set; }
            public IList<bool> userResults { get; set; }
            public int HowManyQuestions { get; set; }
        }

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

        public Tuple<int, int> GetResultsFromPage(UserResults userState)
        {
            int.TryParse(Request.Form["firstNumber"], out int firstNum);
            int.TryParse(Request.Form["secondNumber"], out int secondNum);
            char oper = Request.Form["operators"][0];

            int correctAnswer = (int)GetCorrectAnswer(oper, firstNum, secondNum);

            int.TryParse(Request.Form["text"], out int userAnswer);

            userState.userAnswers.Add(userAnswer);
            userState.userResults.Add(userAnswer == correctAnswer);

            string debugString = JsonSerializer.Serialize(userState);

            stateDebug.InnerHtml = debugString + " ~ Questions Remaining: " + userState.HowManyQuestions;

            return Tuple.Create(userAnswer, correctAnswer);
        }

        public void AskQuestion(question, firstNumber, secondNumber, operators)
        {
            (string questionText, int firstNum, int secondNum, char op) = Web_AskQuestion(, 10);
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
            Quiz Quiz = new Quiz();

            question.InnerText = "How many questions would you like to attempt?";

            if (IsPostBack) // Button has been clicked
            {
                Quiz.AskQuestion();

                HandleButtonClick();
            }
        }
    }
}