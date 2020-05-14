using System;
using System.Collections.Generic;
using System.Text;

namespace MathFun.ExpressionBuilder.Expressions
{
    /// <summary>
    /// Represents a grouped mathematic expression.  Another words, an expression that's encased by parenthesis. 
    /// </summary>
    public class GroupedExpression : IMathExpression
    {
        /// <summary>
        /// Gets or sets the expression within the parenthesis. 
        /// </summary>
        public IMathExpression InnerExpression { get; set; }

        /// <summary>
        /// Being that a parenthesis (grouped) expression is just a scoped version of the original expression, it should return the evaluated value of the inner expression.
        /// </summary>
        /// <returns>The evaluated value of the inner expression.</returns>
        /// <exception cref="ArgumentNullException">If the inner expression is null.</exception>
        public decimal EvaluateExpression()
        {
            if (this.InnerExpression == null)
            {
                throw new ArgumentNullException(nameof(this.InnerExpression), $"Property {nameof(this.InnerExpression)} wasn't provided.");
            }

            return this.InnerExpression.EvaluateExpression();
        }

        /// <summary>
        /// Generates a parenthesis encased string value of the inner expression.
        /// </summary>
        /// <returns>The parenthesis encased string value of the inner expression.</returns>
        /// <exception cref="ArgumentNullException">If the inner expression is null.</exception>
        public override string ToString()
        {
            if (this.InnerExpression == null)
            {
                throw new ArgumentNullException(nameof(this.InnerExpression), $"Property {nameof(this.InnerExpression)} wasn't provided.");
            }

            return $"({this.InnerExpression.ToString()})";
        }
    }
}
