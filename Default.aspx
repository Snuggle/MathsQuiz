<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SimpleWebMathsQuiz.Default" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <title>HELO</title>    
</head>
<body onload="LoadTimer();">
    <h1>Simple Maths Quiz</h1>
    <form id="form1" method="post" runat="server">   
        <p style="display: inline">(Debug) Your user state: </p><p style="display: inline" id="stateDebug" runat="server"></p>

        <p id="answerText" runat="server"></p>
        <ul><li><p id="question" runat="server"></p></li></ul>
        <input type="text" name="text" value="" />

        <input type="hidden" name="operators" id="operators" runat="server" />
        <input type="hidden" name="secondNumber" id="secondNumber" runat="server" />
        <input type="hidden" name="firstNumber" id="firstNumber" runat="server" />
        <input type="hidden" name="UserAnswers" id="UserAnswers" value='' runat="server" />






        <br/>
        <input type="submit" name="submit" value="Submit" />
        <div id="divRemainingTime"  runat="server"></div>        
    </form>

    <script src="Timer.js"></script> 
</body>
</html>
