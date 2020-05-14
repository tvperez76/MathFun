using MathFun.ExpressionBuilder;
using System;

namespace MathFun.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var requests in args)
            {
                string[] commandItems = requests.Split(';');
                if (commandItems.Length == 0)
                {
                    continue;
                }

                string expression = commandItems[0];
                MathFun.ExpressionBuilder.MathExpressionBuilder mathExpressionBuilder = new ExpressionBuilder.MathExpressionBuilder(expression);

                IMathExpression mathExpression = mathExpressionBuilder.GenerateExpression();
                Console.WriteLine($"The expression parsed was {mathExpression.ToString()}");
                if (mathExpressionBuilder.ContainsVariables)
                {
                    foreach (var variableCommand in commandItems)
                    {
                        string[] commandSubparts = variableCommand.Split("="); 
                        if (commandSubparts.Length != 2)
                        {
                            continue;
                        }

                        string variableName = commandSubparts[0];
                        string value = commandSubparts[1];

                        mathExpressionBuilder.SetVariable(variableName, value);
                        Console.WriteLine($"{variableName}={value}");
                    }
                }

                Console.WriteLine($"The expression evaluated {mathExpression.EvaluateExpression()}");
            }
        }
    }
}
