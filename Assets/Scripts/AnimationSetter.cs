using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Float = 0,
    Swim = 1,
    Talk = 2,
}

public class AnimationSetter : MonoBehaviour
{

    public Animator Animator;
    public AnimationType AnimationType;

    void Update()
    {
        Animator.SetInteger("AnimationType", (int)AnimationType);
    }

}
