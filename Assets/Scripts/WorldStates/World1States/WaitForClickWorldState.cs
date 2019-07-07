using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForClickWorldState : AnimationWorldState
{
    private OnClick _onClickAble;

    public WaitForClickWorldState(World1 world1, int myState, int
        NextState, OnClick onClickAble) : base(world1, myState, NextState)
    {
        _onClickAble = onClickAble;
    }

    public override void StartPart()
    {
        base.StartPart();
        _onClickAble.OnClicked += Clicked;
    }

    private void Clicked()
    {
        Success();
    }

    public override void FinishState()
    {
        _onClickAble.OnClicked -= Clicked;
    }

}
