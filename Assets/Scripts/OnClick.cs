using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnClick : MonoBehaviour
{

    public UnityEvent Callback;
    public Collider2D Collider2D;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Utils.IsTouchCollider(Collider2D))
        {
            OnClicked();
        }
    }
    
    public void OnClicked()
    {
        Callback?.Invoke();
    }
}
