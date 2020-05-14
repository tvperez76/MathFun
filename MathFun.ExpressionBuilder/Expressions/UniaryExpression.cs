using System;
using System.Collections.Generic;
using System.Text;

namespace MathFun.ExpressionBuilder.Expressions
{
    /// <summary>
    /// This represents a variable or numeric expression.
    /// </summary>
    public class UniaryExpression : IMathExpression
    {

        /// <summary>
        /// Constructor that takes in the token value, which determines if it is a variable or a literal.
        /// </summary>
        /// <param name="variableNameOrValue"></param>
        public UniaryExpression(string variableNameOrValue)
        {
            if (string.IsNullOrEmpty(variableNameOrValue))
            {
                return;
            }

            if (char.IsLetter(variableNameOrValue[0]))
            {
                this.ExpressionName = variableNameOrValue;
            }
            else
            {
                this.ExpressionValue = Convert.ToDecimal(variableNameOrValue);
            }
        }


        /// <summary>
        /// Gets a value indicating whether the expression is a literal (false) or variable (true).
        /// </summary>
        public bool IsVariable { get { return !string.IsNullOrEmpty(this.ExpressionName); } }

        /// <summary>
        /// Gets the variable name, if applicable.
        /// </summary>
        public string ExpressionName { get; private set; }

        /// <summary>
        /// Gets or sets the literal numeric value.
        /// </summary>
        public decimal? ExpressionValue { get; set; }

        /// <summary>
        /// Returns the literal numeric value.
        /// </summary>
        /// <returns>The evaluated value of the inner expression.</returns>
        /// <exception cref="ArgumentNullException">If the <see cref="ExpressionValue"/> is null.</exception>
        public decimal EvaluateExpression()
        {
            if (this.ExpressionValue == null)
            {
                throw new ArgumentNullException(nameof(this.ExpressionValue));
            }

            return this.ExpressionValue.Value;
        }

        /// <summary>
        /// Generates a human readable rendition of variable name or numeric literal string value.
        /// </summary>
        /// <returns>The string rendition of the variable or number.</returns>
        // <exception cref="ArgumentNullException">If the any of the expressions are null.</exception>
        public override string ToString()
        {
            if (this.ExpressionValue == null & !this.IsVariable)
            {
                throw new ArgumentNullException(nameof(this.ExpressionValue));
            }

            return this.ExpressionValue != null ? this.ExpressionValue.ToString() : this.ExpressionName;
        }
    }
}
