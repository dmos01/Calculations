namespace EquationElements.Functions
{
    //Identifies all Functions because some need one argument and some two.
    public interface IFunction : IInvalidWhenLast, IMayPrecedeNegativeNumber
    {
    }

    public abstract class OneArgumentFunction : OneArgumentElement, IFunction
    {
    }

    public abstract class TwoArgumentFunction : TwoArgumentElement, IFunction
    {
    }
}