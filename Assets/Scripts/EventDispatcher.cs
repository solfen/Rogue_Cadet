using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Events {
    TEST
}

public static class EventDispatcher {

    public delegate void EventHandler(object emiter);
    private static event EventHandler genericEvent;
    private static Dictionary<Events, EventHandler> events = new Dictionary<Events, EventHandler> {
        { Events.TEST,  null }
    };

    public static void AddEventListener(Events type, EventHandler method) {
        events[type] += method;
    }

    public static void RemoveEventListener(Events type, EventHandler method) {
        events[type] -= method;
    }

    public static void DispatchEvent(Events type, object sender) {
        if(events[type] != null) {
            events[type](sender);
        }
    }
}
