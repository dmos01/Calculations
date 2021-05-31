namespace EquationElements
{
    public partial class Number
    {
        /// <summary>
        ///     Using + 1 so AsDecimal and AsDouble can be kept get only.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Number operator ++(Number a) =>
            a.IsDecimal ? new Number(a.AsDecimal + 1) : new Number(a.AsDouble + 1);

        /// <summary>
        ///     Using -1 so AsDecimal and AsDouble can be kept get only.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Number operator --(Number a) =>
            a.IsDecimal ? new Number(a.AsDecimal - 1) : new Number(a.AsDouble - 1);
    }
}