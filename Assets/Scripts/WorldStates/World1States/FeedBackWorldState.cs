using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackWorldState : WorldState
{

    private static List<FeedBackWorldState> _feedBackWorldStates = new List<FeedBackWorldState>();
    private float _delay;
    private float _currentDelay;
    private GameObject _gameObjectPrefab;
    private GameObject _gameObject = null;

    public FeedBackWorldState(World1 world1, int myState, int
        NextState, float delay, GameObject gameObject) :
        base(world1, myState, NextState)
    {
        _delay = delay;
        _gameObjectPrefab = gameObject;
        _feedBackWorldStates.Add(this);
    }

    public override void OnAnimationEventDone()
    {
        //ignore
    }

    public override void StartPart()
    {
        _currentDelay = _delay;
        _gameObject = MonoBehaviour.Instantiate(_gameObjectPrefab);
        _gameObject.SetActive(true);
    }

    public override void Update()
    {
        _currentDelay -= Time.deltaTime;

        if (_currentDelay < 0)
        {
            Success();
        }
    }

    public static void KillAllFeedBacks()
    {

        foreach (var feedbackWorldState in _feedBackWorldStates)
        {
            if (feedbackWorldState._gameObject == null)
            {
                continue;
            }

            MonoBehaviour.Destroy(feedbackWorldState._gameObject);
        }

        _feedBackWorldStates.Clear();

    }


}
