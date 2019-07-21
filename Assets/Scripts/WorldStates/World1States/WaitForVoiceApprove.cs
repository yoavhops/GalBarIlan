using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using KKSpeech;

public class WaitForVoiceApprove : WorldState
{
    private static bool _ALWAYS_ONLY_LOAD_ENOUGH = true;
    private static bool _MIC_TIME_CHECK = true;

    private int _failureState;
    private List<string> _correctVoiceAnswers;
    private bool _onlyLoudness;
    private bool _allowIfLoadEnough;

    private bool _firstMicRecived = false;
    private readonly float _timeSinceLastMic = 1f;
    private float _currentTimeSinceLastMic;

    private readonly float _failureTime = 3f;
    private float _currentFailureTime;

    private bool _useNegativeFeedbackWorldState = true;
    private bool _usePositiveFeedbackWorldState = true;

    public WaitForVoiceApprove(World1 world1, int myState, int
    NextState, int stateAfterFailure, List<string> correctVoiceAnswers,
        bool allowIfLoadEnough = false, bool onlyLoudness = false,
        bool useNegativeFeedbackWorldState = false, bool usePositiveFeedbackWorldState = false) :
    base(world1, myState, NextState)
    {
        if (_ALWAYS_ONLY_LOAD_ENOUGH)
        {
            allowIfLoadEnough = false;
            onlyLoudness = true;
        }

        if (onlyLoudness && allowIfLoadEnough)
        {
            UnityEngine.Debug.LogError("Issue with onlyLoudness and allowIfLoadEnough");
            allowIfLoadEnough = false;
        }

        _usePositiveFeedbackWorldState = usePositiveFeedbackWorldState;
        _useNegativeFeedbackWorldState = useNegativeFeedbackWorldState;
        _allowIfLoadEnough = allowIfLoadEnough;
        _onlyLoudness = onlyLoudness;
        _failureState = stateAfterFailure;
        _correctVoiceAnswers = correctVoiceAnswers;
    }

    public override void StartPart()
    {
        base.StartPart();

        _currentTimeSinceLastMic = _timeSinceLastMic;
        _currentFailureTime = _failureTime;
        _firstMicRecived = false;

        _world1.RecordingCanvas.OnFinalResults += HandleVoiceResult;
        _world1.RecordingCanvas.OnFailureToRecord += HandleFailedRecord;
        if (_onlyLoudness)
        {
            _world1.RecordingCanvas.OnMicrophoneLoadEnoughEvent += OnMicrophoneLoadEnough;
        }
        _world1.StartRecording(_onlyLoudness);
    }

    private void OnMicrophoneLoadEnough()
    {
        if (_MIC_TIME_CHECK)
        {
            _firstMicRecived = true;
            _currentTimeSinceLastMic = _timeSinceLastMic;
            return;
        }

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
            if (result.ToLower().Contains(correctAnswer.ToLower()))
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

        if (_useNegativeFeedbackWorldState)
        {
            _world1.ChangeToTempWorldState(new FeedBackWorldState(_world1, -1, _failureState, 1.5f, _world1.GeneralStatesCouldYouRepeatThat));
            return;
        }

        _world1.ChangeWorldState(_failureState);
    }

    protected override void Success()
    {
        if (_usePositiveFeedbackWorldState)
        {
            _world1.ChangeToTempWorldState(new FeedBackWorldState(_world1, -1, _nextState, 1f, _world1.GeneralStatesGoodUI));
            return;
        }

        _world1.ChangeWorldState(_nextState);
    }

    public override void FinishState()
    {
        _world1.StopRecording();

        _world1.RecordingCanvas.OnFinalResults -= HandleVoiceResult;
        _world1.RecordingCanvas.OnFailureToRecord -= HandleFailedRecord;
        if (_onlyLoudness)
        {
            _world1.RecordingCanvas.OnMicrophoneLoadEnoughEvent -= OnMicrophoneLoadEnough;
        }
    }

    public override void Update()
    {
        _currentFailureTime -= Time.deltaTime;

        if (!_MIC_TIME_CHECK)
        {
            return;
        }

        if (_currentFailureTime < 0 && !_firstMicRecived)
        {
            Failure();
            return;
        }

        if (_firstMicRecived)
        {
            _currentTimeSinceLastMic -= Time.deltaTime;

            if (_currentTimeSinceLastMic < 0)
            {
                Success();
                return;
            }
        }
    }

}
