using System;
using MathFun.Api.Models;
using MathFun.ExpressionBuilder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace MathFun.Api.Controllers
{
    /// <summary>
    /// Operations concerning Math Expressions.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class MathExpressionsController : ControllerBase
    {
        private readonly ILogger<MathExpressionsController> logger;

        public MathExpressionsController(ILogger<MathExpressionsController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Fulfills student requests to evaluate a custom expression
        /// </summary>
        /// <param name="request">The request that contains the expression and variables</param>
        /// <returns>The evaluated expression and the computed value.</returns>
        [HttpPost("Evaluate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExpressionResponse))]
        [SwaggerOperation("Fulfills student requests to evaluate a custom expression")]
        public IActionResult EvaluateExpression([FromBody, SwaggerRequestBody("The request that contains the expression and variables")] ExpressionRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                this.logger.LogError("This model doesn't look right to me!");
                throw new ArgumentException("Your request body wasn't proper! ");
            }

            this.logger.LogInformation($"All looks good.  Let's start parsing: {request.Expression}.");
            MathExpressionBuilder builder = new MathExpressionBuilder(request.Expression);
            IMathExpression parsedExpression = builder.GenerateExpression();
            this.logger.LogDebug("Yup.  Looks like a legit expression.");
            foreach (var variable in request.VariableEntries)
            {
                builder.SetVariable(variable.Name, variable.Value.ToString());
                this.logger.LogDebug("Setting variable {0} with {1}", variable.Name, variable.Value);
            }

            ExpressionResponse response = new ExpressionResponse
            {
                AdjustedExpression = parsedExpression.ToString(),
                Expression = request.Expression,
                VariableEntries = request.VariableEntries,
                ValueGenerated = parsedExpression.EvaluateExpression()
            };

            return this.Ok(response);
        }
    }
}
