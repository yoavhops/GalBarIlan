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
    public Animator Animator;
    public RecordingCanvas RecordingCanvas;

    void Awake()
    {
        ChangeWorldState(World1State);
        RecordingCanvas.OnFinalResults += OnVoiceResults;
        RecordingCanvas.OnVoiceEndOfSpeech += OnVoiceEndOfSpeech;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeWorldState(World1State world1State)
    {
        World1State = world1State;
        ChangeAnimatorPart((int)World1State);

        switch (world1State)
        {
            case World1State.WaitForVoiceApprove:
                StartPartWaitForVoiceApprove();
                break;
        }

    }

    public void ChangeAnimatorPart(int part)
    {
        Animator.SetInteger("Part", part);
    }



    /**** voices handlers ***/

    private void OnVoiceResults(string result)
    {
        switch (World1State)
        {
            case World1State.WaitForVoiceApprove:
                if (string.Equals(result, "yes", StringComparison.OrdinalIgnoreCase))
                {
                    ChangeWorldState(World1State.SwimAway);
                }
                else
                {
                    ChangeWorldState(World1State.WaitForVoiceApproveAgain);
                }
                break;
        }
    }

    private void OnVoiceEndOfSpeech()
    {
        if (World1State == World1State.WaitForVoiceApprove)
        {
            ChangeWorldState(World1State.WaitForVoiceApproveAgain);
        }
    }


    /******* Parts Start **********/
    private void StartPartWaitForVoiceApprove()
    {
        RecordingCanvas.StartRecording();
        TestingVoiceApprove();
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
                OnVoiceResults("No");
            }, 3f);
        }
        else
        {
            Utils.Singleton.ScaledInvoke(() =>
            {
                OnVoiceResults("Yes");
            }, 3f);
        }
    }



    /****** Parts done******/

    public void OnAnimationEventPart1Done()
    {
        ChangeWorldState(World1State.WaitForClickOnStarFish);
    }

    public void OnStarFishClicked()
    {
        if (World1State == World1State.WaitForClickOnStarFish)
        {
            ChangeWorldState(World1State.SealSwimAway);
        }
    }

    public void OnAnimationEventSealSwimAwayDone()
    {
        ChangeWorldState(World1State.WaitForVoiceApprove);
    }

    public void OnAnimationEventWaitForVoiceApproveAgainDone()
    {
        ChangeWorldState(World1State.WaitForVoiceApprove);
    }

}
