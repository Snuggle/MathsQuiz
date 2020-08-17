using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleWebMathsQuiz
{
    public class UserResults
    {
        public List<int> UsersAnswers { get; set; } = new List<int>();
        public List<bool> UsersResults { get; set; } = new List<bool>();
        public int? HowManyQuestions { get; set; }
        public PreviousValues Previous { get; set; } = new PreviousValues();
    }
}