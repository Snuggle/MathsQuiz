using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleWebMathsQuiz
{
    public class UserResults
    {
            public List<int> UsersAnswers { get; set; }
            public List<bool> UsersResults { get; set; }
            public int? HowManyQuestions { get; set; }
            public PreviousValues Previous { get; set; }
       
    }
}