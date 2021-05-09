using Kostic017.Pigeon.Symbols;
using System.Linq;

namespace Kostic017.Pigeon.Operators
{
    class TernaryOperator
    {
        readonly PigeonType whenTrueType;
        readonly PigeonType whenFalseType;
        readonly PigeonType resultType;

        TernaryOperator(PigeonType whenTrueType, PigeonType whenFalseType, PigeonType resultType)
        {
            this.whenTrueType = whenTrueType;
            this.whenFalseType = whenFalseType;
            this.resultType = resultType;
        }

        internal static bool TryGetResType(PigeonType whenTrue, PigeonType whenFalse, out PigeonType pigeonType)
        {
            pigeonType = PigeonType.Error;
            TernaryOperator top;
            if ((top = combinations.FirstOrDefault(t => t.Supports(whenTrue, whenFalse))) != null)
            {
                pigeonType = top.resultType;
                return true;
            }
            return false;
        }


        private bool Supports(PigeonType whenTrue, PigeonType whenFalse)
        {
            return whenTrueType == whenTrue && whenFalseType == whenFalse;
        }

        private static readonly TernaryOperator[] combinations = new TernaryOperator[]
        {
            new TernaryOperator(PigeonType.Int, PigeonType.Int, PigeonType.Bool),
            new TernaryOperator(PigeonType.Int, PigeonType.Float, PigeonType.Bool),
            new TernaryOperator(PigeonType.Float, PigeonType.Int, PigeonType.Bool),
            new TernaryOperator(PigeonType.Float, PigeonType.Float, PigeonType.Bool),
            new TernaryOperator(PigeonType.Bool, PigeonType.Bool, PigeonType.Bool),
            new TernaryOperator(PigeonType.String, PigeonType.String, PigeonType.Bool),
        };
    }
}
