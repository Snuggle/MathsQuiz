using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathsQuiz
{
    class Program
    {
        Random rnd = new Random();

        Dictionary<char, Func<double, double, double>> potato = new Dictionary<char, Func<double, double, double>>()
            {
                /*{ '+', (x, y) => x + y },
                { '-', (x, y) => x - y },
                { '*', (x, y) => x * y },*/
                { '/', (x, y) => x / y },
                { '√', (x, y) => (int) Math.Pow(x, y) }
            };

        static void Main(string[] args)
        {
            // ENTRY POINT
            int maxRandNumber = 10;
            Program P = new Program();

            List<bool> results = P.AskManyQuestions(P, P.HowManyQuestions(), maxRandNumber);

            Func<bool, bool> isTrue = x => x;
            int correctCount = results.Count(isTrue);

            Console.WriteLine("\nYou have gotten '" + correctCount + "' out of '" + results.Count + "' answers correct!");
            Console.ReadLine(); // Don't quit instantly.
        }

        public int HowManyQuestions()
        {
            Console.Write("How many questions would you like to attempt? ");
            bool answerIsInt = Int32.TryParse(Console.ReadLine(), out int answer);
            if (answerIsInt)
            {
                return answer;
            }
            else
            {
                // Input was not valid, try again.
                Console.WriteLine("Please type in an integer and press ENTER. Repeating question...");
                return HowManyQuestions();
            }

        }

        public List<bool> AskManyQuestions(Program P, int numberOfQuestions, int maxRandNumber)
        {
            List<bool> results = new List<bool>();
            for (int questionNum = 1; questionNum <= numberOfQuestions; questionNum++)
            {
                int index = P.rnd.Next(P.potato.Count);
                char op = P.potato.Keys.ElementAt(index);

                int firstNum = P.rnd.Next(0, maxRandNumber);
                int secondNum = P.rnd.Next(0, maxRandNumber);

                double correctAnswer = CheckAnswer(op, firstNum, secondNum);

                // Ask question, add result to list.
                results.Add(P.AskQuestion(op, firstNum, secondNum));
            }

            return results;
        }

        public bool AskQuestion(char op, int firstNum, int secondNum)
        {
            // Ask question!
            String s = String.Format("What is the answer to {0} {1} {2}? ", firstNum, op, secondNum);
            Console.Write(s);

            // Receive answer, must be an integer.
            bool answerIsInt = Int32.TryParse(Console.ReadLine(), out int answer);
            if (answerIsInt)
            {
                // Check if answer is correct or not!
                double correctAnswer = CheckAnswer(op, firstNum, secondNum);
                if (correctAnswer==answer)
                {
                    Console.WriteLine("You are correct!");
                    return true;
                }
                else
                {
                    Console.WriteLine("Not correct! The answer was: " + correctAnswer);
                    return false;
                }
            }
            else
            {
                // Input was not valid, try again.
                Console.WriteLine("Please type in an integer and press ENTER. Repeating question...");
                return AskQuestion(op, firstNum, secondNum);
            }
        }

        public double CheckAnswer(char op, int firstNum, int secondNum)
        {

            //potato.Add('+', (x, y) => x + y);
            // Add other operators

            Console.Write("DEBUG: ");
            Console.WriteLine(potato[op](firstNum, secondNum));

            return potato[op](firstNum, secondNum);

        }
    }
}
