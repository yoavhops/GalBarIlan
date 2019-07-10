using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class WorldStateFactory
{
    public delegate int EnumToInt<T>(T WolrdStateEnum);

    public static KeyValuePair<int,WorldState> GetIntToWorldStateKeyValuePair<T>(World1 world, T thisWorldState, T nextWorldState)
    {
        var worldStateInt = Convert.ToInt32(thisWorldState);
        var nextWorldStateInt = Convert.ToInt32(nextWorldState);
         
        var worldState = new AnimationWorldState(world, worldStateInt, nextWorldStateInt);
        return new KeyValuePair<int, WorldState>(worldStateInt, worldState);
    }

    public static KeyValuePair<int, WorldState> GetIntToWorldStateKeyValuePair<T>(World1 world, T thisWorldState, T
                nextWorldState, OnClick clickable)
    {
        var worldStateInt = Convert.ToInt32(thisWorldState);
        var nextWorldStateInt = Convert.ToInt32(nextWorldState);

        var worldState = new WaitForClickWorldState(world, Convert.ToInt32(worldStateInt), nextWorldStateInt, clickable);
        return new KeyValuePair<int, WorldState>(worldStateInt, worldState);
    }

    public static KeyValuePair<int, WorldState> GetIntToWorldStateKeyValuePair<T>(World1 world, T thisWorldState, T nextWorldState, 
                T failState, List<string> correctAnswers )
    {
        var worldStateInt = Convert.ToInt32(thisWorldState);
        var nextWorldStateInt = Convert.ToInt32(nextWorldState);
        var failStateInt = Convert.ToInt32(failState);

        var worldState = new WaitForVoiceApprove(world, worldStateInt, nextWorldStateInt, failStateInt, correctAnswers);
        return new KeyValuePair<int, WorldState>(worldStateInt, worldState);
    }

}
