using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleWebMathsQuiz
{
    public class Question
    {
        public string QuestionText { get; set; }
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }
        public char Operator { get; set; }
    }
}