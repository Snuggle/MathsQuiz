using System;
using System.Collections.Generic;
using System.Linq;
using QuizLib;

namespace SimpleWebMathsQuiz
{
    class ConsoleQuiz
    {
        private readonly Quiz quiz = new Quiz();

        static void Main(string[] args)
        {
            ConsoleQuiz my_quiz = new ConsoleQuiz();
            my_quiz.Start();
        }

        public void Start()
        {
            int maxRandNumber = AskHowDifficultQuestions();
            int howManyQuestions = AskHowManyQuestions();
            List<bool> results = AskQuestions(howManyQuestions, maxRandNumber);

            Func<bool, bool> isTrue = result => result;
            int correctCount = results.Count(isTrue);

            Console.WriteLine("\nYou have gotten '" + correctCount + "' out of '" + results.Count + "' answers correct!");
            Console.ReadLine();
        }

        public int AskHowDifficultQuestions()
        {
            Console.Write("How difficult would you like the questions to be? [Easy, medium or hard?] ");
            switch (Console.ReadLine().ToLower())
            {
                case "easy":
                    return 5;
                case "medium":
                    return 10;
                case "hard":
                    return 30;
                default:
                    AskHowDifficultQuestions();
                    break;
            }
            return 5;
        }

        public int AskHowManyQuestions()
        {
            Console.Write("How many questions would you like to attempt? ");
            bool answerIsInt = Int32.TryParse(Console.ReadLine(), out int answer);
            if (answerIsInt)
            {
                return answer;
            }
            else
            {
                Console.WriteLine("Please type in an integer and press ENTER. Repeating question...");
                return AskHowManyQuestions();
            }

        }

        public List<bool> AskQuestions(int numberOfQuestions, int maxRandNumber)
        {
            List<bool> results = new List<bool>();
            for (int questionNum = 1; questionNum <= numberOfQuestions; questionNum++)
            {
                Question question = quiz.AskQuestion(maxRandNumber);

                double correctAnswer = quiz.GetCorrectAnswer(question);
                if (correctAnswer != Math.Truncate(correctAnswer))
                {
                    numberOfQuestions++;
                }
                else
                {
                    results.Add(AskQuestion(question));
                }

            }

            return results;
        }

        public bool AskQuestion(Question question)
        {
            string questionText = $"What is the answer to {question.FirstNumber} {question.Operator} {question.SecondNumber}? "; // Much nicer, string interpolation! Closes #12. 
            Console.Write(questionText);

            bool answerIsInt = Int32.TryParse(Console.ReadLine(), out int answer);
            if (answerIsInt)
            {

                double correctAnswer = quiz.GetCorrectAnswer(question);
                if (correctAnswer == answer)
                {
                    Console.WriteLine("You are correct!\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("Not correct! The answer was: " + correctAnswer + "\n");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Please type in an integer and press ENTER. Repeating question...");
                return AskQuestion(question);
            }
        }
    }
}