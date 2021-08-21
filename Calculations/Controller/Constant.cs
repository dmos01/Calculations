using System;
using EquationElements;
using static EquationElements.Utils;

namespace Calculations
{
    partial class Controller
    {
        public partial class Constant
        {
            public string Name { get; }

            private string fullValue;
            public string Value
            {
                get => fullValue;
                set
                {
                    ThrowExceptionIfNullEmptyOrOnlySpaces(value, nameof(Value));
                    fullValue = value;
                    ValueWithoutSpaces = RemoveSpaces(value);
                }
            }

            private string fullUnit;
            public string Unit
            {
                get => fullUnit;
                set
                {
                    if (value is null)
                    {
                        fullUnit = "";
                        UnitWithoutSpaces = "";
                    }
                    else
                    {
                        fullUnit = value;
                        UnitWithoutSpaces = RemoveSpaces(value);
                    }
                }
            }

            private string fullDescription;
            public string Description
            {
                get => fullDescription;
                set
                {
                    if (value is null)
                    {
                        fullDescription = "";
                        DescriptionWithoutSpaces = "";
                    }
                    else
                    {
                        fullDescription = value;
                        DescriptionWithoutSpaces = RemoveSpaces(value);
                    }
                }
            }

            public string NameWithoutSpaces { get; }
            public string ValueWithoutSpaces { get; private set; }
            public string UnitWithoutSpaces { get; private set; }
            public string DescriptionWithoutSpaces { get; private set; }

            public Constant(string name, string value)
            {
                ThrowExceptionIfNullEmptyOrOnlySpaces(name, nameof(name));
                Name = name;
                NameWithoutSpaces = RemoveSpaces(name);
                Value = value;
                Unit = "";
                Description = "";

            }

            public Constant(string name, string value, string unit, string description)
            {
                ThrowExceptionIfNullEmptyOrOnlySpaces(name, nameof(name));
                Name = name;
                NameWithoutSpaces = RemoveSpaces(name);
                Value = value;
                Unit = unit;
                Description = description;
            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="textWithoutSpaces">Should be without spaces.</param>
            /// <returns></returns>
            public bool Matches(string textWithoutSpaces)
            {
                if (NameWithoutSpaces.Contains(textWithoutSpaces, StringComparison.CurrentCultureIgnoreCase))
                    return true;

                if (ValueWithoutSpaces.Contains(textWithoutSpaces, StringComparison.CurrentCultureIgnoreCase))
                    return true;

                if (UnitWithoutSpaces is not null &&
                    UnitWithoutSpaces.Contains(textWithoutSpaces, StringComparison.CurrentCultureIgnoreCase))
                    return true;

                if (DescriptionWithoutSpaces is not null &&
                    DescriptionWithoutSpaces.Contains(textWithoutSpaces,
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