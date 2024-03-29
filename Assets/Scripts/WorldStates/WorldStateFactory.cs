﻿using System.Collections;
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

    public static KeyValuePair<int, WorldState> GetIntToClickWorldStateKeyValuePair<T>(World1 world, T thisWorldState, T
                nextWorldState, OnClick clickable, bool useNoClickPrompt = true)
    {
        var worldStateInt = Convert.ToInt32(thisWorldState);
        var nextWorldStateInt = Convert.ToInt32(nextWorldState);

        var worldState = new WaitForClickWorldState(world, Convert.ToInt32(worldStateInt), nextWorldStateInt, clickable, useNoClickPrompt);
        return new KeyValuePair<int, WorldState>(worldStateInt, worldState);
    }

    public static KeyValuePair<int, WorldState> GetIntToVoiceWorldStateKeyValuePair<T>(World1 world, T thisWorldState, T nextWorldState, 
                T failState, List<string> correctAnswers, bool allowIfLoadEnough = false, bool onlyLoudness = false,
                bool useNegativeFeedbackWorldState = false, bool usePositiveFeedbackWorldState = false,
        float? customeDelay = null, WaitForVoiceApprove.CustomFeedBack customFeedBack = null)
    {
        var worldStateInt = Convert.ToInt32(thisWorldState);
        var nextWorldStateInt = Convert.ToInt32(nextWorldState);
        var failStateInt = Convert.ToInt32(failState);

        var worldState = new WaitForVoiceApprove(world, worldStateInt, nextWorldStateInt, failStateInt, correctAnswers,
            allowIfLoadEnough, onlyLoudness, useNegativeFeedbackWorldState, usePositiveFeedbackWorldState,
            customeDelay, customFeedBack);
        return new KeyValuePair<int, WorldState>(worldStateInt, worldState);
    }

}
