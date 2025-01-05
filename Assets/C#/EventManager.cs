using System;
using System.Collections.Generic;

public static class EventManager
{
    private static Dictionary<string, Delegate> eventDictionary = new Dictionary<string, Delegate>();

    // 订阅无参数事件
    public static void Subscribe(string eventName, Action listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            eventDictionary[eventName] = Delegate.Combine(thisEvent, listener);
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    // 取消订阅无参数事件
    public static void Unsubscribe(string eventName, Action listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            eventDictionary[eventName] = Delegate.Remove(thisEvent, listener);
        }
    }

    // 发布无参数事件
    public static void Publish(string eventName)
    {
        if (eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            if (thisEvent is Action action)
            {
                action.Invoke();
            }
        }
    }

    // 订阅带参数事件
    public static void Subscribe<T>(string eventName, Action<T> listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            eventDictionary[eventName] = Delegate.Combine(thisEvent, listener);
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    // 取消订阅带参数事件
    public static void Unsubscribe<T>(string eventName, Action<T> listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            eventDictionary[eventName] = Delegate.Remove(thisEvent, listener);
        }
    }

    // 发布带参数事件
    public static void Publish<T>(string eventName, T arg)
    {
        if (eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            if (thisEvent is Action<T> action)
            {
                action.Invoke(arg);
            }
        }
    }
}