using UnityEngine;

public class GravityControler : MonoBehaviour, IToggle
{
    public Vector2 activeGravity = Vector2.zero;

    private Vector2 _initialGravity;

    private bool _state = false;

    [SerializeField] private bool _changeWorldUp;

    public void ToggleOn()
    {
        if (_state) return;
        _initialGravity = Physics2D.gravity;
        Physics2D.gravity = activeGravity;
        _state = true;
        if (_changeWorldUp) WorldData.instance.SetWorldUp(-activeGravity.normalized);
    }

    public void ToggleOff()
    {
        if (!_state) return;
        Physics2D.gravity = _initialGravity;
        _state = false;
        if (_changeWorldUp) WorldData.instance.SetWorldUp(-_initialGravity.normalized);

    }

    public void Toggle()
    {
        _state = !_state;
        if (_state)
        {
            ToggleOn();
        }
        else
        {
            ToggleOff();
        }
    }
}

public interface IToggle
{
    public void ToggleOn();
    public void ToggleOff();
    public void Toggle();
}