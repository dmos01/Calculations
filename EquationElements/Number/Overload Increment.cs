namespace EquationElements
{
    public partial class Number
    {
        /// <summary>
        ///     Uses + 1 so AsDecimal and AsDouble can be kept get only. Also, because objects are stored by reference, changing
        ///     them would change the original.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Number operator ++(Number a) =>
            a.IsDecimal ? new Number(a.AsDecimal + 1) : new Number(a.AsDouble + 1);

        /// <summary>
        ///     Uses -1 so AsDecimal and AsDouble can be kept get only. Also, because objects are stored by reference, changing
        ///     them would change the original.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Number operator --(Number a) =>
            a.IsDecimal ? new Number(a.AsDecimal - 1) : new Number(a.AsDouble - 1);
    }
}