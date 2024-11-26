using System;

namespace Miku.State
{
    public class FuncPredicate : IPredicate
    {
        readonly Func<bool> predicateFunc;

        public FuncPredicate(Func<bool> predicateFunc)
        {
            this.predicateFunc = predicateFunc;
        }
        public bool Evaluate() => predicateFunc.Invoke();
    }
}
