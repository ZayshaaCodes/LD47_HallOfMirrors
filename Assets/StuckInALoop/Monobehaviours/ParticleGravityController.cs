using StuckInALoop;
using UnityEngine;

public class ParticleGravityController : MonoBehaviour
{
    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        var gravityControllers = FindObjectsOfType<GravityController>();

        foreach (var gravityController in gravityControllers)
            gravityController.gravityChangedEvent.AddListener(UpdateGravity);
    }

    public void UpdateGravity()
    {
        // Debug.Log("UpdateGravity");
        var fol = _ps.forceOverLifetime;
        fol.x = new ParticleSystem.MinMaxCurve(Physics2D.gravity.x * .1f);
        fol.y = new ParticleSystem.MinMaxCurve(Physics2D.gravity.y * .1f);
    }
}