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
    Intro = 1,
    ClickMegi = 2,
    MegiIntro = 3,
    ClickMegiAgain = 4,
    WaitForMegiVoiceInput = 5,
    GoodJobMegi = 6,
    ClickTanog = 7,
    WaitForTanogVoiceInput = 8,
    GoodJobTanog = 9,
    NextWorld = 10,
}

public enum World3State
{
    ChangeScreens = 0,
    Intro = 1,
    SwimAway = 2,
    NextWorld = 3,
}

public enum World4State
{
    SwimIn = 0,
    AskAboutBoxes = 1,
    WaitForYes = 2,
    WaitForYesAgain = 21,
    ClickOnFirstBoxRequest = 3,
    WaitForClickOnFirstBox = 4,
    RepeatPassword = 5,
    WaitForFirstPassword = 6,
    WaitForFirstPasswordAgain = 61,
    ClickOnSecondBoxRequest = 7,
    WaitForClickOnSecondBox = 8,
    RepeatSecondPassword = 9,
    WaitForSecondPassword = 10,
    WaitForSecondPasswordAgain = 101,
    SwimAway = 11,
    NextWorld = 12,
}

[Serializable]
public class TestingFakeSound
{
    public bool UseMe = true;
    public string Result;
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


    private GameObject _currentWorld = null;
    public List<GameObject> Worlds;


    [Header("World1 Links")]
    public OnClick World1StarFishClickable;

    [Header("World2 Links")]
    public OnClick World2MegiClickable;
    public OnClick World2TanogClickable;

    [Header("World3 Links")]

    [Header("World4 Links")]
    public OnClick World4FirstBoxClickable;
    public OnClick World4SecondBoxClickable;

    [Header("TestSound")]
    public List<TestingFakeSound> TestingFakeSound;
    private int _testingFakeSoundCounter = 0;

    void Awake()
    {

        foreach (var World in Worlds)
        {
            World.SetActive(false);
        }

        InitStates();

        ChangeWorld();
        ChangeWorldState(WorldStartNumber);
    }

    private void InitStates()
    {
        WorldToLastStateDictionary = new Dictionary<int, int>()
        {
            {0, (int)World1State.NextWorld},
            {1, (int)World2State.NextWorld},
            {2, (int)World2State.NextWorld},
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
                            World1StarFishClickable)
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


            {1, new Dictionary<int, WorldState>()
                {
                    {
                        (int)World2State.ChangeScreens, new AnimationWorldState(
                            this, (int)World2State.ChangeScreens, (int)World2State.Intro)
                    },

                    {
                        (int)World2State.Intro, new AnimationWorldState(
                            this, (int)World2State.Intro, (int)World2State.ClickMegi)
                    },

                    {
                        (int)World2State.ClickMegi, new WaitForClickWorldState(
                            this, (int)World2State.ClickMegi, (int)World2State.MegiIntro, World2MegiClickable)
                    },

                    {
                        (int)World2State.MegiIntro, new AnimationWorldState(
                            this, (int)World2State.MegiIntro, (int)World2State.ClickMegiAgain)
                    },

                    {
                        (int)World2State.ClickMegiAgain, new WaitForClickWorldState(
                            this, (int)World2State.ClickMegiAgain, (int)World2State.WaitForMegiVoiceInput, World2MegiClickable)
                    },

                    {
                        (int)World2State.WaitForMegiVoiceInput, new WaitForVoiceApprove(
                            this, (int)World2State.WaitForMegiVoiceInput, (int)World2State.GoodJobMegi, (int)World2State.MegiIntro,
                            new List<string>() {"Megi", "Mago", "Mango", "margo", "margaret", "Magat"} )
                    },

                    {
                        (int)World2State.GoodJobMegi, new AnimationWorldState(
                            this, (int)World2State.GoodJobMegi, (int)World2State.ClickTanog)
                    },

                    {
                        (int)World2State.ClickTanog, new WaitForClickWorldState(
                            this, (int)World2State.ClickTanog, (int)World2State.WaitForTanogVoiceInput, World2TanogClickable)
                    },

                    {
                        (int)World2State.WaitForTanogVoiceInput, new WaitForVoiceApprove(
                            this, (int)World2State.WaitForTanogVoiceInput, (int)World2State.GoodJobTanog, (int)World2State.ClickTanog,
                            new List<string>() {"Tango", "Tanog"} )
                    },

                    {
                        (int)World2State.GoodJobTanog, new AnimationWorldState(
                            this, (int)World2State.GoodJobTanog, (int)World2State.NextWorld)
                    },
                }
            },

            {2, new Dictionary<int, WorldState>()
                {
                    {
                        (int)World3State.ChangeScreens, new AnimationWorldState(
                            this, (int)World3State.ChangeScreens, (int)World3State.Intro)
                    },

                    {
                        (int)World3State.Intro, new AnimationWorldState(
                            this, (int)World3State.Intro, (int)World3State.SwimAway)
                    },

                    {
                        (int)World3State.SwimAway, new AnimationWorldState(
                            this, (int)World3State.SwimAway, (int)World3State.NextWorld)
                    },
                }
            },

            { 4, new Dictionary<int, WorldState>(){
                {
                    (int) World4State.SwimIn, new AnimationWorldState(
                            this, (int)World4State.SwimIn, (int)World4State.AskAboutBoxes)
                },

                {
                    (int) World4State.AskAboutBoxes, new WaitForClickWorldState(
                            this, (int)World4State.AskAboutBoxes, (int)World4State.WaitForYes,
                            World4FirstBoxClickable   )
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
                            this, (int)World4State.WaitForClickOnFirstBox, (int)World4State.NextWorld, World4FirstBoxClickable)
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
                            new List<string>() {"who did the horse kick"})
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
        _currentWorldState = null;

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
    private void TestingVoiceApprove()
    {
        if (!Application.isEditor)
        {
            return;
        }

        while (_testingFakeSoundCounter < TestingFakeSound.Count)
        {
            if (TestingFakeSound[_testingFakeSoundCounter].UseMe)
            {
                break;
            }
            else
            {
                _testingFakeSoundCounter++;
            }
        }

        if (_testingFakeSoundCounter < TestingFakeSound.Count)
        {
            Utils.Singleton.ScaledInvoke(() =>
            {
                RecordingCanvas.OnFinalResults(TestingFakeSound[_testingFakeSoundCounter].Result);
                _testingFakeSoundCounter++;
            }, 3f);
        }
    }
} 
