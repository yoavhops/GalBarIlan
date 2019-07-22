using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackWorldState : WorldState
{

    private float _delay;
    private float _currentDelay;
    private GameObject _gameObject;

    public FeedBackWorldState(World1 world1, int myState, int
        NextState, float delay, GameObject gameObject) :
        base(world1, myState, NextState)
    {
        _delay = delay;
        _gameObject = gameObject;
    }

    public override void OnAnimationEventDone()
    {
        //ignore
    }

    public override void StartPart()
    {
        _currentDelay = _delay;
        var obj = MonoBehaviour.Instantiate(_gameObject);
        obj.SetActive(true);
    }

    public override void Update()
    {
        _currentDelay -= Time.deltaTime;

        if (_currentDelay < 0)
        {
            Success();
        }
    }
    
}
