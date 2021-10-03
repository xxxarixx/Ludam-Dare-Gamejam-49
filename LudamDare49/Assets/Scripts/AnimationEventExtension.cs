using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AnimationEventExtension : MonoBehaviour
{
    public List<UnityEvent> events = new List<UnityEvent>();
    public void PlayEvent(int EventID)
    {
        events[EventID].Invoke();
    }
}
