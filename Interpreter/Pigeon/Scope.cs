using Kostic017.Pigeon.Symbols;
using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    class Scope
    {
        readonly Dictionary<VariableSymbol, object> variables = new Dictionary<VariableSymbol, object>();

        internal void Declare(VariableSymbol variable, object value)
        {
            variables.Add(variable, value);
        }

        internal void Assign(VariableSymbol variable, object value)
        {
            variables[variable] = value;
        }

        internal object Evaluate(VariableSymbol variable)
        {
            return variables[variable];
        }
    }
}
