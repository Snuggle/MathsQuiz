<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SimpleWebMathsQuiz.Default" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <title>HELO</title>    
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" integrity="sha384-JcKb8q3iqJ61gNV9KGb8thSsNjpSL0n8PARn9HuZOnIxN0hoP+VmmDGMN5t9UJ0Z" crossorigin="anonymous">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
</head>
<body onload="LoadTimer();">
    <div class="container">
      <div class="py-5 text-center">
            <h1>Simple Maths Quiz</h1>
            <form id="form1" method="post" runat="server">   
                <h3 id="answerText" runat="server"></h3>
                <h5 id="QuestionsRemaining" runat="server"></h5>
                <hr />
                <h4 id="question" runat="server"></h4>
                <input type="text" class="form-control" name="text" value="" />

                <input type="hidden" name="operators" id="operators" runat="server" />
                <input type="hidden" name="secondNumber" id="secondNumber" runat="server" />
                <input type="hidden" name="firstNumber" id="firstNumber" runat="server" />
                <input type="hidden" name="UserAnswers" id="UserAnswers" value='' runat="server" />






                <br/>
                <button class="btn btn-primary btn-lg btn-block" type="submit" name="submit" value="I've answered the question!">
                    <div id="divRemainingTime"  runat="server"></div>
                </button>
                <hr />
                <code><p id="stateDebug" runat="server"></p></code>
            </form>
          </div>
        </div>
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js" integrity="sha384-9/reFTGAW83EW2RDu2S0VKaIzap3H66lZH81PoYlFhbGU+6BZp6G7niu735Sk7lN" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js" integrity="sha384-B4gt1jrGC7Jh4AgTPSdUtOBvfO8shuf57BaghqFfPlYxofvL8/KUEfYiJOMMV+rV" crossorigin="anonymous"></script>
    <script src="Timer.js"></script> 
</body>
</html>
