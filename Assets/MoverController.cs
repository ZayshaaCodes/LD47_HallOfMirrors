using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverController : MonoBehaviour, IToggle, IDestroyOnClone
{
    private Rigidbody2D _rb;
    private SimpleSprite _ss;

    public WorldButton triggerButton;

    public Vector2 direction = Vector2.up;
    
    public bool enabled;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _ss = GetComponent<SimpleSprite>();

        if (triggerButton)
        {
            triggerButton.ButtonActivateEvent.AddListener(ToggleOn);
            triggerButton.ButtonDeactivateEvent.AddListener(ToggleOff);
        }
    }
    
    private void FixedUpdate()
    {
        if (enabled)
        {
            //_rb.MovePosition(_rb.position + direction * Time.deltaTime);
            _rb.velocity = direction;
        }
    }

    public void ToggleOn(int id)
    {
        enabled = true;
        _ss.SetColor(_ss.color * 2);
    }

    public void ToggleOff(int id)
    {
        enabled = false;
        _rb.velocity = Vector2.zero;
        _ss.SetColor(_ss.color / 2);
    }

    public void Toggle()
    {
        enabled = !enabled;
    }
}
