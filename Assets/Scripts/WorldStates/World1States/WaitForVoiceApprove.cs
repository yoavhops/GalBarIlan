using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaitForVoiceApprove : WorldState
{
    private int _failureState;
    private List<string> _correctVoiceAnswers;

    public WaitForVoiceApprove(World1 world1, int myState, int
    NextState, int stateAfterFailure, List<string> correctVoiceAnswers) :
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
