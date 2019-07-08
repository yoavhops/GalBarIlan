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

public enum World4State
{
    ChangeScreens = 0,
    SwimInAndAskAboutBoxes = 1,
    WaitForYes = 2,
    WaitForYesAgain = 21,
    ClickOnFirstBoxRequest = 3,
    WaitForClickOnFirstBox = 4,
    RepeatPassword = 5,
    WaitForFirstPassword = 6,
    WaitForFirstPasswordAgain = 61,
    ClickOnSecondBoxRequest = 7,
    WaitForSecondPassword = 8,
    WaitForSecondPasswordAgain = 81,
    SwimAway = 9,
    NextWorld = 10,
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
    public OnClick FirstBoxClickable;
    public OnClick SecondBoxClickable;

    private GameObject _currentWorld = null;
    public List<GameObject> Worlds;

    void Awake()
    {
        InitStates();

        ChangeWorld();
        ChangeWorldState(WorldStartNumber);
    }

    private void InitStates()
    {
        WorldToLastStateDictionary = new Dictionary<int, int>()
        {
            {0, (int)World1State.NextWorld},
        };

        WorldToStateDictionary = new Dictionary<int, Dictionary<int, WorldState>>()
        {
            {0, new Dictionary<int, WorldState>(){
                {
                        (int)World1State.Introduction, new AnimationWorldState(
                            this, (int)World1State.Introduction, (int)World1State.WaitForClickOnStarFish)
                },

                    {
                        (int)World1State.WaitForClickOnStarFish, new WaitForClickWorldState(
                            this, (int)World1State.WaitForClickOnStarFish, (int)World1State.SealSwimAway,
                            StarFishClickable)
                    },

                    {
                        (int)World1State.SealSwimAway, new AnimationWorldState(
                            this, (int)World1State.SealSwimAway, (int)World1State.WaitForVoiceApprove)
                    },

                    {
                        (int) World1State.WaitForVoiceApprove, new WaitForVoiceApprove(
                            this, (int)World1State.WaitForVoiceApprove, (int)World1State.SwimAway,
                            (int)World1State.WaitForVoiceApproveAgain,
                            new List<string>() {"yes", "okay", "ok"})
                    },

                    {
                        (int) World1State.WaitForVoiceApproveAgain, new AnimationWorldState(
                            this, (int)World1State.WaitForVoiceApproveAgain, (int)World1State.WaitForVoiceApprove)
                    },

                    {
                        (int) World1State.SwimAway, new AnimationWorldState(
                            this, (int)World1State.SwimAway, (int)World1State.NextWorld)
                    },
                }
            },

            { 4, new Dictionary<int, WorldState>(){
                {
                    (int) World4State.ChangeScreens, new AnimationWorldState(
                            this, (int)World4State.ChangeScreens, (int)World4State.SwimInAndAskAboutBoxes)
                },

                {
                    (int) World4State.SwimInAndAskAboutBoxes, new WaitForClickWorldState(
                            this, (int)World4State.SwimInAndAskAboutBoxes, (int)World4State.WaitForYes,
                            FirstBoxClickable   )
                },

                {
                    (int) World4State.WaitForYes, new WaitForVoiceApprove(
                            this, (int)World4State.WaitForYes, (int)World4State.ClickOnFirstBoxRequest,
                            (int)World4State.WaitForYesAgain,
                            new List<string>() {"yes", "okay", "ok"})
                },

                {
                    (int) World4State.WaitForYesAgain, new AnimationWorldState(
                            this, (int)World4State.WaitForYesAgain, (int)World4State.WaitForYes)
                },

                {
                    (int) World4State.ClickOnFirstBoxRequest, new AnimationWorldState(
                            this, (int)World4State.ClickOnFirstBoxRequest, (int)World4State.WaitForClickOnFirstBox)
                },

                {
                    (int) World4State.WaitForClickOnFirstBox, new WaitForClickWorldState(
                            this, (int)World4State.WaitForClickOnFirstBox, (int)World4State.NextWorld, FirstBoxClickable)
                },

                {
                    (int) World4State.RepeatPassword, new AnimationWorldState(
                            this, (int)World4State.RepeatPassword, (int)World4State.WaitForFirstPassword)
                },

                {
                    (int) World4State.WaitForFirstPassword, new WaitForVoiceApprove(
                            this, (int)World4State.WaitForFirstPassword, (int)World4State.ClickOnSecondBoxRequest,
                            (int)World4State.WaitForFirstPasswordAgain,
                            new List<string>() {"the dog barks in the garden"})
                },

                {
                    (int) World4State.WaitForFirstPasswordAgain, new AnimationWorldState(
                            this, (int)World4State.WaitForFirstPasswordAgain, (int)World4State.WaitForFirstPassword)
                },

                {
                    (int) World4State.ClickOnSecondBoxRequest, new AnimationWorldState(
                            this, (int)World4State.ClickOnSecondBoxRequest, (int)World4State.WaitForSecondPassword)
                },

                {
                    (int) World4State.WaitForSecondPassword, new WaitForVoiceApprove(
                            this, (int)World4State.WaitForSecondPassword, (int)World4State.SwimAway,
                            (int)World4State.WaitForSecondPasswordAgain,
                            new List<string>() {"the dog barks in the garden"})
                },

                {
                    (int) World4State.WaitForSecondPasswordAgain, new AnimationWorldState(
                            this, (int)World4State.WaitForSecondPasswordAgain, (int)World4State.SwimAway)
                },
                
                {
                    (int) World4State.SwimAway, new AnimationWorldState(
                            this, (int)World4State.SwimAway, (int)World4State.NextWorld)
                },
            }
            }
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
        _currentWorld?.SetActive(false);
        Worlds[WorldNumber].SetActive(true);
        _currentWorld = Worlds[WorldNumber];

        Animator.SetInteger("WorldNum", WorldNumber);

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
        _currentWorld.GetComponent<Animator>().SetInteger("Part", part);
        _currentWorld.GetComponent<Animator>().SetTrigger("ChangeState");
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
