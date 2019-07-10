using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldState
{
    protected int _thisState;
    protected int _nextState;
    protected World1 _world1;

    protected WorldState(World1 world1, int myState, int NextState)
    {
        _world1 = world1;
        _thisState = myState;
        _nextState = NextState;
    }

    public virtual void OnAnimationEventDone(){
        Success();
    }

    protected virtual void Success()
    {
        _world1.ChangeWorldState(_nextState);
    }

    public virtual void FinishState() { }

    public int GetWorldStateIndex()
    {
        return _thisState;
    }

    public virtual void StartPart(){
        _world1.ChangeAnimatorPart((int)_thisState);
    }
}
