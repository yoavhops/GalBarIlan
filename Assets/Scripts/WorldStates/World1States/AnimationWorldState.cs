using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWorldState : WorldState
{
    public AnimationWorldState(World1 world1, World1State myState, World1State 
        NextState) : 
        base(world1, myState, NextState)
    { }

}
