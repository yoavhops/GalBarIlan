﻿using System;
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

public class World1 : MonoBehaviour
{
    public int WorldNumber;
    public int WorldStartNumber;
    public WorldState _currentWorldState;
    public Animator Animator;
    public RecordingCanvas RecordingCanvas;

    private Dictionary<int, IDictionary<int, WorldState>> WorldToStateDictionary;
    private Dictionary<int, int> WorldToLastStateDictionary;


    private GameObject _currentWorld = null;
    public List<GameObject> Worlds;


    [Header("World1 Links")]
    public OnClick World1StarFishClickable;

    [Header("World2 Links")]
    public OnClick World2MegiClickable;
    public OnClick World2TanogClickable;

    [Header("World4 Links")]
    public OnClick World4FirstBoxClickable;
    public OnClick World4SecondBoxClickable;

    [Header("TestSound")]
    public bool ShouldTestFailure;

    void Awake()
    {
        VoiceAnswersMediator.Init();

        TurnOffAllWorlds();

        InitStates();

        ChangeWorld();

        ChangeWorldState(WorldStartNumber);
    }

    private void TurnOffAllWorlds()
    {
        foreach (var World in Worlds)
        {
            World.SetActive(false);
        }
    }

    private void InitStates()
    {
        WorldToLastStateDictionary = new Dictionary<int, int>()
        {
            {0, (int)World1State.NextWorld},
            {1, (int)World2State.NextWorld},
            {2, (int)World3State.NextWorld},
            {3, (int)World4State.NextWorld},
        };

        WorldToStateDictionary = new Dictionary<int, IDictionary<int, WorldState>>();

        InitWorldToStateDictionary();

        InitWorld0DictionaryAndVoiceAnswers();
        InitWorld1DictionaryAndVoiceAnswers();
        InitWorld2Dictionary();
        InitWorld3DictionaryAndVoiceAnswers();
    }

    private void InitWorldToStateDictionary()
    {
        for (int i = 0; i < Worlds.Count; i++)
        {
            WorldToStateDictionary.Add(i, new Dictionary<int, WorldState>());
        }
    }

    private void InitWorld0DictionaryAndVoiceAnswers()
    {
        var waitForYesAsnwers = new List<string>() { "yes", "okay", "ok" };

        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World1State.Introduction, World1State.WaitForClickOnStarFish));
        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World1State.WaitForClickOnStarFish, World1State.SealSwimAway, World1StarFishClickable));
        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World1State.SealSwimAway, World1State.WaitForVoiceApprove));
        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World1State.WaitForVoiceApprove, World1State.SwimAway, World1State.WaitForVoiceApproveAgain,
                       waitForYesAsnwers));

        VoiceAnswersMediator.CreateVoiceAnswer(10, (int)World1State.WaitForVoiceApprove, waitForYesAsnwers[0], ShouldTestFailure);

        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World1State.WaitForVoiceApproveAgain, World1State.WaitForVoiceApprove));
        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World1State.SwimAway, World1State.NextWorld));
    }

    private void InitWorld1DictionaryAndVoiceAnswers()
    {
        var waitForMegiAnswers = new List<string>() { "Megi", "Mago", "Mango", "margo", "margaret", "Magat" };
        var waitForTanogAnswers = new List<string>() { "Tango", "Tanog" };

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this, 
                    World2State.ChangeScreens, World2State.Intro));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this, 
                    World2State.Intro, World2State.ClickMegi));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this, 
                    World2State.ClickMegi, World2State.MegiIntro, World2MegiClickable));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                    World2State.MegiIntro, World2State.ClickMegiAgain));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                    World2State.ClickMegiAgain, World2State.WaitForMegiVoiceInput, World2MegiClickable));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                    World2State.WaitForMegiVoiceInput, World2State.GoodJobMegi, World2State.MegiIntro,
                       waitForMegiAnswers));

        VoiceAnswersMediator.CreateVoiceAnswer(1, (int)World2State.WaitForMegiVoiceInput, waitForMegiAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                    World2State.GoodJobMegi, World2State.ClickTanog));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                    World2State.ClickTanog, World2State.WaitForTanogVoiceInput, World2TanogClickable));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                    World2State.WaitForTanogVoiceInput, World2State.GoodJobTanog, World2State.ClickTanog,
                       waitForTanogAnswers));

        VoiceAnswersMediator.CreateVoiceAnswer(1, (int)World2State.WaitForTanogVoiceInput, waitForTanogAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World2State.GoodJobTanog, World2State.NextWorld));

    }

    private void InitWorld2Dictionary()
    {
        WorldToStateDictionary[2].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World3State.ChangeScreens, World3State.Intro));
        WorldToStateDictionary[2].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World3State.Intro, World3State.SwimAway));
        WorldToStateDictionary[2].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World3State.SwimAway, World3State.NextWorld));
    }

    private void InitWorld3DictionaryAndVoiceAnswers()
    {
        var waitForYesAnswers = new List<string>() { "yes", "okay", "ok" };
        var waitForFirstPasswordAnswers = new List<string>() { "the dog barks in the garden" };
        var waitForSecondPasswordAnswers = new List<string>() { "who did the horse kick" };

        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.SwimIn, World4State.AskAboutBoxes));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.AskAboutBoxes, World4State.WaitForYes));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForYes, World4State.ClickOnFirstBoxRequest,
            World4State.WaitForYesAgain, waitForYesAnswers));

        VoiceAnswersMediator.CreateVoiceAnswer(3, (int)World4State.WaitForYes, waitForYesAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForYesAgain, World4State.WaitForYes));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.ClickOnFirstBoxRequest, World4State.WaitForClickOnFirstBox));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForClickOnFirstBox, World4State.RepeatPassword, World4FirstBoxClickable));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.RepeatPassword, World4State.WaitForFirstPassword));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForFirstPassword, World4State.ClickOnSecondBoxRequest,
                World4State.WaitForFirstPasswordAgain, waitForFirstPasswordAnswers));

        VoiceAnswersMediator.CreateVoiceAnswer(3, (int)World4State.WaitForFirstPassword, waitForFirstPasswordAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForFirstPasswordAgain, World4State.WaitForFirstPassword));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.ClickOnSecondBoxRequest, World4State.WaitForClickOnSecondBox));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForClickOnSecondBox, World4State.RepeatSecondPassword, World4SecondBoxClickable));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.RepeatSecondPassword, World4State.WaitForSecondPassword));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForSecondPassword, World4State.SwimAway,
                World4State.WaitForSecondPasswordAgain, waitForSecondPasswordAnswers));

        VoiceAnswersMediator.CreateVoiceAnswer(3, (int)World4State.WaitForSecondPassword, waitForSecondPasswordAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForSecondPasswordAgain, World4State.SwimAway));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.SwimAway, World4State.NextWorld));

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

    private WorldState WorldEnumToState(int enumState)
    {
        return WorldToStateDictionary[WorldNumber][enumState];
    }

    public void ChangeAnimatorPart(int part)
    {
        _currentWorld.GetComponent<Animator>().SetInteger("Part", part);
        _currentWorld.GetComponent<Animator>().SetTrigger("ChangeState");
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

        Utils.Singleton.ScaledInvoke(() =>
        {
            RecordingCanvas.OnFinalResults(VoiceAnswersMediator.GetVoiceAnswer(WorldNumber, _currentWorldState.GetWorldStateIndex()));
        }, 3f);


    }
} 
