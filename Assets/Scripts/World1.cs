using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum World1State
{
    Introduction = 0,
    WaitForClickOnStarFish = 1,
    SealSwimAway = 2,
    WaitForVoiceApprove = 3,
    WaitForVoiceApproveAgain = 31,
    SwimAway = 4,
}


public class World1 : MonoBehaviour
{
    public World1State World1State;
    public WorldState _currentWorldState;
    public Animator Animator;
    public RecordingCanvas RecordingCanvas;
    private Dictionary<World1State, WorldState> EnumToState;
    public OnClick StarFishClickable;

    void Awake()
    {
        InitStates();
        ChangeWorldState(World1State);
    }

    private void InitStates()
    {
        EnumToState = new Dictionary<World1State, WorldState>()
        {
            {World1State.Introduction, new AnimationWorldState(
                    this, World1State.Introduction, World1State.WaitForClickOnStarFish) },

            {World1State.WaitForClickOnStarFish, new WaitForClickWorldState(
                    this, World1State.WaitForClickOnStarFish, World1State.SealSwimAway,
                    StarFishClickable)
            },

            {World1State.SealSwimAway, new AnimationWorldState(
                    this, World1State.SealSwimAway, World1State.WaitForVoiceApprove)
            },

            {World1State.WaitForVoiceApprove, new WaitForVoiceApprove(
                    this, World1State.WaitForVoiceApprove, World1State.SwimAway, World1State.WaitForVoiceApproveAgain,
                    new List<string>(){"yes", "okay", "ok"})
            },

            {World1State.WaitForVoiceApproveAgain, new AnimationWorldState(
                    this, World1State.WaitForVoiceApproveAgain, World1State.WaitForVoiceApprove)
            },
           
            {World1State.SwimAway, new AnimationWorldState(
                    this, World1State.SwimAway, World1State.Introduction)
            }, //Set next state to Introduction temporarily
        };
     
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeWorldState(World1State newWorld1State)
    {
        _currentWorldState?.FinishState();
        World1State = newWorld1State;
        _currentWorldState = EnumToState[newWorld1State];
        _currentWorldState.StartPart();
    }

    public void ChangeAnimatorPart(int part)
    {
        Animator.SetInteger("Part", part);
        Animator.SetTrigger("ChangeState");
    }
   
    public void StartRecording()
    {
        RecordingCanvas.StartRecording();
        TestingVoiceApprove();
    }

    /****** Part End*******/
    public void OnAnimationEventDone()
    {
        EnumToState[World1State].OnAnimationEventDone();
    }

    /****** Testing *******/
    private bool _firstTestingVoiceApprove = true;
    private void TestingVoiceApprove()
    {
        if (!Application.isEditor)
        {
            return;
        }

        if (_firstTestingVoiceApprove)
        {
            _firstTestingVoiceApprove = false;
            Utils.Singleton.ScaledInvoke(() =>
            {
                RecordingCanvas.OnFinalResults("No");
            }, 3f);
        }
        else
        {
            Utils.Singleton.ScaledInvoke(() =>
            {
                RecordingCanvas.OnFinalResults("Yes");
            }, 3f);
        }
    }
} 
