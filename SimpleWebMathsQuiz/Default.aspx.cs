using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.Json;
using System.Web.UI.HtmlControls;
using QuizLib;

namespace SimpleWebMathsQuiz
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly Quiz quiz = new Quiz();

        public bool GameShouldContinue = true;

        private UserResults userState = new UserResults();

        public int GetResultsFromPage(HtmlGenericControl stateDebug)
        {
            int userAnswer = int.Parse(Request.Form["text"]);
            userState.Previous.PrevAnswerText = userAnswer;

            int correctAnswer = quiz.PrevGetCorrectAnswer(userState.Previous);

            userState.UsersAnswers.Add(userState.Previous.PrevAnswerText);
            userState.UsersResults.Add(userAnswer == correctAnswer);
            stateDebug.InnerHtml = JsonSerializer.Serialize(userState);

            QuestionsRemaining.InnerText = "Questions Remaining: " + userState.HowManyQuestions;
            return correctAnswer;
        }

        public void ProcessQuestion()
        {
            userState.HowManyQuestions--;
            int correctAnswer = GetResultsFromPage(stateDebug);
            answerText.InnerText = quiz.IsTheUserCorrect(userState, correctAnswer);
            
            if (userState.HowManyQuestions <= 0)
            {
                GameFinishedEvent();
            }
        }

        public void GameFinishedEvent()
        {
            bool isTrue(bool result) => result;
            int correctCount = userState.UsersResults.Count(isTrue);

            question.InnerText = "Congratulations! You have finished the quiz with " + correctCount + " out of " + (userState.UsersResults.Count()) + " correct! ";
            answerText.InnerText = "🎉🎉🎉";
            quizElements.Visible = false;
            GameShouldContinue = false;
        }

        public void GenerateQuestionText()
        {
            Question randomlyGeneratedQuestion = quiz.AskQuestion(10);
            question.InnerText = randomlyGeneratedQuestion.QuestionText;

            SetPreviousQuestionValues(randomlyGeneratedQuestion);
        }

        public void SetPreviousQuestionValues(Question question)
        {
            userState.Previous.PrevFirstNumber = question.FirstNumber;
            userState.Previous.PrevSecondNumber = question.SecondNumber;
            userState.Previous.PrevOperator = question.Operator;
        }

        public bool GameHasNotStarted()
        {
            return (userState.HowManyQuestions == null);
        }

        public void SetupGame()
        {
            int.TryParse(Request.Form["text"], out int HowManyQuestions);
            userState.HowManyQuestions = HowManyQuestions;
        }

        public void HandleButtonClick()
        {
            userState = JsonSerializer.Deserialize<UserResults>(Session["UserState"].ToString());

            if (GameHasNotStarted()) // If no maths questions have been asked yet.
            {
                SetupGame();
            } 
            else
            {
                ProcessQuestion();
            }


            if (GameShouldContinue)
            {
                GenerateQuestionText();
            }

            Session["UserState"] = JsonSerializer.Serialize(userState);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) // Button has been clicked
            {
                HandleButtonClick();
                text.Value = "";
            }
            else
            {
                question.InnerText = "How many questions would you like to attempt?";

                Session["UserState"] = JsonSerializer.Serialize<UserResults>(userState);
            }
        }
    }
}