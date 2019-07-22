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
    PlayerClickMegiAgain = 22,
    MegiIntroAgain = 33,
    WaitForMegiVoiceInput = 5,
    GoodJobMegi = 6,

    ClickKripar = 121,
    RepeatKripar = 122,
    WaitForKriparVoiceInput = 123,
    GoodJobKripar = 124,

    ClickSpie = 131,
    RepeatSpie = 132,
    WaitForSpieVoiceInput = 133,
    GoodJobSpie = 134,

    ClickTogin = 141,
    RepeatTogin = 142,
    WaitForToginVoiceInput = 143,
    GoodJobTogin = 144,

    ClickTanog = 7,
    RepeatTanog = 8,
    WaitForTanogVoiceInput = 9,
    GoodJobTanog = 10,
    NextWorld = 11,
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

    //box2
    Box2_WaitForClickOnFirstBox = 201,
    Box2_RepeatPassword = 202,
    Box2_WaitForFirstPassword = 203,

    //box3
    Box3_WaitForClickOnFirstBox = 301,
    Box3_RepeatPassword = 302,
    Box3_WaitForFirstPassword = 303,

    //box4
    Box4_WaitForClickOnFirstBox = 401,
    Box4_RepeatPassword = 402,
    Box4_WaitForFirstPassword = 403,


    WaitForClickOnSecondBox = 8,
    RepeatSecondPassword = 9,
    WaitForSecondPassword = 10,
    WaitForSecondPasswordAgain = 101,
    SwimAway = 11,
    NextWorld = 12,
}

public enum World5State
{
    ChangeScreens = 0,
    Intro = 1,
    SwimAroundAway = 2,
    TellAStory = 3,
    SwimOut = 4,
    NextWorld = 5,
}

public enum World6State
{
    ChangeScreens = 0,
    Intro = 1,

    E = 2,
    VoiceE = 21,
    N = 3,
    VoiceN = 31,
    Th = 4,
    VoiceTh = 41,
    The = 5,
    VoiceThe = 51,
    C = 6,
    VoiceC = 61,
    A = 7,
    VoiceA = 71,
    Ve = 8,
    VoiceVe = 81,

    InTheCave = 9,

    SwimOut = 10,

    NextWorld = 11,
}

public enum World7State
{
    ChangeScreens = 0,
    Intro = 1,
    SwimOut = 2,
    NextWorld = 3,
}

public class World1 : MonoBehaviour
{
    public int WorldNumber;
    public int WorldStartNumber;
    public WorldState _currentWorldState;
    public Animator Animator;
    public RecordingCanvas RecordingCanvas;
    public GameObject Menu;

    private Dictionary<int, IDictionary<int, WorldState>> WorldToStateDictionary;
    private Dictionary<int, int> WorldToLastStateDictionary;


    private GameObject _currentWorld = null;
    public List<GameObject> Worlds;


    [Header("World1 Links")]
    public OnClick World1StarFishClickable;

    [Header("World2 Links")]
    public OnClick World2MegiClickable;
    public OnClick World2TanogClickable;
    public OnClick World2KriparClickable;
    public OnClick World2SpieClickable;
    public OnClick World2ToginClickable;

    [Header("World4 Links")]
    public OnClick World4FirstBoxClickable;
    public OnClick World4Box2SecondBoxClickable;
    public OnClick World4Box3SecondBoxClickable;
    public OnClick World4Box4SecondBoxClickable;
    public OnClick World4SecondBoxClickable;

    [Header("GeneralStates")]
    public GameObject GeneralStatesCouldYouRepeatThat;
    public GameObject GeneralStatesGoodUI;

    [Header("TestSound")]
    public bool ShouldTestFailure;

    void Awake()
    {
        VoiceAnswersMediator.Init();

        TurnOffAllWorlds();

        InitStates();

        ChangeWorld();

        ChangeWorldState(WorldStartNumber);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

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
            {4, (int)World5State.NextWorld},
            {5, (int)World6State.NextWorld},
            {6, (int)World7State.NextWorld},
        };

        WorldToStateDictionary = new Dictionary<int, IDictionary<int, WorldState>>();

        InitWorldToStateDictionary();

        InitWorld0DictionaryAndVoiceAnswers();
        InitWorld1DictionaryAndVoiceAnswers();
        InitWorld2Dictionary();
        InitWorld3DictionaryAndVoiceAnswers();
        InitWorld4Dictionary();
        InitWorld5Dictionary();
        InitWorld6Dictionary();
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
        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
                World1State.WaitForClickOnStarFish, World1State.SealSwimAway, World1StarFishClickable));
        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World1State.SealSwimAway, World1State.NextWorld));
        /*
        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World1State.WaitForVoiceApprove, World1State.SwimAway, World1State.WaitForVoiceApproveAgain,
                       waitForYesAsnwers));

        VoiceAnswersMediator.CreateVoiceAnswer(10, (int)World1State.WaitForVoiceApprove, waitForYesAsnwers[0], ShouldTestFailure);

        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World1State.WaitForVoiceApproveAgain, World1State.WaitForVoiceApprove));
        WorldToStateDictionary[0].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World1State.SwimAway, World1State.NextWorld));
                */
    }

    private void InitWorld1DictionaryAndVoiceAnswers()
    {

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this, 
                    World2State.ChangeScreens, World2State.Intro));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this, 
                    World2State.Intro, World2State.ClickMegi));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this, 
                    World2State.ClickMegi, World2State.MegiIntro, World2MegiClickable));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                    World2State.MegiIntro, World2State.ClickMegiAgain));

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                World2State.ClickMegiAgain, World2State.PlayerClickMegiAgain));

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
                World2State.PlayerClickMegiAgain, World2State.WaitForMegiVoiceInput, World2MegiClickable));

        /*
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
            World2State.ClickMegiAgain, World2State.WaitForMegiVoiceInput, World2MegiClickable));
            */

        var waitForMegiAnswers = new List<string>() { "Megi", "Mago", "Mango", "margo", "margaret", "Magat" };
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
                    World2State.WaitForMegiVoiceInput, World2State.GoodJobMegi, World2State.MegiIntro,
                       waitForMegiAnswers, allowIfLoadEnough: true));

        VoiceAnswersMediator.CreateVoiceAnswer(1, (int)World2State.WaitForMegiVoiceInput, waitForMegiAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
                    World2State.GoodJobMegi, World2State.ClickKripar));

        /*Kripar*/

        var waitForKriparAnswers = new List<string>() { "Kripar"};

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
            World2State.ClickKripar, World2State.RepeatKripar, World2KriparClickable));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World2State.RepeatKripar, World2State.WaitForKriparVoiceInput));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World2State.WaitForKriparVoiceInput, World2State.GoodJobKripar, World2State.ClickKripar,
            waitForKriparAnswers, allowIfLoadEnough: true));

        VoiceAnswersMediator.CreateVoiceAnswer(1, (int)World2State.WaitForKriparVoiceInput, waitForKriparAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World2State.GoodJobKripar, World2State.ClickSpie));

        /*Spie*/

        var waitForSpieAnswers = new List<string>() { "Spie" };

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
            World2State.ClickSpie, World2State.RepeatSpie, World2SpieClickable));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World2State.RepeatSpie, World2State.WaitForSpieVoiceInput));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World2State.WaitForSpieVoiceInput, World2State.GoodJobSpie, World2State.ClickSpie,
            waitForSpieAnswers, allowIfLoadEnough: true));

        VoiceAnswersMediator.CreateVoiceAnswer(1, (int)World2State.WaitForSpieVoiceInput, waitForSpieAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World2State.GoodJobSpie, World2State.ClickTogin));
        
        /*Togin*/

        var waitForToginAnswers = new List<string>() { "Togin" };

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
            World2State.ClickTogin, World2State.RepeatTogin, World2ToginClickable));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World2State.RepeatTogin, World2State.WaitForToginVoiceInput));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World2State.WaitForToginVoiceInput, World2State.GoodJobTogin, World2State.ClickTogin,
            waitForToginAnswers, allowIfLoadEnough: true));

        VoiceAnswersMediator.CreateVoiceAnswer(1, (int)World2State.WaitForToginVoiceInput, waitForToginAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World2State.GoodJobTogin, World2State.ClickTanog));
        
        /*Tanog*/
        var waitForTanogAnswers = new List<string>() { "Tango", "Tanog" };

        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
            World2State.ClickTanog, World2State.RepeatTanog, World2TanogClickable));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World2State.RepeatTanog, World2State.WaitForTanogVoiceInput));
        WorldToStateDictionary[1].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
                    World2State.WaitForTanogVoiceInput, World2State.GoodJobTanog, World2State.ClickTanog,
                       waitForTanogAnswers, allowIfLoadEnough: true));

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
        var waitForFirstPasswordAnswers = new List<string>() { "the dog barks in the garden", "dog" };
        var waitForSecondPasswordAnswers = new List<string>() { "who did the horse kick", "horse" };

        bool justSayAgainNoVORepeat = true;

        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.SwimIn, World4State.AskAboutBoxes));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.AskAboutBoxes, World4State.WaitForClickOnFirstBox));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World4State.WaitForYes, World4State.ClickOnFirstBoxRequest,
            World4State.WaitForYesAgain, waitForYesAnswers, false, false, true, true));

        VoiceAnswersMediator.CreateVoiceAnswer(3, (int)World4State.WaitForYes, waitForYesAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForYesAgain, World4State.WaitForYes));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.ClickOnFirstBoxRequest, World4State.WaitForClickOnFirstBox));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
            World4State.WaitForClickOnFirstBox, World4State.RepeatPassword, World4FirstBoxClickable));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.RepeatPassword, World4State.WaitForFirstPassword));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World4State.WaitForFirstPassword, World4State.ClickOnSecondBoxRequest,
            justSayAgainNoVORepeat ? World4State.WaitForFirstPassword : World4State.RepeatPassword, 
            waitForFirstPasswordAnswers, false, false, true, true));

        VoiceAnswersMediator.CreateVoiceAnswer(3, (int)World4State.WaitForFirstPassword, waitForFirstPasswordAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForFirstPasswordAgain, World4State.WaitForFirstPassword));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.ClickOnSecondBoxRequest, World4State.Box2_WaitForClickOnFirstBox));

        //box2
        var Box2Answer = new List<string>() { "yes", "okay", "ok" };
        World4State next = World4State.Box3_WaitForClickOnFirstBox;
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
            World4State.Box2_WaitForClickOnFirstBox, World4State.Box2_RepeatPassword, World4Box2SecondBoxClickable));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.Box2_RepeatPassword, World4State.Box2_WaitForFirstPassword));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World4State.Box2_WaitForFirstPassword, next,
            justSayAgainNoVORepeat ? World4State.Box2_WaitForFirstPassword : World4State.Box2_RepeatPassword, 
            Box2Answer, false, false, true, true));
        VoiceAnswersMediator.CreateVoiceAnswer(3, (int)World4State.Box2_WaitForFirstPassword, Box2Answer[0], ShouldTestFailure);

        //box3
        var Box3Answer = new List<string>() { "yes", "okay", "ok" };
        next = World4State.Box4_WaitForClickOnFirstBox;
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
            World4State.Box3_WaitForClickOnFirstBox, World4State.Box3_RepeatPassword, World4Box3SecondBoxClickable));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.Box3_RepeatPassword, World4State.Box3_WaitForFirstPassword));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World4State.Box3_WaitForFirstPassword, next,
            justSayAgainNoVORepeat ? World4State.Box3_WaitForFirstPassword : World4State.Box3_RepeatPassword,
            Box3Answer, false, false, true, true));
        VoiceAnswersMediator.CreateVoiceAnswer(3, (int)World4State.Box3_WaitForFirstPassword, Box3Answer[0], ShouldTestFailure);

        //Box4
        var Box4Answer = new List<string>() { "yes", "okay", "ok" };
        next = World4State.WaitForClickOnSecondBox;
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
            World4State.Box4_WaitForClickOnFirstBox, World4State.Box4_RepeatPassword, World4Box4SecondBoxClickable));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.Box4_RepeatPassword, World4State.Box4_WaitForFirstPassword));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World4State.Box4_WaitForFirstPassword, next,
            justSayAgainNoVORepeat ? World4State.Box4_WaitForFirstPassword : World4State.Box4_RepeatPassword,
            Box4Answer, false, false, true, true));
        VoiceAnswersMediator.CreateVoiceAnswer(3, (int)World4State.Box4_WaitForFirstPassword, Box4Answer[0], ShouldTestFailure);


        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToClickWorldStateKeyValuePair(this,
            World4State.WaitForClickOnSecondBox, World4State.RepeatSecondPassword, World4SecondBoxClickable));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.RepeatSecondPassword, World4State.WaitForSecondPassword));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World4State.WaitForSecondPassword, World4State.SwimAway,
            justSayAgainNoVORepeat ? World4State.WaitForSecondPassword : World4State.RepeatSecondPassword,
            waitForSecondPasswordAnswers, false, false, true, true));

        VoiceAnswersMediator.CreateVoiceAnswer(3, (int)World4State.WaitForSecondPassword, waitForSecondPasswordAnswers[0], ShouldTestFailure);

        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.WaitForSecondPasswordAgain, World4State.SwimAway));
        WorldToStateDictionary[3].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World4State.SwimAway, World4State.NextWorld));

    }

    private void InitWorld4Dictionary()
    {
        var world = 4;
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World5State.ChangeScreens, World5State.Intro));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World5State.Intro, World5State.SwimAroundAway));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World5State.SwimAroundAway, World5State.TellAStory));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World5State.TellAStory, World5State.SwimOut));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World5State.SwimOut, World5State.NextWorld));
    }


    private void InitWorld5Dictionary()
    {
        var world = 5;
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.ChangeScreens, World6State.Intro));

        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.Intro, World6State.E));
        
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.E, World6State.VoiceE));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World6State.VoiceE, World6State.N, World6State.E, 
            new List<string>(){"Loud"}, false, true));

        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.N, World6State.VoiceN));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World6State.VoiceN, World6State.Th, World6State.N,
            new List<string>() { "Loud" }, false, true));


        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.Th, World6State.VoiceTh));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World6State.VoiceTh, World6State.The, World6State.Th,
            new List<string>() { "Loud" }, false, true));

        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.The, World6State.VoiceThe));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World6State.VoiceThe, World6State.C, World6State.The,
            new List<string>() { "Loud" }, false, true));

        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.C, World6State.VoiceC));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World6State.VoiceC, World6State.A, World6State.C,
            new List<string>() { "Loud" }, false, true));

        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.A, World6State.VoiceA));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World6State.VoiceA, World6State.Ve, World6State.A,
            new List<string>() { "Loud" }, false, true));

        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.Ve, World6State.VoiceVe));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToVoiceWorldStateKeyValuePair(this,
            World6State.VoiceVe, World6State.InTheCave, World6State.Ve,
            new List<string>() { "Loud" }, false, true));

        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.InTheCave, World6State.SwimOut));

        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World6State.SwimOut, World6State.NextWorld));
    }

    private void InitWorld6Dictionary()
    {
        var world = 6;
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World7State.ChangeScreens, World7State.Intro));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World7State.Intro, World7State.SwimOut));
        WorldToStateDictionary[world].Add(WorldStateFactory.GetIntToWorldStateKeyValuePair(this,
            World7State.SwimOut, World7State.NextWorld));
    }

    public void MenuChangeWorld(int worldIndex)
    {
        WorldNumber = worldIndex;
        Menu?.SetActive(false);
        Animator.SetTrigger("ChangeState");
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

    public void ChangeToTempWorldState(WorldState tempWorldState)
    {
        _currentWorldState?.FinishState();
        _currentWorldState = null;

        _currentWorldState = tempWorldState;
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

    public void StartRecording(bool onlyLoudness = false)
    {
        RecordingCanvas.StartRecording(onlyLoudness);
        if (!onlyLoudness)
        {
            TestingVoiceApprove();
        }
    }

    public void StopRecording()
    {
        RecordingCanvas.StopRecording();
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

    void Update()
    {
        _currentWorldState?.Update();
    }
} 
