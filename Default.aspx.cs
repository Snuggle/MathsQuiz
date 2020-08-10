using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MathsQuiz;

namespace SimpleWebMathsQuiz
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Program test = new MathsQuiz.Program();

            (string questionText, int firstNum, int secondNum, char op) = test.Web_AskQuestion(test, 10);
            question.InnerText = questionText;
            firstNumber.Attributes["value"] = firstNum.ToString();
            secondNumber.Attributes["value"] = secondNum.ToString();
            operators.Attributes["value"] = op.ToString();



            if (IsPostBack)
            {
                question.InnerText = $"The answer you had provided to '{Request.Form["firstNumber"]} {Request.Form["operators"]} {Request.Form["secondNumber"]}' was: {Request.Form["text"]}";
            }
        }


        
    }
}