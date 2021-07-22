using System;

namespace EquationElements.Functions
{
    /// <summary>
    ///     Works with integers only.
    /// </summary>
    public class RandomFunction : TwoArgumentFunction
    {
        readonly Random rand;

        public RandomFunction() => rand = new Random();

        public override string ToString() => FunctionRepresentations.RandomShortWord;

        /// <summary>
        /// </summary>
        /// <param name="inclusiveMin">An integer.</param>
        /// <param name="inclusiveMax">An integer.</param>
        /// <returns></returns>
        protected override Number PerformOnAfterNullCheck(Number inclusiveMin, Number inclusiveMax)
        {
            if (int.TryParse(inclusiveMin.ToString(), out int minValue) == false)
                throw new ArgumentOutOfRangeException(null,
                    ElementsExceptionMessages.RandomWasNotAnIntegerBeforeParameter + inclusiveMin +
                    ElementsExceptionMessages.RandomWasNotAnIntegerAfterParameter);

            if (int.TryParse(inclusiveMax.ToString(), out int maxValue) == false)
                throw new ArgumentOutOfRangeException(null,
                    ElementsExceptionMessages.RandomWasNotAnIntegerBeforeParameter + inclusiveMax +
                    ElementsExceptionMessages.RandomWasNotAnIntegerAfterParameter
                );

            return new Number(rand.Next(minValue, maxValue + 1));
        }
    }
}