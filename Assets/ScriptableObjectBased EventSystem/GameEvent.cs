using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Events/Game Event")]
public class GameEvent : ScriptableObject
{
    // List of listeners that will be notified when the event fires
    private List<IGameEventListener> listeners = new List<IGameEventListener>();

    // Raise the event (notify all listeners)
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    // Registration methods
    public void RegisterListener(IGameEventListener listener) {
        if (!listeners.Contains(listener))
        {

            listeners.Add(listener);

        }
    }
    public void UnregisterListener(IGameEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
