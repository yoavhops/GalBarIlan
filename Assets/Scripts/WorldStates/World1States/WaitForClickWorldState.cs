using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForClickWorldState : AnimationWorldState
{
    private OnClick _onClickAble;
    private float _timeTilFailure = 5f;
    private float _currentTimeTilFailure;
    private bool _useNoClickPrompt;

    public WaitForClickWorldState(World1 world1, int myState, int
        NextState, OnClick onClickAble, bool useNoClickPrompt = true) : base(world1, myState, NextState)
    {
        _useNoClickPrompt = useNoClickPrompt;
        _onClickAble = onClickAble;
    }

    public override void StartPart()
    {
        base.StartPart();
        _onClickAble.OnClicked += Clicked;
        _currentTimeTilFailure = _timeTilFailure;
    }

    private void Clicked()
    {
        Success();
    }

    public override void FinishState()
    {
        _onClickAble.OnClicked -= Clicked;
    }


    public override void Update()
    {
        if (!_useNoClickPrompt)
        {
            return;
        }

        _currentTimeTilFailure -= Time.deltaTime;

        if (_currentTimeTilFailure < 0)
        {
            _world1.ChangeToTempWorldState(new FeedBackWorldState(_world1, -1, _thisState, 2, _world1.GeneralStateNoClick()));
        }
    }

}
