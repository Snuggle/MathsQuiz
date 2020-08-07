using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathsQuiz { 
    class Program
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

        static void Main(string[] args)
        {
            Program P = new Program(); // This is bad! See issue #11. :(

            int maxRandNumber = P.AskHowDifficultQuestions();
            List<bool> results = P.AskQuestions(P, P.AskHowManyQuestions(), maxRandNumber);
            
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

        public List<bool> AskQuestions(Program P, int numberOfQuestions, int maxRandNumber)
        {
            List<bool> results = new List<bool>();
            for (int questionNum = 1; questionNum <= numberOfQuestions; questionNum++)
            {
                int index = P.rnd.Next(P.potato.Count);
                char op = P.potato.Keys.ElementAt(index);

                int firstNum = P.rnd.Next(1, maxRandNumber);
                int secondNum = P.rnd.Next(1, maxRandNumber);

                double correctAnswer = GetCorrectAnswer(op, firstNum, secondNum);
                if (correctAnswer != Math.Truncate(correctAnswer))
                {
                    numberOfQuestions++;
                } else
                {
                     results.Add(P.AskQuestion(op, firstNum, secondNum));
                }

            }

            return results;
        }

        public bool AskQuestion(char op, int firstNum, int secondNum)
        {
            string questionText = $"What is the answer to {firstNum} {op} {secondNum}? "; // Much nicer, string interpolation! Closes #12. 
            Console.Write(questionText);

            bool answerIsInt = Int32.TryParse(Console.ReadLine(), out int answer);
            if (answerIsInt)
            {

                double correctAnswer = GetCorrectAnswer(op, firstNum, secondNum);
                if (correctAnswer==answer)
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
                return AskQuestion(op, firstNum, secondNum);
            }
        }

        public double GetCorrectAnswer(char op, int firstNum, int secondNum)
        {
            return potato[op](firstNum, secondNum);
        }
    }
}
