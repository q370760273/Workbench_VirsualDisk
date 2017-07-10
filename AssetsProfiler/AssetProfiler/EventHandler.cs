using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventHandler : Singleton<EventHandler>
{
    private Dictionary<string, Action<object[]>> _handlers = new Dictionary<string, Action<object[]>>();

    public override void Dispose()
    {
        _handlers.Clear();
    }

    public void RegisterHandler(string eventName, Action<object[]> handler)
    {
        if (!_handlers.ContainsKey(eventName))
        {
            _handlers[eventName] = handler;
        }
        else
        {
            _handlers[eventName] -= handler; //去重
            _handlers[eventName] += handler; //添加多个监听函数
        }
    }

    public void ReleaseHandler(string eventName, Action<object[]> handler)
    {
        if (!_handlers.ContainsKey(eventName))
            return;
        _handlers[eventName] -= handler;
        if (_handlers[eventName] == null)
            _handlers.Remove(eventName);
    }

    public void HandleInvoke(string eventName, params object[] param)
    {
        if (_handlers.ContainsKey(eventName))
            _handlers[eventName].Invoke(param);
    }
}
