using System;
using EquationElements;
using static EquationElements.Utils;

namespace Calculations
{
    partial class Controller
    {
        public partial class Constant : IComparable
        {
            public string Name { get; }
            public string Value { get; private set; }
            public string? Unit { get; private set; }
            public string? Description { get; private set; }
            public string ValueWithoutSpaces { get; private set; }
            public string? UnitWithoutSpaces { get; private set; }
            public string? DescriptionWithoutSpaces { get; private set; }

            public Constant(string name, string value, string? unit = null, string? description = null)
            {
                ThrowExceptionIfNullEmptyOrOnlySpaces(name, nameof(name));
                ThrowExceptionIfNullEmptyOrOnlySpaces(value, nameof(value));

                Name = name;
                Value = value;
                Unit = unit;
                Description = description;

                ValueWithoutSpaces = RemoveSpaces(Value);
                UnitWithoutSpaces = RemoveSpaces(Unit);
                DescriptionWithoutSpaces = RemoveSpaces(Description);
            }

            /// <summary>
            ///     Changes the non-key properties.
            /// </summary>
            /// <param name="newValue"></param>
            /// <param name="newUnit"></param>
            /// <param name="newDescription"></param>
            public void Overwrite(string newValue, string? newUnit = null, string? newDescription = null)
            {
                ThrowExceptionIfNullEmptyOrOnlySpaces(newValue, nameof(newValue));

                Value = newValue;
                Unit = newUnit;
                Description = newDescription;

                ValueWithoutSpaces = RemoveSpaces(Value);
                UnitWithoutSpaces = RemoveSpaces(Unit);
                DescriptionWithoutSpaces = RemoveSpaces(Description);
            }

            /// <summary>
            ///     Tests if any of this Constant's fields contains the search text.
            /// </summary>
            /// <returns>True if the Constant matches the search text. Otherwise, false.</returns>
            public bool Matches(string searchTextWithoutSpaces)
            {
                if (ValueWithoutSpaces.Contains(searchTextWithoutSpaces, StringComparison.CurrentCultureIgnoreCase))
                    return true;

                if (UnitWithoutSpaces != null &&
                    UnitWithoutSpaces.Contains(searchTextWithoutSpaces, StringComparison.CurrentCultureIgnoreCase))
                    return true;

                if (DescriptionWithoutSpaces != null &&
                    DescriptionWithoutSpaces.Contains(searchTextWithoutSpaces,
                        StringComparison.CurrentCultureIgnoreCase))
                    return true;

                return false;
            }

            /// <summary>
            ///     Returns 'name = value unit' as a string.
            /// </summary>
            /// <returns></returns>
            public override string ToString() =>
                Name + " " + OperatorRepresentations.EqualsSymbol + " " + Value + " " + Unit;
        }
    }
}