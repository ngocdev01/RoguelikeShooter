using System;
using System.Collections.Generic;
using UnityEngine;

namespace NgocDev.Core.Event
{
    public interface IGameEvent { }

    public interface IEventBus
    {
        IDisposable Subscribe<TEvent>(Action<TEvent> listener)
            where TEvent : IGameEvent;

        void Publish<TEvent>(TEvent gameEvent)
            where TEvent : IGameEvent;

        void Clear();
    }

    public sealed class EventBus : MonoBehaviour, IEventBus
    {
        private readonly Dictionary<Type, IEventChannel> _channels = new();

        public IDisposable Subscribe<TEvent>(Action<TEvent> listener)
            where TEvent : IGameEvent
        {
            return GetChannel<TEvent>().Subscribe(listener);
        }

        public void Publish<TEvent>(TEvent gameEvent)
            where TEvent : IGameEvent
        {
            GetChannel<TEvent>().Publish(gameEvent);
        }

        public void Clear()
        {
            foreach (var channel in _channels.Values)
            {
                channel.Clear();
            }

            _channels.Clear();
        }

        private EventChannel<TEvent> GetChannel<TEvent>()
            where TEvent : IGameEvent
        {
            var type = typeof(TEvent);

            if (!_channels.TryGetValue(type, out var channel))
            {
                channel = new EventChannel<TEvent>();
                _channels[type] = channel;
            }

            return (EventChannel<TEvent>)channel;
        }
    }

    internal interface IEventChannel
    {
        void Clear();
    }

    internal sealed class EventChannel<TEvent> : IEventChannel
        where TEvent : IGameEvent
    {
        private readonly HashSet<Action<TEvent>> _listeners = new();
        private readonly List<Action<TEvent>> _publishBuffer = new();

        public IDisposable Subscribe(Action<TEvent> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            _listeners.Add(listener);

            return new EventSubscription<TEvent>(this, listener);
        }

        public void Publish(TEvent gameEvent)
        {
            _publishBuffer.Clear();
            _publishBuffer.AddRange(_listeners);

            foreach (var listener in _publishBuffer)
            {
                if (!_listeners.Contains(listener))
                    continue;

                try
                {
                    listener.Invoke(gameEvent);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            _publishBuffer.Clear();
        }

        public void Unsubscribe(Action<TEvent> listener)
        {
            if (listener == null)
                return;

            _listeners.Remove(listener);
        }

        public void Clear()
        {
            _listeners.Clear();
            _publishBuffer.Clear();
        }
    }

    internal sealed class EventSubscription<TEvent> : IDisposable
        where TEvent : IGameEvent
    {
        private EventChannel<TEvent> _channel;
        private Action<TEvent> _listener;

        public EventSubscription(EventChannel<TEvent> channel, Action<TEvent> listener)
        {
            _channel = channel;
            _listener = listener;
        }

        public void Dispose()
        {
            if (_channel == null || _listener == null)
                return;

            _channel.Unsubscribe(_listener);

            _channel = null;
            _listener = null;
        }
    }
}