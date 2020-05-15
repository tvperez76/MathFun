using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathFun.Api.Models
{
    /// <summary>
    /// Represents a user's REST math equation evaluation request.
    /// </summary>
    public class ExpressionRequest
    {
        /// <summary>
        /// Gets or sets the client's math expression.
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// Gets or sets the entries that contains the values for each variable.
        /// </summary>
        public List<VariableEntry> VariableEntries { get; set; }
    }
}
