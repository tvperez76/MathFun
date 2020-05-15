using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathFun.Api.Models
{
    /// <summary>
    /// The REST contract to declare a variable's value.
    /// </summary>
    public class VariableEntry
    {
        /// <summary>
        /// Gets or sets the name of the variable.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the declared variable.
        /// </summary>
        public decimal Value { get; set; }
    }
}
