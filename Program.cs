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

        static string ListToString(List<bool> someList)
        {
            return String.Join(",", someList);
        }

        static void Main(string[] args)
        {
            List<char> operators = new List<char>() { '+', '-', '*', '/' };
            int maxRandNumber = 100;

            Program P = new Program();
            int numberOfQuestions = P.HowManyQuestions();

            List<bool> results = new List<bool>();
            for (int questionNum = 1; questionNum <= numberOfQuestions; questionNum++)
            {
                char op = operators[P.rnd.Next(0, operators.Count())];
                int firstNum = P.rnd.Next(0, maxRandNumber);
                int secondNum = P.rnd.Next(0, maxRandNumber);
                results.Add(P.AskQuestion(op, firstNum, secondNum));
            }

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
                bool correct = CheckAnswer(op, firstNum, secondNum, answer);
                if (correct)
                {
                    Console.WriteLine("You are correct!");
                }
                else
                {
                    Console.WriteLine("Not correct!");
                }
                return correct;
            }
            else
            {
                // Input was not valid, try again.
                Console.WriteLine("Please type in an integer and press ENTER. Repeating question...");
                return AskQuestion(op, firstNum, secondNum);
            }
        }

        public bool CheckAnswer(char op, int firstNum, int secondNum, int answer)
        {
            switch (op)
            {
                case '+':
                    return (firstNum + secondNum) == answer;
                case '-':
                    return (firstNum - secondNum) == answer;
                case '*':
                    return (firstNum * secondNum) == answer;
                case '/':
                    return (firstNum / secondNum) == answer;
                default:
                    return false;
            }
        }
    }
}
