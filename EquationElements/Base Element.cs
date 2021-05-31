namespace EquationElements
{
    public abstract class BaseElement
    {
        public string Type => GetType().Name;

        //Forces sub-classes to override.
        public abstract override string ToString();
    }
}