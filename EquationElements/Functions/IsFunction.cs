using System;
using System.Collections.Generic;

namespace EquationElements.Functions
{
    /// <summary>
    ///     Static class.
    /// </summary>
    public static class IsFunction
    {
        static readonly Dictionary<string, Type> stringToFunction =
            new Dictionary<string, Type>(StringComparer.CurrentCultureIgnoreCase)
            {
                {FunctionRepresentations.SinWord, typeof(SinFunction)},
                {FunctionRepresentations.CosWord, typeof(CosFunction)},
                {FunctionRepresentations.TanWord, typeof(TanFunction)},

                {FunctionRepresentations.ASinWord, typeof(ASinFunction)},
                {FunctionRepresentations.ACosWord, typeof(ACosFunction)},
                {FunctionRepresentations.ATanWord, typeof(ATanFunction)},

                {FunctionRepresentations.SinhWord, typeof(SinhFunction)},
                {FunctionRepresentations.CoshWord, typeof(CoshFunction)},
                {FunctionRepresentations.TanhWord, typeof(TanhFunction)},

                {FunctionRepresentations.NaturalLogShortWord, typeof(LnFunction)},
                {FunctionRepresentations.LogWord, typeof(LogFunction)},

                {FunctionRepresentations.RoundWord, typeof(RoundFunction)},
                {FunctionRepresentations.EvenRoundWord, typeof(EvenRoundFunction)},

                {FunctionRepresentations.FloorWord, typeof(FloorFunction)},
                {FunctionRepresentations.CeilingWord, typeof(CeilingFunction)},
                {FunctionRepresentations.CeilingShortWord, typeof(CeilingFunction)},
                {FunctionRepresentations.AbsoluteWord, typeof(AbsoluteFunction)},
                {FunctionRepresentations.AbsoluteShortWord, typeof(AbsoluteFunction)},
                {FunctionRepresentations.TruncateWord, typeof(TruncateFunction)},
                {FunctionRepresentations.TruncateShortWord, typeof(TruncateFunction)},

                {FunctionRepresentations.RandomWord, typeof(RandomFunction)},
                {FunctionRepresentations.RandomShortWord, typeof(RandomFunction)},

                {FunctionRepresentations.FactorialSymbol, typeof(FactorialSymbolFunction)},
                {FunctionRepresentations.FactorialWord, typeof(FactorialWordFunction)}
            };

        /// <summary>
        ///     Returns false if name is null or not a function; otherwise true.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="function">Null if method returns false.</param>
        /// <returns></returns>
        public static bool Run(string name, out BaseElement function)
        {
            if (stringToFunction.TryGetValue(name, out Type value))
                function = (BaseElement) Activator.CreateInstance(value);
            else
                function = null;

            return !(function is null);
        }
    }
}