using System.Collections.Generic;
using EquationElements;

namespace EquationBuilder
{
    //Must be made of references to the objects in the equation linked list.
    internal class EquationSegment
    {
        public LinkedListNode<BaseElement> Start { get; }
        public LinkedListNode<BaseElement> End { get; }

        public EquationSegment(LinkedListNode<BaseElement> start, LinkedListNode<BaseElement> end)
        {
            Start = start;
            End = end;
        }
    }
}