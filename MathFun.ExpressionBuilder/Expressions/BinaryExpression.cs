using System;
using System.Collections.Generic;
using System.Text;

namespace MathFun.ExpressionBuilder.Expressions
{
    public class BinaryExpression : IMathExpression
    {
        private OperationType operatorType;
        private Func<decimal> operation;
        private string operationSymbol;

        public BinaryExpression()
        {
            this.operatorType = OperationType.Addition;
            this.operation = this.Add;
        }

        public BinaryExpression(string operationType)
        {
            switch (operationType)
            {
                case "+":
                    this.operation = this.Add;
                    this.operationSymbol = "+";
                    this.operatorType = OperationType.Addition;
                    break;
                case "-":
                    this.operation = this.Subtract;
                    this.operationSymbol = "-";
                    this.operatorType = OperationType.Subtraction;
                    break;
                case "*":
                    this.operation = this.Multiply;
                    this.operationSymbol = "*";
                    this.operatorType = OperationType.Multiplication;
                    break;
                case "/":
                    this.operation = this.Division;
                    this.operationSymbol = "/";
                    this.operatorType = OperationType.Division;
                    break;
                case "^":
                    this.operation = this.Exponent;
                    this.operationSymbol = "^";
                    this.operatorType = OperationType.Exponential;
                    break;
                default:
                    throw new ArgumentException($"An invalid operator was used! \"{operationType}\" is not supported!");
            }
        }

        /// <summary>
        /// Gets or sets the left-sided expression.
        /// </summary>
        public IMathExpression LeftExpression { get; set; }

        /// <summary>
        /// Gets or sets the operator type. 
        /// </summary>
        /// <remarks>This property also sets the operation method to be used for evaluations.</remarks>
        public OperationType Operator
        {
            get
            {
                return this.operatorType;
            }

            set
            {
                this.operatorType = value;
                switch (value)
                {
                    case OperationType.Addition:
                        this.operation = this.Add;
                        this.operationSymbol = "+";
                        break;
                    case OperationType.Subtraction:
                        this.operation = this.Subtract;
                        this.operationSymbol = "-";
                        break;
                    case OperationType.Multiplication:
                        this.operation = this.Multiply;
                        this.operationSymbol = "*";
                        break;
                    case OperationType.Division:
                        this.operation = this.Division;
                        this.operationSymbol = "/";
                        break;
                    case OperationType.Exponential:
                        this.operation = this.Exponent;
                        this.operationSymbol = "^";
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the right-sided expression.
        /// </summary>
        public IMathExpression RightExpression { get; set; }

        /// <summary>
        /// Evaluates each of the sides of the expression and then applies the mathematical operation to their evaluated values.
        /// </summary>
        /// <returns>The result of the application of the mathematical operation of the evaluated expressions.</returns>
        // <exception cref="ArgumentNullException">If the any of the expressions are null.</exception>
        public decimal EvaluateExpression()
        {
            if (this.LeftExpression == null)
            {
                throw new ArgumentNullException(nameof(this.LeftExpression), $"Property {nameof(this.LeftExpression)} wasn't provided.");
            }

            if (this.RightExpression == null)
            {
                throw new ArgumentNullException(nameof(this.RightExpression), $"Property {nameof(this.RightExpression)} wasn't provided.");
            }

            return this.operation();
        }

        /// <summary>
        /// Generates a human readable rendition of each expression's string value delimited by it's operation.
        /// </summary>
        /// <returns>The binary expression in human readable form.</returns>
        // <exception cref="ArgumentNullException">If the any of the expressions are null.</exception>
        public override string ToString()
        {
            if (this.LeftExpression == null)
            {
                throw new ArgumentNullException(nameof(this.LeftExpression), $"Property {nameof(this.LeftExpression)} wasn't provided.");
            }

            if (this.RightExpression == null)
            {
                throw new ArgumentNullException(nameof(this.RightExpression), $"Property {nameof(this.RightExpression)} wasn't provided.");
            }

            return $"{this.LeftExpression.ToString()} {this.operationSymbol} {this.RightExpression.ToString()}";
        }

        /// <summary>
        /// This performs the actual addition operation.
        /// </summary>
        /// <returns>The result of an addition of the two evaluated expressions.</returns>
        private decimal Add()
        {
            return LeftExpression.EvaluateExpression() + RightExpression.EvaluateExpression();
        }

        /// <summary>
        /// This performs the actual subtraction operation.
        /// </summary>
        /// <returns>The result of an subtraction of the two evaluated expressions.</returns>
        private decimal Subtract()
        {
            return LeftExpression.EvaluateExpression() - RightExpression.EvaluateExpression();
        }

        /// <summary>
        /// This performs the actual multiplication operation.
        /// </summary>
        /// <returns>The result of an multiplication of the two evaluated expressions.</returns>
        private decimal Multiply()
        {
            return LeftExpression.EvaluateExpression() * RightExpression.EvaluateExpression();
        }

        /// <summary>
        /// This performs the actual division operation.
        /// </summary>
        /// <returns>The result of an division of the two evaluated expressions.</returns>
        private decimal Division()
        {
            return LeftExpression.EvaluateExpression() / RightExpression.EvaluateExpression();
        }

        /// <summary>
        /// This performs the actual exponent operation.
        /// </summary>
        /// <returns>The result of an exponent of the two evaluated expressions.</returns>
        private decimal Exponent()
        {
            return (decimal)Math.Pow((double)LeftExpression.EvaluateExpression(), (double)RightExpression.EvaluateExpression());
        }
    }
}
