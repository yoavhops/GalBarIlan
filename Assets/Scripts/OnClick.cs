using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnClick : MonoBehaviour
{
    public Collider2D Collider2D;
    public Action OnClicked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Utils.IsTouchCollider(Collider2D))
        {
            OnClicked?.Invoke();
        }
    }
    
}
