using NCalc;

namespace Quanta;

public class MathTextBox : TextBox
{
    public void Evaluate()
    {
        Text = Text.Replace("÷", "/");
        Text = Text.Replace("x", "*");

        try
        {
            string expression = Text;
            Text = new Expression(expression).Evaluate().ToString();
            Parent.GetNode<Label>("LastExpressionLabel").Text = expression + "=";
        }
        catch (Exception ex)
        {
        }
    }
}