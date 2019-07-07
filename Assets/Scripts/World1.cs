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
    NextWorld = 5,
}

public enum World2State
{
    ChangeScreens = 0,
    WaitForClickOnStarFish = 1,
    SealSwimAway = 2,
    WaitForVoiceApprove = 3,
    WaitForVoiceApproveAgain = 31,
    SwimAway = 4,
}

public class World1 : MonoBehaviour
{
    public int WorldNumber;
    public int WorldStartNumber;
    public WorldState _currentWorldState;
    public Animator Animator;
    public RecordingCanvas RecordingCanvas;

    private Dictionary<int, Dictionary<int, WorldState>> WorldToStateDictionary;
    private Dictionary<int, int> WorldToLastStateDictionary;

    public OnClick StarFishClickable;

    void Awake()
    {
        InitStates();

        ChangeWorld();
        ChangeWorldState(WorldStartNumber);
    }

    private void InitStates()
    {
        WorldToStateDictionary = new Dictionary<int, Dictionary<int, WorldState>>()
        {
            {0, new Dictionary<int, WorldState>(){
                    {
                        (int) World1State.Introduction, new AnimationWorldState(
                            this, (int)World1State.Introduction, (int)World1State.WaitForClickOnStarFish)
                    },

                    {
                        (int) World1State.WaitForClickOnStarFish, new WaitForClickWorldState(
                            this, (int)World1State.WaitForClickOnStarFish, (int)World1State.SealSwimAway,
                            StarFishClickable)
                    },

                    {
                        (int) World1State.SealSwimAway, new AnimationWorldState(
                            this, (int)World1State.SealSwimAway, (int)World1State.WaitForVoiceApprove)
                    },

                    {
                        (int) World1State.WaitForVoiceApprove, new WaitForVoiceApprove(
                            this, World1State.WaitForVoiceApprove, World1State.SwimAway,
                            World1State.WaitForVoiceApproveAgain,
                            new List<string>() {"yes", "okay", "ok"})
                    },

                    {
                        (int) World1State.WaitForVoiceApproveAgain, new AnimationWorldState(
                            this, (int)World1State.WaitForVoiceApproveAgain, (int)World1State.WaitForVoiceApprove)
                    },

                    {
                        (int) World1State.SwimAway, new AnimationWorldState(
                            this, (int)World1State.SwimAway, (int)World1State.Introduction)
                    }, //Set next state to Introduction temporarily
                }
            },
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

    private WorldState WorldEnumToState(int enumState)
    {
        return WorldToStateDictionary[WorldNumber][enumState];
    }

    private bool ShouldGoToNextWorld(int nextState)
    {
        return WorldToLastStateDictionary[WorldNumber] == nextState;
    }

    private void NextWorld()
    {
        WorldNumber++;
        ChangeWorld();
    }

    private void ChangeWorld()
    {
        Animator.SetInteger("worldNum", WorldNumber);
        ChangeWorldState(0);
    }
    
    public void ChangeWorldState(int newWorldState)
    {
        _currentWorldState?.FinishState();

        if (ShouldGoToNextWorld(newWorldState))
        {
            NextWorld();
            return;
        }

        _currentWorldState = WorldEnumToState(newWorldState);
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
        _currentWorldState.OnAnimationEventDone();
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
