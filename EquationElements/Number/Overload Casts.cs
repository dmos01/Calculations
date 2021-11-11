namespace EquationElements
{
    partial class Number
    {      
        //Implicit casting enables Number n = 0;

        /* Implicit casts (no acknowledgement of the conversion by the programmer required) should be used only when no exceptions can be thrown in the conversion, such as when creating a Number from an int64 or decimal. Giving more weight to this, both will have "full functionality" (AsDecimal and AsDouble can both be used).*/
        /* Casts should be used when the object is really of type x but stored as a different type; otherwise, conversion should be used.*/


        /// <summary>
        /// Implicitly convert an int64 into a number. Stores both the integer (as a decimal) and its double representation. Both can be used.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Number(long value) => new Number(value);

        /// <summary>
        /// Implicitly convert a decimal into a number. Stores both the decimal and its double representation. Both can be used.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Number(decimal value) => new Number(value);
    }
}