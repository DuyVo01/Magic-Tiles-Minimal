using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace PatternHelper
{
    /// <summary>
    /// Provides a centralized, reactive event system for game-wide communication.
    ///
    /// Flow:
    /// 1. Components subscribe to specific event types using GetStream<TEvent>().
    /// 2. Events are published using Publish<TEvent>(), which queues them for processing.
    /// 3. Each frame, the system processes all queued events:
    ///    - Retrieves events from their respective queues.
    ///    - Emits events through their type-specific subjects.
    /// 4. Subscribers receive and handle the events.
    ///
    /// This system ensures decoupled, type-safe communication between game components
    /// while maintaining performance through frame-based batch processing.
    /// </summary>
    public class ReactiveEventBus : MonoBehaviour
    {
        private static ReactiveEventBus instance;

        /// <summary>
        /// Gets the singleton instance of the ReactiveEventBus.
        /// </summary>
        public static ReactiveEventBus Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("ReactiveEventBus");
                    instance = go.AddComponent<ReactiveEventBus>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        // Stream that emits a signal each frame
        private readonly Subject<Unit> frameUpdateStream = new Subject<Unit>();

        // Stores subjects for each event type
        private readonly Dictionary<Type, object> eventStreams = new Dictionary<Type, object>();

        // Stores queues of pending events for each type
        private readonly Dictionary<Type, Queue<object>> eventQueues =
            new Dictionary<Type, Queue<object>>();

        private void Update()
        {
        }

        /// <summary>
        /// Gets an observable stream for the specified event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of event to observe.</typeparam>
        /// <returns>An IObservable<TEvent> that emits events of the specified type.</returns>
        public Observable<TEvent> GetStream<TEvent>()
        {
            var eventType = typeof(TEvent);
            if (!eventStreams.ContainsKey(eventType))
            {
                var subject = new Subject<TEvent>();
                eventStreams[eventType] = subject;
                eventQueues[eventType] = new Queue<object>();

                frameUpdateStream
                    .Select(_ => GetEventThisFrame<TEvent>())
                    .Where(e => e != null)
                    .Subscribe(e => subject.OnNext(e));
            }

            return (Observable<TEvent>)eventStreams[eventType];
        }

        /// <summary>
        /// Retrieves the next event of the specified type from the queue.
        /// </summary>
        private TEvent GetEventThisFrame<TEvent>()
        {
            var evenType = typeof(TEvent);
            if (
                eventQueues.TryGetValue(evenType, out var queue)
                && queue is Queue<object> typeQueue
                && typeQueue.Count > 0
            )
            {
                return (TEvent)typeQueue.Dequeue();
            }
            return default;
        }

        /// <summary>
        /// Publishes an event, queuing it for processing in the next frame.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to publish.</typeparam>
        /// <param name="eventData">The event data to publish.</param>
        public void Publish<TEvent>(TEvent eventData)
        {
            var eventType = typeof(TEvent);
            if (!eventQueues.ContainsKey(eventType))
            {
                eventQueues[eventType] = new Queue<object>();
            }

            ((Queue<object>)eventQueues[eventType]).Enqueue(eventData);
            frameUpdateStream.OnNext(Unit.Default);

        }

        private void OnDestroy()
        {
            // Clean up to prevent memory leaks
            frameUpdateStream.Dispose();
            foreach (var stream in eventStreams.Values)
            {
                (stream as IDisposable).Dispose();
            }
        }
    }
}
