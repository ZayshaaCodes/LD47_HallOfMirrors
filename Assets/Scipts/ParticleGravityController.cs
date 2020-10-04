using UnityEngine;

public class ParticleGravityController : MonoBehaviour
{
    private ParticleSystem _ps;

    void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        var gravityControllers = FindObjectsOfType<GravityControler>();

        foreach (var gravityController in gravityControllers)
        {
            gravityController.GravityChangedEvent.AddListener(UpdateGravity);
        }
    }

    public void UpdateGravity()
    {
        // Debug.Log("UpdateGravity");
        ParticleSystem.ForceOverLifetimeModule fol = _ps.forceOverLifetime;
        fol.x = new ParticleSystem.MinMaxCurve(Physics2D.gravity.x * .1f);
        fol.y = new ParticleSystem.MinMaxCurve(Physics2D.gravity.y * .1f);
    }
}