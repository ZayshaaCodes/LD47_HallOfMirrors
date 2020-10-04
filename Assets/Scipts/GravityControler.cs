using System;
using UnityEngine;
using UnityEngine.Events;

public class GravityControler : MonoBehaviour, IToggle, IDestroyOnClone
{
    public WorldButton triggerButton;

    public Vector2 activeGravity = Vector2.up;
    public Vector2 revertGravity = Vector2.down;

    public bool revertGravityOnDepress = true;

    public UnityEvent GravityChangedEvent;

    // private Vector2 _initialGravity;

    private bool _state = false;

    [SerializeField] private bool _changeWorldUp;

    private void Awake()
    {
        if (triggerButton == null)
        {
            triggerButton = GetComponent<WorldButton>();
        }

        if (triggerButton)
        {
            triggerButton.ButtonActivateEvent.AddListener(ToggleOn);
            triggerButton.ButtonDeactivateEvent.AddListener(ToggleOff);
        }
    }

    public void ToggleOn(int id)
    {
        if (revertGravityOnDepress)
        {
            revertGravity = Physics2D.gravity;
        }
        if (_state) return;
        _state = true;

        Physics2D.gravity = activeGravity;
        if (_changeWorldUp) WorldData.instance.SetWorldUp(-activeGravity.normalized);

        GravityChangedEvent.Invoke();
    }

    public void ToggleOff(int id)
    {
        if (!_state) return;
        _state = false;

        if (revertGravityOnDepress)
        {
            Physics2D.gravity = revertGravity;
            if (_changeWorldUp) WorldData.instance.SetWorldUp(-revertGravity.normalized);
            GravityChangedEvent.Invoke();
        }
    }

    public void Toggle()
    {
        _state = !_state;
        if (_state)
        {
            ToggleOn(0);
        }
        else
        {
            ToggleOff(0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3) activeGravity * .1f);
    }
}

public interface IToggle
{
    public void ToggleOn(int id);
    public void ToggleOff(int id);
    public void Toggle();
}