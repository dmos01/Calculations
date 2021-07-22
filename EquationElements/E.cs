using System;

namespace EquationElements
{
    public class E : BaseElement
    {
        public const int MaxAbsoluteExponent = 307;

        public override string ToString() => ElementsResources.EulersSymbolUpperCase;

        /// <summary>
        ///     Throws an exception if not an integer, or outside the bounds of -MaxAbsoluteExponent and MaxAbsoluteExponent.
        /// </summary>
        /// <param name="power">An integer.</param>
        public static void TestPower(Number power)
        {
            if (power is null)
                throw new ArgumentNullException(null,
                    ElementsExceptionMessages.StringIsNullEmptyOrOnlySpacesBeforeParameter + nameof(power) +
                    ElementsExceptionMessages.StringIsNullEmptyOrOnlySpacesAfterParameter);

            if (!int.TryParse(power.ToString(), out int asInteger))
                throw new ArgumentOutOfRangeException(null,
                    ElementsExceptionMessages.ExponentIsNotIntegerBeforeParameter +
                    power +
                    ElementsExceptionMessages.ExponentIsNotIntegerAfterParameter);

            if (Math.Abs(asInteger) > MaxAbsoluteExponent)
                throw new ArgumentOutOfRangeException(null,
                    ElementsExceptionMessages.ExponentTooLargeOrSmallBeforeParameter +
                    power +
                    ElementsExceptionMessages.ExponentTooLargeOrSmallAfterParameter);
        }
    }
}