using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathsQuiz
{
    class Program
    {
        static string ListToString(List<bool> someList)
        {
            return String.Join(",", someList);
        }

        static void Main(string[] args)
        {
            List<bool> results = new List<bool>();
                     
            Program P = new Program();
            results.Add(P.AskQuestion('+', 2, 3));
            results.Add(P.AskQuestion('+', 3, 4));
            results.Add(P.AskQuestion('+', 4, 5));
            results.Add(P.AskQuestion('+', 5, 6));

            Func<bool, bool> isTrue = x => x;
            int correctCount = results.Count(isTrue);

            Console.WriteLine("\nYou have gotten '" + correctCount + "' out of '" + results.Count + "' answers correct!");
            Console.ReadLine(); // Don't quit instantly.
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
                if (correct) {
                    Console.WriteLine("You are correct!");
                } else
                {
                    Console.WriteLine("Not correct!");
                }
                return correct;
            }
            else
            {
                // Input was not valid, try again.
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
                default:
                    return false;
            }
        }
    }
}
