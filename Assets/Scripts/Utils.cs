using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Utils Singleton;

    void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator ScaledInvoke(Action callback, float delay)
    {
        IEnumerator coroutine = InternalInvoke(callback, delay);
        StartCoroutine(coroutine);
        return coroutine;
    }


    private IEnumerator InternalInvoke(Action callback, float delay)
    {
        while (delay >= 0)
        {
            delay -= Time.deltaTime;
            yield return null;
        }
        callback();
    }

    public static bool IsTouchCollider(Collider2D collider2D)
    {
        if (Input.touchCount == 1)
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (collider2D == Physics2D.OverlapPoint(touchPos))
            {
                return true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (collider2D == Physics2D.OverlapPoint(touchPos))
            {
                return true;
            }
        }
        return false;
    }

}
