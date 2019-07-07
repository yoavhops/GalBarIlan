using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaitForVoiceApprove : WorldState
{
    private World1State _failureState;
    private List<string> _correctVoiceAnswers;

    public WaitForVoiceApprove(World1 world1, World1State myState, World1State
    NextState, World1State stateAfterFailure, List<string> correctVoiceAnswers) :
    base(world1, myState, NextState)
    {
        _failureState = stateAfterFailure;
        _correctVoiceAnswers = correctVoiceAnswers;      
    }

    public override void StartPart()
    {
        base.StartPart();
        _world1.RecordingCanvas.OnFinalResults += HandleVoiceResult;
        _world1.RecordingCanvas.OnFailureToRecord += HandleFailedRecord;
        _world1.StartRecording();
    }

    private void HandleFailedRecord()
    {
        Failure();
    }

    private void HandleVoiceResult(string result)
    {
        foreach(var correctAnswer in _correctVoiceAnswers)
        {
            if(string.Equals(result, correctAnswer, StringComparison.OrdinalIgnoreCase))
            {
                Success();
                return;
            }
        }

        Failure();
    }

    protected void Failure()
    {
        _world1.ChangeWorldState(_failureState);
    }

    public override void FinishState()
    {
        _world1.RecordingCanvas.OnFinalResults -= HandleVoiceResult;
        _world1.RecordingCanvas.OnFailureToRecord -= HandleFailedRecord;
    }

}
