using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventPasser : MonoBehaviour
{
    public UnityEvent OnAnimationEvent;
    
    public void OnAnimationEventDone()
    {
        OnAnimationEvent?.Invoke();
    }
}