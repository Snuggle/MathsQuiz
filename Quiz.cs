using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleWebMathsQuiz
{
    public class Quiz
    {
        private readonly Random __Random = new Random();
        private readonly Dictionary<char, Func<double, double, double>> __Calculator =
            new Dictionary<char, Func<double, double, double>>()
            {
                { '+', (oneNum, twoNum) => oneNum + twoNum },
                { '-', (oneNum, twoNum) => oneNum - twoNum },
                { '*', (oneNum, twoNum) => oneNum * twoNum },
                { '/', (oneNum, twoNum) => oneNum / twoNum }
                /*{ '√', (oneNum, twoNum) => (int) Math.Sqrt(oneNum) },
                { '^', (oneNum, twoNum) => (int) Math.Pow(oneNum, twoNum) }*/
            };

        public double GetCorrectAnswer(char op, int firstNum, int secondNum)
        {
            return __Calculator[op](firstNum, secondNum);
        }

        public Question AskQuestion(int maxRandNumber)
        {
            int index = __Random.Next(__Calculator.Count);
            char op = __Calculator.Keys.ElementAt(index);

            int firstNum = __Random.Next(1, maxRandNumber);
            int secondNum = __Random.Next(1, maxRandNumber);

            if (op.Equals('/'))
            {
                firstNum *= secondNum;
            }

            string questionText = $"What is the answer to {firstNum} {op} {secondNum}? "; // Much nicer, string interpolation! Closes #12. 

            return new Question
            {
                QuestionText = questionText,
                FirstNumber = firstNum,
                SecondNumber = secondNum,
                Operator = op
            };
        }

        public string IsTheUserCorrect(UserResults userState, int correctAnswer)
        {
            if (userState.Previous.PrevAnswerText.Equals(correctAnswer))
            {
                return $"YAY, CORRECT! ✅🎉";
            }
            return $"Wrong. '{userState.Previous.PrevFirstNumber} {userState.Previous.PrevOperator} {userState.Previous.PrevSecondNumber}' is {correctAnswer}! ❌";
        }
    }

}