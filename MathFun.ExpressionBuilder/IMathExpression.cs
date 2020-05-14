using System;

namespace MathFun.ExpressionBuilder
{
    /// <summary>
    /// The contract/interface for all Math Expressions.
    /// </summary>
    public interface IMathExpression
    {
        /// <summary>
        /// Evaluates the expression based on its parameters to produce a output in decimal form.
        /// </summary>
        /// <exception cref="ArithmeticException">Thrown if any of expression is incompletely populated.</exception>
        /// <returns>The product of the expression by evaluated.  It will be returned in decimal form, at this datatype is more appropriate for its flexibility and accuracy.</returns>
        decimal EvaluateExpression();

        /// <summary>
        /// Generates a human readable rendering of a mathematic expression.
        /// </summary>
        /// <returns>The math expression string rendition.</returns>
        string ToString();
    }
}
