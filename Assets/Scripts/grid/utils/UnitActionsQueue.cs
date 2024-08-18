using System.Collections.Generic;

namespace grid.utils
{
    public class UnitActionsQueue
    {
        public int size = 1;
        private Queue<UnitAction> queue = new();
        private UnitAction currentUnitAction;

        public UnitActionsQueue Add(UnitAction unitAction)
        {
            queue.Enqueue(unitAction);
            while (queue.Count > size)
                queue.Dequeue();
            return this;
        }
        
        public UnitActionsQueue Next()
        {
            if (currentUnitAction != null || queue.Count == 0) return this;

            currentUnitAction = queue.Dequeue();
            if (currentUnitAction.predicate())
                currentUnitAction.action.Invoke(OnComplete);
            else
                OnComplete();
            return this;
        }

        private void OnComplete()
        {
            currentUnitAction = null;
            Next();
        }
    }
}