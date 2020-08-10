<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SimpleWebMathsQuiz.Default" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <title>HELO</title>    
</head>
<body>
    <h1>Simple Maths Quiz</h1>
    <form id="form1" method="post" runat="server">   
        <h3 id="answerText" runat="server"></h3>
        <ul><li><p id="question" runat="server"></p></li></ul>
        <input type="text" name="text" value="" />

        <input type="hidden" name="operators" id="operators" runat="server" />

        <input type="hidden" name="secondNumber" id="secondNumber" runat="server" />

        <input type="hidden" name="firstNumber" id="firstNumber" runat="server" />

        <br/>
        <input type="submit" name="submit" value="Submit" />
    </form>
</body>
</html>
