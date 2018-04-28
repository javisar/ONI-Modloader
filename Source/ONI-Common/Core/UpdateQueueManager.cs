namespace ONI_Common.Core
{
    using System;
    using System.Collections.Generic;

    public static class UpdateQueueManager
    {
        private static readonly Queue<EventHandler> _actionQueue = new Queue<EventHandler>();

        public static void EnqueueAction(EventHandler action)
        {
            _actionQueue.Enqueue(action);
        }

        public static void OnGameUpdate()
        {
            while (_actionQueue.Count > 0)
            {
                EventHandler action = _actionQueue.Dequeue();

                try
                {
                    action.Invoke(null, EventArgs.Empty);
                }
                catch
                {
                    // log
                }
            }
        }
    }
}