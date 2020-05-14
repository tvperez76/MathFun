using MathFun.ExpressionBuilder.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MathFun.ExpressionBuilder
{
    public class MathExpressionBuilder
    {
        private Regex tokenizer = new Regex("(?<Variable>[a-zA-Z])|(?<Decimal>[0-9]+\\.?[0-9]*)|(?<Operator>[+\\-\\/*^])|(?<OpenGroup>\\()|(?<CloseGroup>\\))");
        private IDictionary<string, UniaryExpression> variables;
        private Stack<IMathExpression> expressionStack;
        private string originalExpression;

        public MathExpressionBuilder(string expression)
        {
            this.originalExpression = expression;
            this.expressionStack = new Stack<IMathExpression>();
            this.variables = new Dictionary<string, UniaryExpression>();
        }

        public bool ContainsVariables { get { return this.variables.Count > 0; } }

        public IMathExpression GenerateExpression()
        {
            MatchCollection matches = this.tokenizer.Matches(this.originalExpression);
            if (matches.Count == 0)
            {
                throw new InvalidOperationException("Unable to parse expression");
            }

            foreach (Match tokenMatch in matches)
            {
                var expression = this.ProcessOpenGroup(tokenMatch);
                this.expressionStack.Push(expression);
            }

            return this.Reduce();
        }

        public void SetVariable(string variableName, string value)
        {
            if (string.IsNullOrEmpty(variableName))
            {
                throw new ArgumentNullException(nameof(variableName));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (variables.TryGetValue(variableName.Trim(), out UniaryExpression variableExpression))
            {
                variableExpression.ExpressionValue = Convert.ToDecimal(value.Trim());
            }
        }

        private IMathExpression ProcessOpenGroup(Match tokenMatch)
        {
            Group token = tokenMatch.Groups["OpenGroup"];
            if (string.IsNullOrEmpty(token?.Value))
            {
                return this.ProcessCloseGroup(tokenMatch);
            }

            return new GroupedExpression();
        }

        private IMathExpression ProcessCloseGroup(Match tokenMatch)
        {
            Group token = tokenMatch.Groups["CloseGroup"];
            if (string.IsNullOrEmpty(token?.Value))
            {
                return this.ProcessOperator(tokenMatch);
            }

            IMathExpression innerExpression = this.Reduce();
            GroupedExpression groupedExpression = this.expressionStack.Pop() as GroupedExpression;
            if (groupedExpression != null)
            {
                groupedExpression.InnerExpression = innerExpression;
            }

            return groupedExpression;
        }

        private IMathExpression ProcessOperator(Match tokenMatch)
        {
            Group token = tokenMatch.Groups["Operator"];
            if (string.IsNullOrEmpty(token?.Value))
            {
                return this.ProcessUniary(tokenMatch);
            }

            return new BinaryExpression(token.Value);
        }

        private IMathExpression ProcessUniary(Match tokenMatch)
        {
            UniaryExpression variableOrLiteral = null;
            Group token = tokenMatch.Groups["Variable"];
            bool isNotVariable = false;
            if ((isNotVariable = string.IsNullOrEmpty(token?.Value)) || (token != null && !this.variables.TryGetValue(token.Value, out variableOrLiteral)))
            {
                variableOrLiteral = new UniaryExpression(tokenMatch.Value);
                if (!isNotVariable)
                {
                    this.variables.Add(token.Value, variableOrLiteral);
                }
            }

            return variableOrLiteral;
        }

        private List<IMathExpression> GenerateScopeExpressions()
        {
            List<IMathExpression> scopedMathExpressions = new List<IMathExpression>();
            bool proceedDownTheStack = false;
            if (this.expressionStack.Count > 0)
            {
                do
                {
                    GroupedExpression groupedExpression = this.expressionStack.Peek() as GroupedExpression;
                    if (groupedExpression == null || groupedExpression.InnerExpression != null)
                    {
                        scopedMathExpressions.Insert(0, this.expressionStack.Pop());
                        proceedDownTheStack = this.expressionStack.Count > 0;
                    }
                    else
                    {
                        proceedDownTheStack = false;
                    }

                } while (proceedDownTheStack);
            }

            return scopedMathExpressions;
        }

        private IMathExpression Reduce()
        {
            List<IMathExpression> scopedItems = this.GenerateScopeExpressions();

            List<IMathExpression> exponentReduction = this.ReduceExponents(scopedItems, OperationType.Exponential);

            List<IMathExpression> multiplicationAndDivisionReduction = this.ReduceExponents(exponentReduction, OperationType.Multiplication, OperationType.Division);

            List<IMathExpression> additionAndSubtractionReduction = this.ReduceExponents(multiplicationAndDivisionReduction, OperationType.Addition, OperationType.Subtraction);

            if (additionAndSubtractionReduction.Count > 1 || additionAndSubtractionReduction.Count == 0)
            {
                throw new SyntaxErrorException("The syntax was incorrect.  Please adjust your expression.");
            }

            return additionAndSubtractionReduction[0];
        }

        private List<IMathExpression> ReduceExponents(List<IMathExpression> expressions, params OperationType[] desiredOperationTypes)
        {
            List<IMathExpression> reducedExpressions = new List<IMathExpression>();
            bool requiresEvaluation = false;
            foreach (IMathExpression expression in expressions)
            {
                if (!requiresEvaluation)
                {
                    BinaryExpression binaryExpression = expression as BinaryExpression;
                    if (binaryExpression == null || desiredOperationTypes.All(x => x != binaryExpression.Operator))
                    {
                        reducedExpressions.Add(expression);
                        continue; // Skip the rest and move to the next iteration.
                    }

                    requiresEvaluation = true;
                    int index = reducedExpressions.Count - 1;
                    IMathExpression leftExpression = reducedExpressions[index];
                    reducedExpressions.RemoveAt(index);
                    binaryExpression.LeftExpression = leftExpression;
                    reducedExpressions.Add(binaryExpression);
                }
                else
                {
                    requiresEvaluation = false;
                    int index = reducedExpressions.Count - 1;
                    BinaryExpression rightExpression = reducedExpressions[index] as BinaryExpression;
                    rightExpression.RightExpression = expression;
                }
            }

            return reducedExpressions;
        }
    }
}
