using System;

namespace grid.utils
{
    public class UnitAction
    {
        public Func<bool> predicate;

        public Action<Action> action;

        public UnitAction(
            Func<bool> predicate,
            Action<Action> action 
        )
        {
            this.action = action;
            this.predicate = predicate;
        }
    }
}