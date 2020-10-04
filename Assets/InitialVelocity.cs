using UnityEngine;

public class InitialVelocity : MonoBehaviour, IDestroyOnClone
{
    public Vector2 velocity;
    public float angularVelocity;
    
    void Start()
    {
        var rb = GetComponent<Rigidbody2D>();

        rb.velocity = velocity;
        rb.angularVelocity = angularVelocity;
    }
}
