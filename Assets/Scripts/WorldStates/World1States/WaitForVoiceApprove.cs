using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using KKSpeech;

public class WaitForVoiceApprove : WorldState
{
    private int _failureState;
    private List<string> _correctVoiceAnswers;
    private bool _onlyLoudness;
    private bool _allowIfLoadEnough;

    public WaitForVoiceApprove(World1 world1, int myState, int
    NextState, int stateAfterFailure, List<string> correctVoiceAnswers,
        bool allowIfLoadEnough = false, bool onlyLoudness = false) :
    base(world1, myState, NextState)
    {
        _allowIfLoadEnough = allowIfLoadEnough;
        _onlyLoudness = onlyLoudness;
        _failureState = stateAfterFailure;
        _correctVoiceAnswers = correctVoiceAnswers; 
    }

    public override void StartPart()
    {
        base.StartPart();
        _world1.RecordingCanvas.OnFinalResults += HandleVoiceResult;
        _world1.RecordingCanvas.OnFailureToRecord += HandleFailedRecord;
        if (_onlyLoudness)
        {
            _world1.RecordingCanvas.OnMicrophoneLoadEnoughEvent += OnMicrophoneLoadEnough;
        }
        _world1.StartRecording();
    }

    private void OnMicrophoneLoadEnough()
    {
        if (_onlyLoudness)
        {
            Success();
        }
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
        if (_allowIfLoadEnough && SpeechRecognizer.WasMicrophoneLoadEnough())
        {
            Success();
            return;
        }

        _world1.ChangeWorldState(_failureState);
    }

    public override void FinishState()
    {
        _world1.RecordingCanvas.OnFinalResults -= HandleVoiceResult;
        _world1.RecordingCanvas.OnFailureToRecord -= HandleFailedRecord;
        if (_onlyLoudness)
        {
            _world1.RecordingCanvas.OnMicrophoneLoadEnoughEvent -= OnMicrophoneLoadEnough;
        }
    }

}
