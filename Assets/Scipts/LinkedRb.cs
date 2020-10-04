using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedRb : MonoBehaviour, ILoopBehaviour
{
    public Rigidbody2D linked;
    private Rigidbody2D _rb;
    private Rigidbody _rb2;

    public Vector2 linkOffset;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        _rb.velocity = linked.velocity;
        _rb.angularVelocity = linked.angularVelocity;
        _rb.position = linked.position + linkOffset;
        _rb.rotation = linked.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleContacts(collision);
    }

    private void HandleContacts(Collision2D collision)
    {
        foreach (ContactPoint2D contactPoint2D in collision.contacts)
        {
            var pos = contactPoint2D.point;
            var normal = contactPoint2D.normalImpulse;
            var tan = contactPoint2D.tangentImpulse;
            var snormal = contactPoint2D.normal;

            var normalForce = snormal * normal;
            var tangentForce = new Vector2(snormal.y, -snormal.x).normalized * tan;

            var localPoint = transform.InverseTransformPoint(contactPoint2D.point);
            var worldForLinkined = linked.transform.TransformPoint(localPoint);
            
            // Debug.DrawRay(worldForLinkined, normalForce, Color.red, 2);
            // Debug.DrawRay(worldForLinkined, tangentForce, Color.blue, 2);

            if (normalForce + tangentForce != new Vector2(Single.NaN, Single.NaN))
            {
                linked.AddForceAtPosition((normalForce + tangentForce), worldForLinkined, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleContacts(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
    }

    public void Loop(Vector2 offset)
    {
        transform.position -= (Vector3) offset;
    }
}