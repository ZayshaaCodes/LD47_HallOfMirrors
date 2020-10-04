using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGravityController : MonoBehaviour
{
    private ParticleSystem _ps;

    void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    public void UpdateGravity()
    {
        ParticleSystem.ForceOverLifetimeModule fol = _ps.forceOverLifetime;
        fol.x = new ParticleSystem.MinMaxCurve(Physics2D.gravity.x * .1f);
        fol.y = new ParticleSystem.MinMaxCurve(Physics2D.gravity.y * .1f);
    }
}
